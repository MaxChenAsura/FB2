<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sm/WFB2SM1300_Qry.aspx.cs" Inherits="WebContent_fb2sm_WFB2SM1300_Qry" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            gridviewScroll();
            $.unblockUI();
            $("#txt_DATA_YEAR_search").mask('9999');
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
        function ClearAll() {
            $('#txt_DATA_YEAR_search').val("");
            return false;
        }
      
        function CheckReleaseAction() {
            if (LookUpCheckboxs() == 1) {
                return true;
            }
            else {
                alert($('#hidwfb2sm_Release_NotChoiceMessage').val());
                return false;
            }
        }
        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2sm_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
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
        function ReleaseConfirmAfter() {
            if (confirm($('#hidwfb2sm_ReleaseConfirm_message').val())) {
                $('#btn_confirmRelease').click();
            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="20%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YEAR_search" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_DATA_YEAR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DATA_YEAR_search" runat="server" MaxLength="4" Width="80px" ClientIDMode="Static"></asp:TextBox>
                                         <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_lb_DATA_YM_isError%>"
                                            ControlToValidate="txt_DATA_YEAR_search" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SM1300Search" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM1200Search%>" OnClick="WFB2SM1300Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                            <%--<asp:Button ID="WFB2SM1300Search" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM1200Search%>" OnClick="WFB2SM1300Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                                            <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,wfb2sm_btn_clear%>" OnClientClick="return ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <asp:Button ID="btn_confirmRelease" runat="server" OnClick="btn_confirmRelease_Click" ClientIDMode="Static" Style="display: none" />
                            <aces:Btn ID="WFB2SM1300Release" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM130Release%>" OnClientClick="return CheckReleaseAction();" OnClick="WFB2SM1300Release_Click" Visible="false" />
                            <aces:Btn ID="WFB2SM1300Detail" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM1200Detail%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2SM1300Detail_Click" Visible="false" />
                           <%-- <asp:Button ID="WFB2SM1300Release" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM130Release%>" OnClientClick="return CheckReleaseAction();" OnClick="WFB2SM1300Release_Click" Visible="false" />
                            <asp:Button ID="WFB2SM1300Detail" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM1200Detail%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2SM1300Detail_Click" Visible="false" />--%>
                        </div>

                    </td>
                </tr>
                <tr></tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SM1300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DATA_YEAR_search"
                        Name="data_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_firstLoad"
                        Name="firstLoad" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="false" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sm_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--年度--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_DATA_YEAR%>" SortExpression="DATA_YEAR" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DATA_YEAR" runat="server" Text='<%#Bind("DATA_YEAR")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--晉升回數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm__lbDATA_SEQ%>" SortExpression="DATA_SEQ" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DATA_SEQ" runat="server" Text='<%#Bind("DATA_SEQ")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資料生成日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_GENERATE_DT%>" SortExpression="GENERATE_DT" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_GENERATE_DT" runat="server" Text='<%#Bind("GENERATE_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--提出核可日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_NOTICE_DT_Qry%>" SortExpression="NOTICE_DT" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_NOTICE_DT" runat="server" Text='<%#Bind("NOTICE_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--核可日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_APPROVE_DT%>" SortExpression="APPROVE_DT" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發佈日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_RELEASE_DT%>" SortExpression="RELEASE_DT" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_RELEASE_DT" runat="server" Text='<%#Bind("RELEASE_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--生效日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_EXECUTIVE_DT%>" SortExpression="EXECUTIVE_DT" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EXECUTIVE_DT" runat="server" Text='<%#Bind("EXECUTIVE_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                            <%--核可狀態--%>
                            <asp:HiddenField ID="hid_PROCESS_STATUS" runat="server" Value='<%#Bind("PROCESS_STATUS")%>' />
                        </ItemTemplate>
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
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_firstLoad" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sm_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sm_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sm_Release_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sm_Release_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sm_ReleaseConfirm_message" Value="<%$Resources:Resource,wfb2sm_ReleaseConfirm_message%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sm_ReleaseSuccess_message" Value="<%$Resources:Resource,wfb2sm_ReleaseSuccess_message%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_message" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_qdatakey" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


