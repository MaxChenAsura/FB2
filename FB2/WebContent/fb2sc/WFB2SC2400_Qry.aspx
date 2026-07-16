<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2400_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2400_Qry" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ymd').mask('9999/99/99');
            $('.ym').mask('9999/99');
            gridviewScroll();
            $.unblockUI();
            $('#txt_SALARY_NAME').attr("readonly", true);

            //工號取得姓名的ajax
            $("#txt_EMP_ID_search").change(function () {
                if ($("#txt_EMP_ID_search").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID_search').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME_search').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME_search').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME_search').val("");
                }
            });
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F",
                headerrowcount: 1,
                freezesize: 8
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }
        function ClearAll() {
            $('#ddl_SALARY_TYPE_search').val("");
            $('#txt_SALARY_DT_search').val("");
            $('#txt_SALARY_ID_search').val("");
            $('#txt_SALARY_NAME').val("");
            $('#ddl_PROCESS_STATUS_search').val("");
            $('#txt_EMP_ID_search').val("");
            $('#txt_EMP_NAME_search').val("");
            return false;
        }
        function CheckApproveAction() {
            if (LookUpCheckboxs() > 0) {
                if (confirm($("#hidwfb2sc_Approve_ConfirmMessage").val())) {
                    BlockUI();
                    return true;
                }
                else
                    return false;
            }
            else {
                alert($("#hidwfb2sc_action_NotChoiceMessage").val());
                return false;
            }
        }
        function CheckRejectAction() {
            if (LookUpCheckboxs() > 0) {
                BlockUI();
                return true;
            }
            else {
                alert($("#hidwfb2sc_action_NotChoiceMessage").val());
                return false;
            }
        }
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("_freezeitem") != -1 && ItemCheckBoxs[i].id.indexOf("_freezeheader") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function rejectConfirm() {
            if (confirm($("#hidwfb2sc_Reject_ConfirmMessage").val())) {
                BlockUI();
                $("#btn_rejectCheck").click();
            }
            else
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
                                    <%--發薪類別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_TYPE_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_TYPE_search" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <%--發薪日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_DT_search" runat="server" MaxLength="8" CssClass="ymd date" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_SALARY_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_DT_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--薪資項目--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ID_search" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_ID_search" runat="server" ClientIDMode="Static" Width="64px" MaxLength="4" OnTextChanged="txt_SALARY_ID_search_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <input id="bt_SALARY_ID" type="button" value="..." onclick="OpenSearch('SalaryItem_Search.aspx', 'txt_SALARY_ID_search', 'txt_SALARY_NAME', '', '');" runat="server" />
                                        <asp:TextBox ID="txt_SALARY_NAME" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static" Width="100px"></asp:TextBox>
                                    </td>
                                    <%--處理狀態--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PROCESS_STATUS_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_PROCESS_STATUS_search" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--工號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID_search" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_search', 'txt_EMP_NAME_search', 'N', '');" />
                                    </td>
                                    <%--姓名--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_NAME2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME_search" runat="server" MaxLength="30" ClientIDMode="Static" Width="64px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <%--<aces:Btn ID="WFB2SC2400Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Search%>" OnClick="WFB2SC2400Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                                            <asp:Button ID="WFB2SC2400Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Search%>" OnClick="WFB2SC2400Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                            <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,wfb2sc_btn_clear%>" OnClientClick="return ClearAll();" />
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
                            <aces:Btn ID="WFB2SC2400Approve" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2400Approve%>" OnClick="WFB2SC2400Approve_Click" OnClientClick="return CheckApproveAction();" Visible="False" />
                            <aces:Btn ID="WFB2SC2400Reject" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2400Reject%>" OnClick="WFB2SC2400Reject_Click" OnClientClick="return CheckRejectAction();" Visible="False" />
                            <%--<asp:Button ID="WFB2SC2400Approve" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2400Approve%>" OnClick="WFB2SC2400Approve_Click" OnClientClick="return CheckApproveAction();" Visible="False" />
                            <asp:Button ID="WFB2SC2400Reject" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2400Reject%>" OnClick="WFB2SC2400Reject_Click" OnClientClick="return CheckRejectAction();" Visible="False" />--%>
                        </div>

                    </td>
                </tr>
                <tr></tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SC2400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_SALARY_TYPE_search"
                        Name="salary_type" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SALARY_DT_search"
                        Name="salary_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SALARY_ID_search"
                        Name="salary_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_PROCESS_STATUS_search"
                        Name="process_status" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID_search"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME_search"
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資料年月--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_DATA_YM%>" SortExpression="DATA_YM" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lb_DATA_YM" Width="80px" Style="text-align: center;" runat="server" CssClass="ym" ClientIDMode="Static" Text='<%#Bind("DATA_YM")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發薪日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>" SortExpression="SALARY_DT" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                        <ItemTemplate>
                            <div style="text-align: left;">
                                <asp:Label ID="lb_SALARY_DT" runat="server" Width="90px" Style="text-align: center;" Text='<%#Bind("SALARY_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發薪類別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>" SortExpression="SALARY_TYPE" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: left;">
                                <asp:Label ID="lb_SALARY_TYPE" runat="server" Width="100px" Style="text-align: left;" Text='<%#Bind("SALARY_TYPE_DESC")%>'></asp:Label>

                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Width="60px" Style="text-align: center;" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_EMP_NAME2%>" SortExpression="EMP_NAME" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Width="70px" Style="text-align: center;" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--薪資項目--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_SALARY_ID%>" SortExpression="SALARY_ID" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_ID" runat="server" Width="120px" Style="text-align: left;" Text='<%#Bind("SALARY_NAME_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發放項目--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>" SortExpression="PAY_KIND" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_PAY_KIND" runat="server" Width="80px" Style="text-align: left;" Text='<%#Bind("PAY_KIND_NAME")%>'></asp:Label>
                            <asp:HiddenField ID="hid_SALARY_TYPE" runat="server" Value='<%#Bind("SALARY_TYPE")%>' />
                            <asp:HiddenField ID="hid_SALARY_NAME" runat="server" Value='<%#Bind("SALARY_NAME")%>' />
                            <asp:HiddenField ID="hid_SALARY_ID" runat="server" Value='<%#Bind("SALARY_ID")%>' />
                            <asp:HiddenField ID="hid_IS_PLUS" runat="server" Value='<%#Bind("IS_PLUS")%>' />
                            <asp:HiddenField ID="hid_IS_TAX" runat="server" Value='<%#Bind("IS_TAX")%>' />
                            <asp:HiddenField ID="hid_TAX_FORMAT" runat="server" Value='<%#Bind("TAX_FORMAT")%>' />
                            <asp:HiddenField ID="hid_PAY_TYPE" runat="server" Value='<%#Bind("PAY_TYPE")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--異動狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_CHG_STATUS%>" SortExpression="CHG_STATUS" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_CHG_STATUS" runat="server" Width="80px" Style="text-align: center;" Text='<%#Bind("CHG_STATUS_DESC")%>'></asp:Label>
                            <asp:HiddenField ID="hid_CHG_STATUS" runat="server" Value='<%#Bind("CHG_STATUS")%>' />
                            <asp:HiddenField ID="hid_PAY_KIND" runat="server" Value='<%#Bind("PAY_KIND")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--異動前金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_CHG_AMT_B%>" SortExpression="CHG_AMT_B" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_CHG_AMT_B" runat="server" Width="80px" Style="text-align: right;" Text='<%#Bind("CHG_AMT_B","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--異動後金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_CHG_AMT_A%>" SortExpression="CHG_AMT_A" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_CHG_AMT_A" runat="server" Width="80px" Style="text-align: right;" Text='<%#Bind("CHG_AMT_A","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--處理狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS2%>" SortExpression="PROCESS_STATUS" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_PROCESS_STATUS" runat="server" Width="80px" Style="text-align: left;" Text='<%#Bind("PROCESS_STATUS_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--異動人員--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_CREATED_BY%>" SortExpression="CREATED_BY" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_CREATED_BY" runat="server" Width="80px" Style="text-align: center;" Text='<%#Bind("UPDATED_BY_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--備註說明--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lb_REMARK" runat="server" Width="150px" Style="text-align: left; word-wrap: break-word;" Text='<%#Bind("REMARK")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--主管核定備註--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_APP_REMARK%>" SortExpression="APP_REMARK" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_APP_REMARK" runat="server" Width="100%" TextMode="MultiLine" MaxLength="100" Rows="2" Style="text-align: left;" Text='<%#Bind("APP_REMARK")%>'></asp:TextBox>
                            <asp:HiddenField ID="hid_SEQ_NO" runat="server" Value='<%#Bind("SEQ_NO")%>' />
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
            <asp:Button ID="btn_rejectCheck" runat="server" Style="display: none" ClientIDMode="Static" OnClick="btn_rejectCheck_Click" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Reject_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Reject_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Approve_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Approve_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_action_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_action_NotChoiceMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


