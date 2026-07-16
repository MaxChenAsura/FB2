<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2se/WFB2SE2100_Qry.aspx.cs" Inherits="WebContent_fb2se_WFB2SE2100_Qry" Culture="auto" UICulture="auto"%>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date2").mask('9999/99');
            $(".ym").mask('9999/99');
              gridviewScroll();
            $.unblockUI();

        }
        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hid_wfb2se_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        //檢核不調薪對象生成
        function CheckExecute() {
            if (LookUpCheckboxs() == 1) {
                return true;
            }
            else {
                alert("請選取一筆資料!");
                return false;
            }

            var processed = true;
            BlockUI();
           
            processed = confirm("不調薪對象只能執行一次,是否確認要執行？");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
        //檢查勾了幾個checkbox
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        //清空
        function doClear() {
            $("#txt_EFFECT_YM").val("");
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="5%" />
                                <col width="45%" />
                                <col width="15%" />
                                <col width="35%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EFFECT_YM" runat="server" Text="<%$Resources:Resource,wfb2_EFFECT_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EFFECT_YM" runat="server" MaxLength="7" Width="64px" CssClass="date2" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2_START_DATE_FORMAT_ERROR%>" ControlToValidate="txt_EFFECT_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="Body_label" colspan="3">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SE2100Search" runat="server" Text="<%$Resources:Resource,wfb2si_search%>" OnClick="WFB2SE2100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />

                                            <%--<asp:Button ID="WFB2SE2100Search" runat="server" Text="<%$Resources:Resource,wfb2si_search%>" OnClick="WFB2SE2100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2si_clear%>" OnClientClick="return doClear();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFB2SE2100Detail" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Detail%>" OnClick="WFB2SE2100Detail_Click" Visible="false" OnClientClick="return CheckDtlAction(); BlockUI();" />
                            <aces:Btn ID="WFB2SE2100Process" runat="server" Text="<%$Resources:Resource,wfb2se_bt_WFB2SE2100Process%>" OnClick="WFB2SE2100Process_Click" Visible="false" OnClientClick="return CheckExecute(); BlockUI();" />

                            <%--<asp:Button ID="WFB2SE2100Detail" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Detail%>" OnClick="WFB2SE2100Detail_Click" Visible="false" OnClientClick="return CheckDtlAction(); BlockUI();" />
                            <asp:Button ID="WFB2SE2100Process" runat="server" Text="<%$Resources:Resource,wfb2se_bt_WFB2SE2100Process%>" OnClick="WFB2SE2100Process_Click" Visible="false" OnClientClick="return CheckExecute(); BlockUI();" />--%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData" SelectCountMethod="GetCount" TypeName="CFB2SE2100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting" StartRowIndexParameterName="startRowIndex"  OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EFFECT_YM" DefaultValue="" Name="effect_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" >
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="text-align: center">
                                <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px" >
                        <ItemTemplate>
                            <asp:Label Style="text-align: right;" Width="40px" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_EFFECT_YM%>" SortExpression="EFFECT_YM"  HeaderStyle-Width="80px" >
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_EFFECT_ym" runat="server" Text='<%#Bind("EFFECT_YM")%>' CssClass="ym"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_SUB_DESC%>" SortExpression=""  HeaderStyle-Width="100px" >
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: left;" ID="lb_APPROVE_STATUS_NAME" runat="server" Text='<%#Bind("APPROVES_STATUS_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_DATA_CNT%>" SortExpression=""  HeaderStyle-Width="80px" >
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_NOADJ_CNT" runat="server" Text='<%#Bind("NOADJ_CNT", "{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_lb_MEM_CREATE_BY%>" SortExpression="" HeaderStyle-Width="140px" >
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: left;" ID="lb_MEM_CREATE_BY_NAME" runat="server" Text='<%#Bind("MEM_CREATE_BY_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_lb_MEM_CREATE_DT%>" SortExpression="" HeaderStyle-Width="140px" >
                        <ItemTemplate>
                            <asp:Label Width="140px" Style="text-align: left;" ID="lb_MEM_CREATE_DT" runat="server" ClientIDMode="Static" Text='<%#Bind("MEM_CREATE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
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

            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2se_Dtl_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sa_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2se_Execute_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sa_Execute_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2se_Execute_alreadyMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2se_Execute_alreadyMessage%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>