<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sa/action/WFB2SA1500_Qry.aspx.cs" Inherits="WebContent_fb2sa_WFB2SA1500_Qry" Culture="auto" UICulture="auto"%>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $('.year').mask('9999');
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
                alert($('#hid_wfb2sa_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        function CheckGen() {
            if (LookUpCheckboxs() == 1) {
                BlockUI();
                return true;
            }
            else {
                alert($('#hid_wfb2sa_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        //檢核異動對象生成
        function CheckExecute() {
            if (LookUpCheckboxs() == 1) {
                BlockUI();
                choietr = choietr - 2;
                //對象生成
                if ($("[id=lb_MEM_CREATE_DT]")[choietr].innerText != "") {
                    alert($('#hid_wfb2sa_Execute_alreadyMessage').val());
                    $.unblockUI();
                    return false;
                }
            }
            else {
                alert($('#hid_wfb2sa_Execute_NotChoiceMessage').val());
                $.unblockUI();
                choietr = null;
                return false;
            }
        }
        var choietr = null;
        //檢查勾了幾個checkbox
        function LookUpCheckboxs() {
            choietr = null;
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    if (choietr == null)
                        choietr = i;
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        //清空
        function doClear() {
            var now = new Date();
            var Current = now.getFullYear();
            $("#txt_DATA_YEAR").val(Current);
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="5%" />
                                <col width="15%" />
                                <col width="20%" />
                                <col width="20%" />

                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_DATA_YEAR1%>"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_DATA_YEAR" runat="server" MaxLength="4" Width="64px" CssClass="year" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2sa_error_DATA_YEAR1%>" ControlToValidate="txt_DATA_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>

                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <%--<asp:Btn ID="WFB2SA1500Search1" runat="server" Text="<%$Resources:Resource,wfb2si_search%>" OnClick="WFB2SA1500Search1_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>

                                            <asp:Button ID="WFB2SA1500Search1" runat="server" Text="<%$Resources:Resource,wfb2si_search%>" OnClick="WFB2SA1500Search1_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
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
                            <aces:Btn ID="WFB2SA1500Detail" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Detail%>" OnClick="WFB2SA1500Detail_Click" Visible="false" OnClientClick="return CheckDtlAction(); BlockUI();" />
                            <aces:Btn ID="WFB2SA1500Execute" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Execute%>" OnClick="WFB2SA1500Execute_Click" Visible="false" OnClientClick="return CheckExecute();" />
                            
                            
                            <aces:Btn ID="WFB2SA1500Execute2" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Execute2%>"  Visible="false" OnClick="WFB2SA1500Execute2_Click" OnClientClick="return CheckGen();" />
                                <aces:Btn ID="WFB2SA1500Print" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Print%>"  Visible="false" OnClick="WFB2SA1500Print_Click"  />
                                <aces:Btn ID="WFB2SA1500Generate" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Generate%>"  Visible="false" OnClick="WFB2SA1500Generate_Click" OnClientClick="return CheckGen();" />

                            <%--
                                new 2階
                                <aces:Btn ID="WFB2SA1500Execute2" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Execute2%>"  Visible="false" OnClick="WFB2SA1500Execute2_Click" OnClientClick="return CheckGen();" />
                                <aces:Btn ID="WFB2SA1500Print" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Print%>"  Visible="false" OnClick="WFB2SA1500Print_Click"  />
                                <aces:Btn ID="WFB2SA1500Generate" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Generate%>"  Visible="false" OnClick="WFB2SA1500Generate_Click" OnClientClick="return CheckGen();" />

                                <asp:Button ID="WFB2SA1500Execute2" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Execute2%>"  Visible="false" OnClick="WFB2SA1500Execute2_Click" OnClientClick="return CheckGen();" />
                            <asp:Button ID="WFB2SA1500Print" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Print%>"  Visible="false" OnClick="WFB2SA1500Print_Click"  />
                            <asp:Button ID="WFB2SA1500Generate" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Generate%>"  Visible="false" OnClick="WFB2SA1500Generate_Click" OnClientClick="return CheckGen();" />
                                end
                                <asp:Button ID="WFB2SA1500Detail" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Detail%>" OnClick="WFB2SA1500Detail_Click" Visible="false" OnClientClick="return CheckDtlAction(); BlockUI();" />

                                <asp:Button ID="WFB2SA1500Execute" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Execute%>" OnClick="WFB2SA1500Execute_Click" Visible="false" OnClientClick="return CheckExecute();" />--%>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                SelectCountMethod="GetCount" TypeName="CFB2SA1500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DATA_YEAR" DefaultValue=""
                        Name="data_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_DATA_YEAR1%>" SortExpression="DATA_YEAR">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_DATA_YEAR" runat="server" Text='<%#Bind("DATA_YEAR")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_PROCESS_STATUS1%>" SortExpression="PROCESS_STATUS">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_PROCESS_STATUS" runat="server" Text='<%#Bind("PROCESS_STATUS")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_START_DT%>" SortExpression="START_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_END_DT%>" SortExpression="END_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_END_DT" runat="server" Text='<%#Bind("END_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_PROCESS_DT%>" SortExpression="APPROVE_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_APPROVE_DT" runat="server" ClientIDMode="Static" Text='<%#Bind("APPROVE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_DATA_CNT%>" >
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_DATA_CNT" runat="server" Text='<%#Bind("DATA_CNT", "{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_MEM_CREATE_BY%>" SortExpression="MEM_CREATE_BY">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_MEM_CREATE_BY" runat="server" Text='<%#Bind("DESC1")%>'></asp:Label>
                            <asp:HiddenField ID="hid_EMP_NAME" runat="server" ClientIDMode="Static" Value='<%#Bind("EMP_NAME")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_MEM_CREATE_DT%>" SortExpression="MEM_CREATE_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_MEM_CREATE_DT" runat="server" ClientIDMode="Static" Text='<%#Bind("MEM_CREATE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>

                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_checkall" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                            </td>
                            <td>
                                <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe2" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_DATA_YEAR1%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe3" runat="server" Text="<%$Resources:Resource,wfb2sa_PROCESS_STATUS1%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2sa_START_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sa_END_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sa_PROCESS_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2sa_DATA_CNT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2sa_MEM_CREATE_BY%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sa_MEM_CREATE_DT%>"></asp:Label>
                            </td>
                        </tr>
                    </table>

                </EmptyDataTemplate>
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
            <asp:HiddenField ID="hid_wfb2sa_Dtl_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sa_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sa_Execute_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sa_Execute_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sa_Execute_alreadyMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sa_Execute_alreadyMessage%>" />
            <%--<asp:HiddenField ID="hid_wfb2si_Announce_Message" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Del_ConfirmMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Announce_ReleaseMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Gen_dt_NotReleaseMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Release_ConfirmMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Release_NotChoiceMessage" runat="server" ClientIDMode="Static" />--%>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SA1500Print" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>