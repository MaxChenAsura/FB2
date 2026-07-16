<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA3400_Qry.aspx.cs" Inherits="WebContent_fb2ia_WFB2IA3400_Qry" Culture="auto" UICulture="auto"%>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            //$(".number").formatNumber({ format: "#,###", locale: "tw" });
            $('.empid').mask('99999');
            gridviewScroll();
            $.unblockUI();
            $('#txt_EMP_NAME').attr("readonly", true);
            //工號取得姓名的ajax
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME').val("");
                }
            });
        }

        function CheckAPPROVE_STATUS(source, arguments) {
            if ($("#ddl_APPROVE_STATUS").val() == -1)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        //凍結視窗
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F",
                    headerrowcount: 1,
                    freezesize: 7

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }

        }

        function CheckApproveAction() {
            if (LookUpCheckboxs() == 0) {
                alert($('#hid_wfb2ia_App_NotChoiceMessage').val());
                return false;
            }
            else {
                return confirm($('#hid_wfb2ia_Approve_ConfirmMessage').val());
            }
        }
        function CheckRejectAction() {

            if (LookUpCheckboxs() == 0) {
                alert($('#hid_wfb2ia_App_NotChoiceMessage').val());
                return false;
            }
        }
        function checkRejectClick() {
            if (confirm($('#hid_wfb2ia_Reject_ConfirmMessage').val())) {
                $('#Reject_AfterConfirm').click();
            }
            else {
                return false;
            }
        }
        var ItemCheckBoxs = null;
        //檢查勾了幾個checkbox
        function LookUpCheckboxs() {
            ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("_freezeitem") != -1 && ItemCheckBoxs[i].id.indexOf("_freezeheader") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function empCheck(value) {
            if(value!="")
                __doPostBack('question', 'true');
            else
                $("#txt_EMP_NAME").val("");
        }
        //function APPROVE_STATUS_Choose() {
        //    $("#HID_APPROVE_STATUS").val($("#ddl_APPROVE_STATUS").val());
            
        //}
        //清空
        function doClear() {
            $("#txt_SALARY_YM").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_APPROVE_STATUS").val("-1");
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--頁面table-->
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="10%" />
                                <col width="15%" />
                                <col width="15%" />
                                <col width="15%" />
                                <col width="30%" />
                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2ia_SALARY_YM%>"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_SALARY_YM" runat="server" MaxLength="6" Width="64px" CssClass="ym date" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_SALARY_YM%>" ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="empid" ></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPROVE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2ia_APPROVE_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_APPROVE_STATUS" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_Rrequired_APPROVE_STATUS%>" ClientValidationFunction="CheckAPPROVE_STATUS" ForeColor="Red"
                                            ControlToValidate="ddl_APPROVE_STATUS" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                           <aces:Btn ID="WFB2IA3400Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2IA3400Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <aces:Btn ID="WFB2IA3400Approve" runat="server" Text="<%$Resources:Resource,wfb2ia_approve%>" OnClick="WFB2IA3400Approve_Click" OnClientClick="return CheckApproveAction();" ClientIDMode="Static" />
                                            <aces:Btn ID="WFB2IA3400Reject" runat="server" Text="<%$Resources:Resource,wfb2ia_reject%>" OnClick="WFB2IA3400Reject_Click" OnClientClick="return CheckRejectAction();" ClientIDMode="Static" />

                                            <%--<asp:Button ID="WFB2IA3400Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2IA3400Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sk_clear%>" OnClientClick="return doClear();" />
                                            <%--<asp:Button ID="WFB2IA3400Approve" runat="server" Text="<%$Resources:Resource,wfb2ia_approve%>" OnClick="WFB2IA3400Approve_Click" OnClientClick="return CheckApproveAction();" ClientIDMode="Static" />
                                            <asp:Button ID="WFB2IA3400Reject" runat="server" Text="<%$Resources:Resource,wfb2ia_reject%>" OnClick="WFB2IA3400Reject_Click" OnClientClick="return CheckRejectAction();" ClientIDMode="Static" />--%>
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
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                SelectCountMethod="GetCount" TypeName="CFB2IA3400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_SALARY_YM" DefaultValue=""
                        Name="salary_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_APPROVE_STATUS" DefaultValue=""
                        Name="approve_status" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderStyle-Width="5px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField >
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderStyle-Width="40px" HeaderText="<%$Resources:Resource,wfb2sk_RowNum%>">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_SALARY_YM%>" HeaderStyle-Width="60px" SortExpression="SALARY_YM">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_SALARY_YM" runat="server" Text='<%#Bind("SALARY_YM", "{0:yyyy/MM}")%>' CssClass="ym"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_SUB_DESC_INS%>" HeaderStyle-Width="60px" SortExpression="INS_TYPE_DESC">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_SUB_DESC_INS" runat="server" Text='<%#Bind("INS_TYPE_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_ID%>" HeaderStyle-Width="50px" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_SUB_DESC_IDENTITY%>" HeaderStyle-Width="50px" SortExpression="IDENTITY_KIND_DESC">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_SUB_DESC_IDENTITY"  runat="server" Text='<%#Bind("IDENTITY_KIND_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>" HeaderStyle-Width="100px" SortExpression="LICENSE_ID">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_NAME%>" HeaderStyle-Width="60px" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_SUB_DESC_TRACE_KIND%>" HeaderStyle-Width="60px" SortExpression="TRACE_KIND_DESC">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_SUB_DESC_TRACE_KIND" ClientIDMode="Static" runat="server" Text='<%#Bind("TRACE_KIND_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_SUB_DESC_TRACE_TYPE%>" HeaderStyle-Width="60px" SortExpression="TRACE_TYPE_DESC">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_SUB_DESC_TRACE_TYPE" ClientIDMode="Static" runat="server" Text='<%#Bind("TRACE_TYPE_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_TRACE_AMT%>" HeaderStyle-Width="60px" SortExpression="TRACE_AMT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_TRACE_AMT" ClientIDMode="Static" runat="server" Text='<%#Bind("TRACE_AMT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sk_REMARK%>" HeaderStyle-Width="100px" SortExpression="REMARK">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_APP_REMARK%>" HeaderStyle-Width="120px" SortExpression="APP_REMARK">
                        <ItemTemplate>
                            <asp:TextBox Width="90%" Style="text-align: left;" ID="txt_APP_REMARK" MaxLength="100"  ClientIDMode="Static" runat="server" Text='<%#Bind("APP_REMARK")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_APP_REMARK%>" SortExpression="qdatakey2" Visible="false">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_qdatakey2" ClientIDMode="Static" runat="server" Text='<%#Bind("qdatakey2")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>

                    <table id="t1" class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_checkall" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Width="40px" Text="<%$Resources:Resource,wfb2sk_RowNum%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Width="80px" Text="<%$Resources:Resource,wfb2ia_SALARY_YM%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Width="100px" Text="<%$Resources:Resource,wfb2ia_SUB_DESC_INS%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Width="80px" Text="<%$Resources:Resource,wfb2ia_SUB_DESC_IDENTITY%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Width="100px" Text="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Width="60px" Text="<%$Resources:Resource,wfb2ia_EMP_NAME%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Width="100px" Text="<%$Resources:Resource,wfb2ia_SUB_DESC_TRACE_TYPE%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Width="100px" Text="<%$Resources:Resource,wfb2ia_TRACE_AMT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Width="60px" Text="<%$Resources:Resource,wfb2sk_REMARK%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Width="120px" Text="<%$Resources:Resource,wfb2ia_APP_REMARK%>"></asp:Label>
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
                            <asp:ListItem Text="每頁10000筆" Value="10000"></asp:ListItem>
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
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <%--<asp:HiddenField ID="HID_APPROVE_STATUS" runat="server" ClientIDMode="Static" />--%>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2ia_App_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2ia_App_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2ia_APP_REMARK_RequiredMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2ia_APP_REMARK_RequiredMessage%>" />
        <asp:HiddenField ID="hid_wfb2ia_Approve_ConfirmMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2ia_Approve_ConfirmMessage%>" />
            <asp:HiddenField ID="hid_wfb2ia_Reject_ConfirmMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2ia_Reject_ConfirmMessage%>" />
            <asp:Button ID="Reject_AfterConfirm" runat="server" ClientIDMode="Static" OnClick="Reject_AfterConfirm_Click" Style="display:none;" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
