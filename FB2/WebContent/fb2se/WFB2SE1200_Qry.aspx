<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2se/WFB2SE1200_Qry.aspx.cs" Inherits="WebContent_fb2se_WFB2SE1200_Qry" Culture="auto" UICulture="auto" %>

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
            $('#txt_DIV_DEPT_FULL_NAME').attr("readonly", true);
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
                }
                else {
                    $('#txt_EMP_NAME').val("");
                }
            });
            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NO").change(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DIV_DEPT_FULL_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DIV_DEPT_FULL_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DIV_DEPT_FULL_NAME').val("");
                }
            });
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
                    freezesize: 5

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }

        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hid_wfb2sk_Del_ConfirmMessage').val());
            else {
                alert($('#hid_wfb2sk_Del_NotChoiceMessage').val());
                return false;
            }
        }
        //檢查勾了幾個checkbox
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
        //清空
        function doClear() {
            $("#txt_EFFECT_YM").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DIV_DEPT_FULL_NAME").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
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
                                <col width="30%" />
                                <col width="5%" />
                                <col width="5%" />
                                <col width="15%" />
                                <col width="30%" />
                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EFFECT_YM" runat="server" Text="<%$Resources:Resource,wfb2se_EFFECT_YM%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_EFFECT_YM" runat="server" MaxLength="6" Width="80px" CssClass="ym MandatoryField date" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2se_required_EFFECT_YM%>"
                                            ControlToValidate="txt_EFFECT_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2se_error_EFFECT_YM%>" ControlToValidate="txt_EFFECT_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2se_lb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="80px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DIV_DEPT_FULL_NAME', 'N', '');" />
                                        <asp:TextBox ID="txt_DIV_DEPT_FULL_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="80px" ClientIDMode="Static" CssClass="empid"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
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
                                            <%--<asp:Btn ID="WFB2SE1200Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2SE1200Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>

                                            <asp:Button ID="WFB2SE1200Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2SE1200Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
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
                            <%--<asp:Btn ID="WFB2SE1200Excel" runat="server" Text="<%$Resources:Resource,wfb2se_excel%>" OnClick="WFB2SE1200Excel_Click" OnClientClick="BlockUI();" Visible="true" />
                            <asp:Btn ID="WFB2SE1200Delete" runat="server" Text="<%$Resources:Resource,wfb2sk_del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SE1200Delete_Click" Visible="false" />--%>

                            <asp:Button ID="WFB2SE1200Excel" runat="server" Text="<%$Resources:Resource,wfb2se_excel%>" OnClick="WFB2SE1200Excel_Click" OnClientClick="BlockUI();" Visible="true" />
                            <asp:Button ID="WFB2SE1200Delete" runat="server" Text="<%$Resources:Resource,wfb2sk_del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SE1200Delete_Click" Visible="false" />
                        </div>

                    </td>
                </tr>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                SelectCountMethod="GetCount" TypeName="CFB2SE1200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EFFECT_YM" DefaultValue=""
                        Name="effect_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO" DefaultValue=""
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1600px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sk_RowNum%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-Width="40px" HeaderText="<%$Resources:Resource,wfb2se_APPROVE_MARK%>" SortExpression="APPROVE_MARK">
                        <ItemTemplate>
                            <asp:CheckBox ID="IS_APPROVE_MARK" runat="server" ClientIDMode="AutoID" Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_EFFECT_YM%>" SortExpression="EFFECT_YM" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EFFECT_YM" runat="server" Text='<%#Bind("EFFECT_YM", "{0:yyyy/MM}")%>' CssClass="ym"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_GRADE_CD%>" SortExpression="GRADE_CD" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_GRADE_CD" runat="server" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_DEPT_NAME_20%>" SortExpression="DEPT_NAME_20" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_DEPT_NAME_20" runat="server" Text='<%#Bind("DEPT_NAME_20")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_DEPT_NAME_30%>" SortExpression="DEPT_NAME_30" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_DEPT_NAME_30" ClientIDMode="Static" runat="server" Text='<%#Bind("DEPT_NAME_30")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_DEPT_NAME_40%>" SortExpression="DEPT_NAME_40" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_DEPT_NAME_40" ClientIDMode="Static" runat="server" Text='<%#Bind("DEPT_NAME_40")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_THIS_YEAR_GRADE%>" SortExpression="THIS_YEAR_GRADE" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_THIS_YEAR_GRADE" ClientIDMode="Static" runat="server" Text='<%#Bind("THIS_YEAR_GRADE")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_OLD%>" SortExpression="LEVEL_PAY_OLD" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_LEVEL_PAY_OLD" runat="server" Text='<%#Bind("LEVEL_PAY_OLD", "{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_ADJ%>" SortExpression="EXAMINE_ADJ" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_EXAMINE_ADJ" runat="server" Text='<%#Bind("EXAMINE_ADJ", "{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_ADJ%>" SortExpression="LEVEL_ADJ" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_LEVEL_ADJ" runat="server" Text='<%#Bind("LEVEL_ADJ", "{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_NEW%>" SortExpression="LEVEL_PAY_NEW" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_LEVEL_PAY_NEW" runat="server" Text='<%#Bind("LEVEL_PAY_NEW", "{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_ABILITY_PAY_OLD%>" SortExpression="ABILITY_PAY_OLD" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_ABILITY_PAY_OLD" runat="server" Text='<%#Bind("ABILITY_PAY_OLD", "{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_ABILITY_ADJ%>" SortExpression="ABILITY_ADJ" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_ABILITY_ADJ" runat="server" Text='<%#Bind("ABILITY_ADJ", "{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_ABILITY_PAY_NEW%>" SortExpression="ABILITY_PAY_NEW" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_ABILITY_PAY_NEW" runat="server" Text='<%#Bind("ABILITY_PAY_NEW", "{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_DIFF%>" SortExpression="LEVEL_PAY_DIFF" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_LEVEL_PAY_DIFF" runat="server" Text='<%#Bind("LEVEL_PAY_DIFF", "{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_NOPAYDIFF_YN%>" SortExpression="NOPAYDIFF_YN" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_NOPAYDIFF_YN" runat="server" ClientIDMode="Static" Enabled="false" />
                            <asp:HiddenField ID="hid_NOPAYDIFF_YN" runat="server" Value='<%#Bind("NOPAYDIFF_YN")%>'></asp:HiddenField>
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

            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:HiddenField ID="HID_TYPE" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <%--<asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />--%>
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2sk_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Del_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sk_Del_ConfirmMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Del_ConfirmMessage%>" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SE1200Excel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
