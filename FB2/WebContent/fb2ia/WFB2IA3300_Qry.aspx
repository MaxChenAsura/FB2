<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA3300_Qry.aspx.cs" Inherits="WebContent_fb2ia_WFB2IA3300_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();

        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $('.number').mask('9999999');
            $('.empid').mask('99999');
            //$(".number").formatNumber({ format: "#,###", locale: "tw" });
            gridviewScroll();
            $.unblockUI();
            $('#txt_EMP_NAME').attr("readonly", true);
            $('#txt_NEW_EMP_NAME').attr("readonly", true);
            $('#txt_NEW_EMP_NAME_FAMILY').attr("readonly", true);
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
            //工號取得姓名的ajax(NEW)
            $("#txt_NEW_EMP_ID").change(function () {
                if ($("#txt_NEW_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_NEW_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_NEW_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_NEW_EMP_NAME').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_NEW_EMP_NAME').val("");
                }
            });
        }

        function CheckAddnum(source, arguments) {
            if (parseInt($("#txt_NEW_TRACE_AMT").val(), 10) == 0)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }
        function CheckEdinum(source, arguments) {
            if (parseInt($("#txt_EDIT_TRACE_AMT").val(), 10) == 0)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }

        function Checkddl_ins(source, arguments) {
            if ($("#ddl_SUB_DESC_INS").val() == -1)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        function Checkddl_identity(source, arguments) {
            if ($("#ddl_SUB_DESC_IDENTITY").val() == -1)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        function Checkddl_trace_type(source, arguments) {
            if ($("#ddl_SUB_DESC_TRACE_TYPE").val() == -1)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        function Checkddl_trace_kind(source, arguments) {
            if ($("#ddl_ADD_TRACE_KIND").val() == -1)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        function CheckAPPROVE_STATUS(source, arguments) {
            if ($("#ddl_APPROVE_STATUS").val() == -1)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        function addcheck() {
            $("#HID_Freeze").val("Y");
            $("#HID_NEW_SALARY_YM").val($("#txt_NEW_SALARY_YM").val());
            $("#HID_EMP_ID").val($("#txt_NEW_EMP_ID").val());
            $("#HID_NEW_EMP_NAME").val($("#txt_NEW_EMP_NAME").val());
            $("#HID_SUB_DESC_INS").val($("#ddl_SUB_DESC_INS").val());
            $("#HID_SUB_DESC_IDENTITY").val($("#ddl_SUB_DESC_IDENTITY").val());
            $("#HID_NEW_LICENSE_ID").val($("#txt_NEW_LICENSE_ID").val());
            $("#HID_NEW_EMP_NAME_FAMILY").val($("#txt_NEW_EMP_NAME_FAMILY").val());
            $("#HID_SUB_DESC_TRACE_TYPE").val($("#ddl_SUB_DESC_TRACE_TYPE").val());
            $("#HID_NEW_TRACE_AMT").val($("#txt_NEW_TRACE_AMT").val());
            $("#HID_NEW_REMARK").val($("#txt_NEW_REMARK").val());
            $("#HID_TRACE_KIND").val($("#ddl_ADD_TRACE_KIND").val());
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
                    freezesize: 8

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }
            else {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F"
                });
            }
        }
        
        function checkDelClick() {
            if (confirm($('#hid_wfb2sk_Del_ConfirmMessage').val()))
            {
                $('#Del_AfterConfirm').click();
            }
            else
            {
                return false;
            }
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() == 0) {
                alert($('#hid_wfb2sk_Del_NotChoiceMessage').val());
                return false;
            }
        }
        function CheckModeifyAction() {
            if (LookUpCheckboxs() != 1) {
                alert($('#hid_wfb2sk_Mod_NotChoiceMessage').val());
                return false;
            }
        }
        //檢查勾了幾個checkbox
        var ItemCheckBoxs = null;
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


        function idCheck(value, type) {
            if (value != "") {
                $("#HID_VALUE").val(value);
                $("#HID_TYPE").val(type);
                __doPostBack('question', 'true');
            }
            else
                $("#txt_NEW_EMP_NAME_FAMILY").val("");
        }
        function add_no_freeze() {
            $("#HID_Freeze").val("N");

        }
        //清空
        function doClear() {
            $("#txt_SALARY_YM").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_APPROVE_STATUS").val("-1");
            return false;
        }
        function OpenIDSearch(obj_id, obj_name) {
            OpenSearch('LICENSE_ID_Search.aspx', obj_id, obj_name, "IDENTITY_KIND=" + $("#ddl_SUB_DESC_IDENTITY").val() + "&EMP_ID=" + $("#txt_NEW_EMP_ID").val() + "&LICENSE_ID=" + $("#txt_NEW_LICENSE_ID").val(),'Y');
            //if (json != undefined) {
            //    $("#" + obj_id).val(json.CD);
            //    $("#txt_NEW_LICENSE_ID").val(json.CD);
            //    if ($("#ddl_SUB_DESC_IDENTITY").val() == "1")
            //        $("#" + obj_name).val("");
            //    else
            //        $("#" + obj_name).val(json.DESC);
            //}
        }
        function OpenRemarkSearch(obj_id, obj_name) {
            //OpenSearch('LICENSE_ID_Search.aspx', obj_id, obj_name, "IDENTITY_KIND=" + $("#ddl_SUB_DESC_IDENTITY").val() + "&EMP_ID=" + $("#txt_NEW_EMP_ID").val() + "&LICENSE_ID=" + $("#txt_NEW_LICENSE_ID").val(), 'Y');
            OpenSearch('CommCode_Search.aspx', obj_id, obj_name, 'MAIN_CD=TRACED_MEMO&SYS_CD=IA');
        }
        function LICENSE_ID_Search(id, name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#" + id).val(obj.CD);
                $("#txt_NEW_LICENSE_ID").val(obj.CD);
                if ($("#ddl_SUB_DESC_IDENTITY").val() == "1")
                    $("#" + name).val("");
                else
                    $("#" + name).val(obj.DESC);

                return obj;
            }
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
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="empid"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPROVE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2ia_APPROVE_STATUS%>"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddl_APPROVE_STATUS" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:CustomValidator ID="CusAS" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="核定狀態不可空白" ClientValidationFunction="CheckAPPROVE_STATUS" ForeColor="Red"
                                            ControlToValidate="ddl_APPROVE_STATUS" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_TRACE_KIND" runat="server" Text="<%$Resources:Resource,wfb2ia_TRACE_KIND%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_TRACE_KIND" runat="server" ClientIDMode="Static" ></asp:DropDownList>                                        
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
                                            <aces:Btn ID="WFB2IA3300Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2IA3300Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />

                                            <%--<asp:Button ID="WFB2IA3300Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2IA3300Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sk_clear%>" OnClientClick="return doClear();" />
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
                            <aces:Btn ID="WFB2IA3300Add" runat="server" Text="<%$Resources:Resource,wfb2sk_add%>" OnClick="WFB2IA3300Add_Click" Visible="true" OnClientClick="add_no_freeze(); BlockUI();" />
                            <aces:Btn ID="WFB2IA3300Delete" runat="server" Text="<%$Resources:Resource,wfb2sk_del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IA3300Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2IA3300Edit" runat="server" Text="<%$Resources:Resource,wfb2sk_edit%>" OnClick="WFB2IA3300Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction(); BlockUI();" />
                            <aces:Btn ID="WFB2IA3300OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" Visible="false" OnClick="WFB2IA3300OK_Click" OnClientClick="addcheck(); CheckValid();" ValidationGroup="GroupB" />

                           <%-- <asp:Button ID="WFB2IA3300Add" runat="server" Text="<%$Resources:Resource,wfb2sk_add%>" OnClick="WFB2IA3300Add_Click" Visible="true" OnClientClick="add_no_freeze(); BlockUI();" />
                            <asp:Button ID="WFB2IA3300Delete" runat="server" Text="<%$Resources:Resource,wfb2sk_del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IA3300Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2IA3300Edit" runat="server" Text="<%$Resources:Resource,wfb2sk_edit%>" OnClick="WFB2IA3300Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction(); BlockUI();" />
                            <asp:Button ID="WFB2IA3300OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" Visible="false" OnClick="WFB2IA3300OK_Click" OnClientClick="addcheck(); CheckValid();" ValidationGroup="GroupB" />--%>
                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sk_cancel%>" Visible="false" OnClick="btn_cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                        </div>

                    </td>
                </tr>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                SelectCountMethod="GetCount" TypeName="CFB2IA3300DAO" EnablePaging="True"
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
                    <asp:ControlParameter ControlID="ddl_TRACE_KIND" DefaultValue=""
                        Name="trace_kind" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1200px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sk_RowNum%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </FooterTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_SALARY_YM%>" SortExpression="SALARY_YM" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_SALARY_YM" runat="server" Text='<%#Bind("SALARY_YM", "{0:yyyy/MM}")%>' CssClass="ym"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lb_SALARY_YM" runat="server" Text='<%#Bind("SALARY_YM", "{0:yyyy/MM}")%>' CssClass="ym"></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox Style="text-align: left" ID="txt_NEW_SALARY_YM" runat="server" MaxLength="7" Width="90%" CssClass="MandatoryField ym date" ClientIDMode="Static"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_error_SALARY_YM%>" ControlToValidate="txt_NEW_SALARY_YM" ForeColor="Red" ValidationGroup="GroupB"
                                    ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server" ErrorMessage="追朔年月不可空白"
                                    ControlToValidate="txt_NEW_SALARY_YM" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox ID="txt_NEW_EMP_ID" runat="server" MaxLength="5" Width="45px" ClientIDMode="Static" CssClass="MandatoryField empid"></asp:TextBox>
                                <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_NEW_EMP_ID', 'txt_NEW_EMP_NAME', 'N', ''); " />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server" ErrorMessage="工號不可空白"
                                    ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox ID="txt_NEW_EMP_NAME" runat="server" Width="90%" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_SUB_DESC_INS%>" SortExpression="INS_TYPE" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_SUB_DESC_INS" runat="server" Text='<%#Bind("INS_TYPE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lb_SUB_DESC_INS" runat="server" Text='<%#Bind("INS_TYPE")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:DropDownList ID="ddl_SUB_DESC_INS" runat="server" AutoPostBack="true" ClientIDMode="Static" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_SUB_DESC_INS_SelectedIndexChanged"></asp:DropDownList>
                                <asp:CustomValidator ID="CustomValidator31" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="保險類別不可空白" ClientValidationFunction="Checkddl_ins" ForeColor="Red"
                                    ControlToValidate="ddl_SUB_DESC_INS" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_SUB_DESC_IDENTITY%>" SortExpression="IDENTITY_KIND" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_SUB_DESC_IDENTITY" runat="server" Text='<%#Bind("IDENTITY_KIND")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lb_SUB_DESC_IDENTITY" runat="server" Text='<%#Bind("IDENTITY_KIND")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:DropDownList ID="ddl_SUB_DESC_IDENTITY" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:CustomValidator ID="CustomValidator32" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="身份別不可空白" ClientValidationFunction="Checkddl_identity" ForeColor="Red"
                                    ControlToValidate="ddl_SUB_DESC_IDENTITY" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>" SortExpression="LICENSE_ID" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lb_LICENSE_ID" runat="server" ClientIDMode="Static" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox Style="text-align: left" ID="txt_NEW_LICENSE_ID" Width="90%" runat="server" MaxLength="20" ClientIDMode="Static" CssClass="MandatoryField" onblur="idCheck(this.value,'id');"></asp:TextBox>
                                <input id="bt_NEW_LICENSE_ID" type="button" value="..." onclick="OpenIDSearch('txt_NEW_LICENSE_ID', 'txt_NEW_EMP_NAME_FAMILY'); " />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_LICENSE_ID%>"
                                    ControlToValidate="txt_NEW_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_NAME_FAMILY%>" SortExpression="FAMILY_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_NAME_FAMILY" ClientIDMode="Static" runat="server" Text='<%#Bind("FAMILY_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lb_EMP_NAME_FAMILY" runat="server" ClientIDMode="Static" Text='<%#Bind("FAMILY_NAME")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox ID="txt_NEW_EMP_NAME_FAMILY" runat="server" BorderWidth="0" Width="90%" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_SUB_DESC_TRACE_TYPE%>" SortExpression="TRACE_TYPE" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_SUB_DESC_TRACE_TYPE" ClientIDMode="Static" runat="server" Text='<%#Bind("TRACE_TYPE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_SUB_DESC_TRACE_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:CustomValidator ID="CustomValidator39" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="追朔類別不可空白" ClientValidationFunction="Checkddl_trace_type" ForeColor="Red"
                                    ControlToValidate="ddl_SUB_DESC_TRACE_TYPE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:DropDownList ID="ddl_SUB_DESC_TRACE_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:CustomValidator ID="CustomValidator33" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="追朔類別不可空白" ClientValidationFunction="Checkddl_trace_type" ForeColor="Red"
                                    ControlToValidate="ddl_SUB_DESC_TRACE_TYPE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_TRACE_KIND%>" SortExpression="TRACE_KIND" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_TRACE_KIND" ClientIDMode="Static" runat="server" Text='<%#Bind("TRACE_KIND")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_ADD_TRACE_KIND" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:CustomValidator ID="Validator38" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="追溯區分不可空白" ClientValidationFunction="Checkddl_trace_kind" ForeColor="Red"
                                    ControlToValidate="ddl_ADD_TRACE_KIND" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:DropDownList ID="ddl_ADD_TRACE_KIND" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:CustomValidator ID="Validator33" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="追溯區分不可空白" ClientValidationFunction="Checkddl_trace_kind" ForeColor="Red"
                                    ControlToValidate="ddl_ADD_TRACE_KIND" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_TRACE_AMT%>" SortExpression="TRACE_AMT" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_TRACE_AMT" ClientIDMode="Static" runat="server" Text='<%#Bind("TRACE_AMT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_EDIT_TRACE_AMT" runat="server" MaxLength="7" Width="90%" ClientIDMode="Static" Text='<%#Bind("TRACE_AMT")%>' CssClass=" MandatoryField number"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator34" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="追朔保費不可為零" ClientValidationFunction="CheckEdinum" ForeColor="Red"
                                    ControlToValidate="txt_EDIT_TRACE_AMT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_TRACE_AMT%>"
                                    ControlToValidate="txt_EDIT_TRACE_AMT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="checknum" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_error_TRACE_AMT%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_EDIT_TRACE_AMT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:TextBox ID="txt_NEW_TRACE_AMT" runat="server" Width="90%" MaxLength="7" ClientIDMode="Static" CssClass=" MandatoryField number"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="追朔保費不可為零" ClientValidationFunction="CheckAddnum" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TRACE_AMT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_TRACE_AMT%>"
                                    ControlToValidate="txt_NEW_TRACE_AMT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="checknum2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_error_TRACE_AMT%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TRACE_AMT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sk_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox Style="text-align: left" ID="txt_EDIT_REMARK" runat="server" MaxLength="300" Width="145px" Text='<%#Bind("REMARK")%>'  ClientIDMode="Static"></asp:TextBox>
                                 <input id="bt_NEW_REMARK" type="button" value="..." onclick="OpenRemarkSearch('txt_EDIT_REMARK', 'txt_EDIT_REMARK');" />
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox Style="text-align: left" ID="txt_NEW_REMARK" Width="90%" runat="server" MaxLength="300" ClientIDMode="Static"></asp:TextBox>
                                 <input id="bt_NEW_REMARK" type="button" value="..." onclick="OpenRemarkSearch('txt_NEW_REMARK', 'txt_NEW_REMARK');" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_APPROVE_STATUS%>" SortExpression="APPROVE_STATUS" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_APPROVE_STATUS" ClientIDMode="Static" runat="server" Text='<%#Bind("APPROVE_STATUS")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label Width="100%" Style="text-align: left;" ID="lb_APPROVE_STATUS" runat="server" Text='<%#Bind("APPROVE_STATUS")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_APP_REMARK%>" SortExpression="APP_REMARK" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_APP_REMARK" runat="server" Text='<%#Bind("APP_REMARK")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label Width="100%" Style="text-align: left;" ID="lb_APP_REMARK" runat="server" Text='<%#Bind("APP_REMARK")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_IS_YN%>" SortExpression="IS_YN" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_IS_YN" runat="server" Text='<%#Bind("IS_YN")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label Width="100%" Style="text-align: left;" ID="lb_IS_YN" runat="server" Text='<%#Bind("IS_YN")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <div style="overflow: auto; width: 1020px">
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
                                    <asp:Label ID="Label3" runat="server" Width="60px" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label17" runat="server" Width="60px" Text="<%$Resources:Resource,wfb2ia_EMP_NAME%>"></asp:Label>
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
                                    <asp:Label ID="Label7" runat="server" Width="100px" Text="<%$Resources:Resource,wfb2ia_EMP_NAME_FAMILY%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Width="100px" Text="<%$Resources:Resource,wfb2ia_SUB_DESC_TRACE_TYPE%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label14" runat="server" Width="100px" Text="<%$Resources:Resource,wfb2ia_TRACE_KIND%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Width="100px" Text="<%$Resources:Resource,wfb2ia_TRACE_AMT%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Width="60px" Text="<%$Resources:Resource,wfb2sk_REMARK%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Width="100px" Text="<%$Resources:Resource,wfb2ia_APPROVE_STATUS%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" Width="120px" Text="<%$Resources:Resource,wfb2ia_APP_REMARK%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label13" runat="server" Width="100px" Text="<%$Resources:Resource,wfb2ia_IS_YN%>"></asp:Label>
                                </td>
                            </tr>
                            <tr class="normal">
                                <td>
                                    <asp:CheckBox ID="cb_check" runat="server" />
                                </td>
                                <td>
                                    <asp:Label ID="lb_EmptyRowNumber" runat="server" Text="1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox Style="text-align: left" ID="txt_NEW_SALARY_YM" runat="server" Width="80px" MaxLength="6" CssClass="MandatoryField ym date" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ErrorMessage="<%$Resources:Resource,wfb2ia_error_SALARY_YM%>" ControlToValidate="txt_NEW_SALARY_YM" ForeColor="Red" ValidationGroup="GroupB"
                                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="追朔年月不可空白"
                                        ControlToValidate="txt_NEW_SALARY_YM" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField empid" ></asp:TextBox>
                                    <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_NEW_EMP_ID', 'txt_NEW_EMP_NAME', 'N','');" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ErrorMessage="工號不可空白"
                                        ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_EMP_NAME" runat="server" Width="60px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_SUB_DESC_INS" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField" AutoPostBack="true" OnSelectedIndexChanged="ddl_SUB_DESC_INS_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="保險類別不可空白" ClientValidationFunction="Checkddl_ins" ForeColor="Red"
                                        ControlToValidate="ddl_SUB_DESC_INS" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_SUB_DESC_IDENTITY" runat="server" Width="80px" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="身份別不可空白" ClientValidationFunction="Checkddl_identity" ForeColor="Red"
                                        ControlToValidate="ddl_SUB_DESC_IDENTITY" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                </td>
                                <td>
                                    <asp:TextBox Style="text-align: left" ID="txt_NEW_LICENSE_ID" Width="100px" runat="server" MaxLength="20" ClientIDMode="Static" CssClass="MandatoryField" onblur="idCheck(this.value,'id');"></asp:TextBox>
                                    <input id="bt_NEW_LICENSE_ID" type="button" value="..." onclick="OpenIDSearch('txt_NEW_LICENSE_ID', 'txt_NEW_EMP_NAME_FAMILY');" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_LICENSE_ID%>"
                                        ControlToValidate="txt_NEW_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_EMP_NAME_FAMILY" runat="server" Width="100px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_SUB_DESC_TRACE_TYPE" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                    <asp:CustomValidator ID="CustomValidator35" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="追朔類別不可空白" ClientValidationFunction="Checkddl_trace_type" ForeColor="Red"
                                        ControlToValidate="ddl_SUB_DESC_TRACE_TYPE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_ADD_TRACE_KIND" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                    <asp:CustomValidator ID="Validator33" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="追溯區分不可空白" ClientValidationFunction="Checkddl_trace_kind" ForeColor="Red"
                                        ControlToValidate="ddl_ADD_TRACE_KIND" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_TRACE_AMT" runat="server" MaxLength="7" Width="100px" ClientIDMode="Static" CssClass=" MandatoryField number"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="追朔保費不可為零" ClientValidationFunction="CheckAddnum" ForeColor="Red"
                                        ControlToValidate="txt_NEW_TRACE_AMT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_TRACE_AMT%>"
                                        ControlToValidate="txt_NEW_TRACE_AMT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="checknum3" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_error_TRACE_AMT%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TRACE_AMT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                </td>
                                <td>
                                    <asp:TextBox Style="text-align: left" ID="txt_NEW_REMARK" Width="60px" runat="server" MaxLength="300" ClientIDMode="Static"></asp:TextBox>
                                    <input id="bt_NEW_REMARK" type="button" value="..." onclick="OpenRemarkSearch('txt_NEW_REMARK', 'txt_NEW_REMARK');" />
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
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
            <asp:HiddenField ID="HID_VALUE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_SALARY_YM" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_EMP_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_SUB_DESC_INS" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_SUB_DESC_IDENTITY" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_LICENSE_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_TRACE_KIND" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_EMP_NAME_FAMILY" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_SUB_DESC_TRACE_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_TRACE_AMT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_REMARK" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <%--<asp:HiddenField ID="HID_APPROVE_STATUS" runat="server" ClientIDMode="Static" />--%>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2sk_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2sk_Del_ConfirmMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2sk_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" />
            <asp:Button ID="Del_AfterConfirm" runat="server" ClientIDMode="Static" OnClick="Del_AfterConfirm_Click" Style="display:none;" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
