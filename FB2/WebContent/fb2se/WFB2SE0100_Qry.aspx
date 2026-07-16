<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2se/WFB2SE0100_Qry.aspx.cs" Inherits="WebContent_fb2se_WFB2SE0100_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $(".number").mask('9999999');
            //$(".number").formatNumber({ format: "#,###", locale: "tw" });
            gridviewScroll();
            $.unblockUI();

        }

        function CheckRequiredAdd() {
            var ItemCheckBoxs = $("[type=text]");
            var lb_LEVEL_CD_gv1s = $("[id=lb_LEVEL_CD_gv1]");
            var lb_LEVEL_CD_gv2s = $("[id=lb_LEVEL_CD_gv2]");
            var LEVEL_PAY_LOWs = $("[id=txt_NEW_LEVEL_PAY_LOW]");
            var LEVEL_PAY_AVGs = $("[id=txt_NEW_LEVEL_PAY_AVG]");
            var LEVEL_PAY_UPs = $("[id=txt_NEW_LEVEL_PAY_UP]");
            var ABILITY_ADJs = $("[id=txt_NEW_ABILITY_ADJ]");
            //欄位不可空白.欄位不為數字
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].id.indexOf("txt_NEW_") != -1 && ItemCheckBoxs[i].value == "") {
                    alert($('#hid_wfb2se_required_items').val());
                    return false;
                }
                if (ItemCheckBoxs[i].id.indexOf("txt_NEW_") != -1 && isNaN(ItemCheckBoxs[i].value)) {
                    alert('欄位輸入錯誤');
                    return false;
                }
            }
            for (var i = 0; i < lb_LEVEL_CD_gv2s.length; i++) {
                //中數不允小於等於下限
                if (parseInt(LEVEL_PAY_AVGs[i].value, 10) <= parseInt(LEVEL_PAY_LOWs[i].value, 10)) {
                    alert($('#hid_wfb2se_Add_errorMessage').val());
                    return false;
                }
                //上限不允小於等於中數
                if (parseInt(LEVEL_PAY_UPs[i].value, 10) <= parseInt(LEVEL_PAY_AVGs[i].value, 10)) {
                    alert($('#hid_wfb2se_Add_errorMessage1').val());
                    return false;
                }

                if (i > 0 && lb_LEVEL_CD_gv2s[i].innerText == lb_LEVEL_CD_gv2s[i - 1].innerText) {
                    //下限值不一致
                    if (LEVEL_PAY_LOWs[i].value != LEVEL_PAY_LOWs[i - 1].value) {
                        alert($('#hid_wfb2se_LEVEL_CD').val() + lb_LEVEL_CD_gv2s[i].innerText + $('#hid_wfb2se_Add_LEVEL_PAY_LOWerrorMessage').val());
                        return false;
                    }
                    //中限值不一致
                    if (LEVEL_PAY_AVGs[i].value != LEVEL_PAY_AVGs[i - 1].value) {
                        alert($('#hid_wfb2se_LEVEL_CD').val() + lb_LEVEL_CD_gv2s[i].innerText + $('#hid_wfb2se_Add_LEVEL_PAY_AVGerrorMessage').val());
                        return false;
                    }
                    //上限值不一致
                    if (LEVEL_PAY_UPs[i].value != LEVEL_PAY_UPs[i - 1].value) {
                        alert($('#hid_wfb2se_LEVEL_CD').val() + lb_LEVEL_CD_gv2s[i].innerText + $('#hid_wfb2se_Add_LEVEL_PAY_UPerrorMessage').val());
                        return false;
                    }
                    //資格B/U值不一致
                    if (ABILITY_ADJs[i].value != ABILITY_ADJs[i - 1].value) {
                        alert($('#hid_wfb2se_LEVEL_CD').val() + lb_LEVEL_CD_gv2s[i].innerText + $('#hid_wfb2se_Add_ABILITY_ADJerrorMessage').val());
                        return false;
                    }
                }
            }
            //修改
            if (LookUpCheckboxs() == 1) {
                //欄位不可空白
                for (var i = 0; i < ItemCheckBoxs.length; i++) {
                    if (ItemCheckBoxs[i].id.indexOf("txt_EDIT_") != -1 && ItemCheckBoxs[i].value == "") {
                        alert($('#hid_wfb2se_required_items').val());
                        return false;
                    }
                    if (ItemCheckBoxs[i].id.indexOf("txt_EDIT_") != -1 && isNaN(ItemCheckBoxs[i].value)) {
                        alert('欄位輸入錯誤');
                        return false;
                    }
                }
            }
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
            $('#<%=gv_result2.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hid_wfb2sk_Mod_NotChoiceMessage').val());
                return false;
            }
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
            $("#ddl_LEVEL_CD").val("-1");
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
                                        <asp:Label ID="lb_EFFECT_YM" runat="server" Text="<%$Resources:Resource,wfb2se_EFFECT_YM%>"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_EFFECT_YM" runat="server" MaxLength="6" Width="64px" CssClass="ym MandatoryField date" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2se_required_EFFECT_YM%>"
                                            ControlToValidate="txt_EFFECT_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2se_error_EFFECT_YM%>" ControlToValidate="txt_EFFECT_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2se_LEVEL_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_LEVEL_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
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
                                            <%--<asp:Btn ID="WFB2SE0100LoadAdd" runat="server" Text="<%$Resources:Resource,wfb2se_WFB2SE0100LoadAdd%>" OnClick="WFB2SE0100LoadAdd_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <asp:Btn ID="WFB2SE0100Search" runat="server" Text="<%$Resources:Resource,wfb2se_search%>" OnClick="WFB2SE0100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <%--
                                            <asp:Button ID="WFB2SE0100LoadAdd" runat="server" Text="<%$Resources:Resource,wfb2se_WFB2SE0100LoadAdd%>" OnClick="WFB2SE0100LoadAdd_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            --%>
                                            
                                            <asp:Button ID="WFB2SE0100Search" runat="server" Text="<%$Resources:Resource,wfb2se_search%>" OnClick="WFB2SE0100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2se_clear%>" OnClientClick="return doClear();" />
                                            <aces:Btn ID="WFB2SE0100Import" runat="server" Text="<%$Resources:Resource,Import%>" OnClick="WFB2SE0100Import_Click" />
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
                            <%-- 
                            <asp:Button ID="WFB2SE0101Edit" runat="server" Text="<%$Resources:Resource,wfb2se_WFB2SE0101Edit%>" OnClick="WFB2SE0101Edit_Click" Visible="false" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                            <asp:Button ID="WFB2SE0100Edit" runat="server" Text="<%$Resources:Resource,wfb2sk_edit%>" OnClick="WFB2SE0100Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction(); BlockUI();" />
                            <asp:Button ID="WFB2SE0100OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" Visible="false" OnClick="WFB2SE0100OK_Click" OnClientClick="return CheckRequiredAdd(); BlockUI();" />
                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sk_cancel%>" Visible="false" OnClick="btn_cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                             --%>
                        </div>

                    </td>
                </tr>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                SelectCountMethod="GetCount" TypeName="CFB2SE0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EFFECT_YM" DefaultValue=""
                        Name="effect_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_LEVEL_CD" DefaultValue=""
                        Name="level_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox Width="20px" Style="text-align: center;" ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox Width="100%" Style="text-align: center;" ID="cb_check" runat="server" Checked="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sk_RowNum%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Width="40px" Style="text-align: center;" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Width="40px" Style="text-align: left;" ID="lb_LEVEL_CD_gv1" runat="server" ClientIDMode="Static" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LEVEL_CD_gv1" runat="server" ClientIDMode="Static" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_GRADE_CD%>" SortExpression="GRADE_CD" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Width="40px" Style="text-align: left;" ID="lb_GRADE_CD" runat="server" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_GRADE_CD" runat="server" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_A%>" SortExpression="EXAMINE_A" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_EXAMINE_A" runat="server" Text='<%#Bind("EXAMINE_A","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_EDIT_EXAMINE_A" runat="server" MaxLength="7" ClientIDMode="Static" Text='<%#Bind("EXAMINE_A")%>' CssClass="number MandatoryField"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_B%>" SortExpression="EXAMINE_B" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_EXAMINE_B" runat="server" Text='<%#Bind("EXAMINE_B","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_EDIT_EXAMINE_B" runat="server" MaxLength="7" ClientIDMode="Static" Text='<%#Bind("EXAMINE_B")%>' CssClass="number MandatoryField"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_C%>" SortExpression="EXAMINE_C" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_EXAMINE_C" runat="server" Text='<%#Bind("EXAMINE_C","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_EDIT_EXAMINE_C" runat="server" MaxLength="7" ClientIDMode="Static" Text='<%#Bind("EXAMINE_C")%>' CssClass="number MandatoryField"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_D%>" SortExpression="EXAMINE_D" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_EXAMINE_D" runat="server" Text='<%#Bind("EXAMINE_D","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_EDIT_EXAMINE_D" runat="server" MaxLength="7" ClientIDMode="Static" Text='<%#Bind("EXAMINE_D")%>' CssClass="number MandatoryField"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_E%>" SortExpression="EXAMINE_E" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_EXAMINE_E" runat="server" Text='<%#Bind("EXAMINE_E","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_EDIT_EXAMINE_E" runat="server" MaxLength="7" ClientIDMode="Static" Text='<%#Bind("EXAMINE_E")%>' CssClass="number MandatoryField"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_ABILITY_ADJ%>" SortExpression="ABILITY_ADJ" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_ABILITY_ADJ" runat="server" ClientIDMode="Static" Text='<%#Bind("ABILITY_ADJ","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_EDIT_ABILITY_ADJ" runat="server" Enabled="false" MaxLength="7" ClientIDMode="Static" Text='<%#Bind("ABILITY_ADJ")%>' CssClass="number MandatoryField"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_ADJ%>" SortExpression="LEVEL_ADJ" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_LEVEL_ADJ" runat="server" Text='<%#Bind("LEVEL_ADJ","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_EDIT_LEVEL_ADJ" runat="server" MaxLength="7" ClientIDMode="Static" Text='<%#Bind("LEVEL_ADJ")%>' CssClass="number MandatoryField"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_LOW%>" SortExpression="LEVEL_PAY_LOW" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_LEVEL_PAY_LOW" runat="server" ClientIDMode="Static" Text='<%#Bind("LEVEL_PAY_LOW","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_EDIT_LEVEL_PAY_LOW" runat="server" Enabled="false" MaxLength="7" ClientIDMode="Static" Text='<%#Bind("LEVEL_PAY_LOW")%>' CssClass="number MandatoryField"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_AVG%>" SortExpression="LEVEL_PAY_AVG" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_LEVEL_PAY_AVG" runat="server" ClientIDMode="Static" Text='<%#Bind("LEVEL_PAY_AVG","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_EDIT_LEVEL_PAY_AVG" runat="server" Enabled="false" MaxLength="7" ClientIDMode="Static" Text='<%#Bind("LEVEL_PAY_AVG")%>' CssClass="number MandatoryField"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_UP%>" SortExpression="LEVEL_PAY_UP" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_LEVEL_PAY_UP" runat="server" ClientIDMode="Static" Text='<%#Bind("LEVEL_PAY_UP","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_EDIT_LEVEL_PAY_UP" runat="server" Enabled="false" MaxLength="7" ClientIDMode="Static" Text='<%#Bind("LEVEL_PAY_UP")%>' CssClass="number MandatoryField"></asp:TextBox>
                        </EditItemTemplate>
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
            <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="GetData_Add"
                SelectCountMethod="GetCount_Add" TypeName="CFB2SE0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EFFECT_YM" DefaultValue=""
                        Name="effect_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_LEVEL_CD" DefaultValue=""
                        Name="level_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False"
                OnRowDataBound="gv_result_RowDataBound" Width="1020px">

                <Columns>
                    <%--<asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox Width="20px" Style="text-align: center;" ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sk_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Width="40px" Style="text-align: center;" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_CD%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Width="40px" Style="text-align: left;" ID="lb_LEVEL_CD_gv2" runat="server" ClientIDMode="Static" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_GRADE_CD%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Width="40px" Style="text-align: left;" ID="lb_GRADE_CD" runat="server" ClientIDMode="Static" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_A%>" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:TextBox Width="70px" Style="text-align: right;" ID="txt_NEW_EXAMINE_A" runat="server" MaxLength="7" ClientIDMode="Static" Text="0" CssClass="number MandatoryField"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_B%>" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:TextBox Width="70px" Style="text-align: right;" ID="txt_NEW_EXAMINE_B" runat="server" MaxLength="7" ClientIDMode="Static" Text="0" CssClass="number MandatoryField"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_C%>" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:TextBox Width="70px" Style="text-align: right;" ID="txt_NEW_EXAMINE_C" runat="server" MaxLength="7" ClientIDMode="Static" Text="0" CssClass="number MandatoryField"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_D%>" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:TextBox Width="70px" Style="text-align: right;" ID="txt_NEW_EXAMINE_D" runat="server" MaxLength="7" ClientIDMode="Static" Text="0" CssClass="number MandatoryField"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_E%>" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:TextBox Width="70px" Style="text-align: right;" ID="txt_NEW_EXAMINE_E" runat="server" MaxLength="7" ClientIDMode="Static" Text="0" CssClass="number MandatoryField"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_ABILITY_ADJ%>" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:TextBox Width="70px" Style="text-align: right;" ID="txt_NEW_ABILITY_ADJ" runat="server" MaxLength="7" ClientIDMode="Static" Text="0" CssClass="number MandatoryField"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_ADJ%>" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:TextBox Width="70px" Style="text-align: right;" ID="txt_NEW_LEVEL_ADJ" runat="server" MaxLength="7" ClientIDMode="Static" Text="0" CssClass="number MandatoryField"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_LOW%>" HeaderStyle-Width="50px">
                        <ItemTemplate>
                            <asp:TextBox Width="50px" Style="text-align: right;" ID="txt_NEW_LEVEL_PAY_LOW" runat="server" MaxLength="7" ClientIDMode="Static" Text="0" CssClass="number MandatoryField"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_AVG%>" HeaderStyle-Width="50px">
                        <ItemTemplate>
                            <asp:TextBox Width="50px" Style="text-align: right;" ID="txt_NEW_LEVEL_PAY_AVG" runat="server" MaxLength="7" ClientIDMode="Static" Text="0" CssClass="number MandatoryField"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_UP%>" HeaderStyle-Width="50px">
                        <ItemTemplate>
                            <asp:TextBox Width="50px" Style="text-align: right;" ID="txt_NEW_LEVEL_PAY_UP" runat="server" MaxLength="7" ClientIDMode="Static" Text="0" CssClass="number MandatoryField"></asp:TextBox>
                            <asp:HiddenField ID="HID_ORDER_SEQ" runat="server" ClientIDMode="Static" Value='<%#Bind("ORDER_SEQ")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>


            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2sk_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Mod_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2se_Add_AlreadyExistMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2se_Add_AlreadyExistMessage%>" />
            <asp:HiddenField ID="hid_wfb2se_Add_errorMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2se_Mod_errorMessage%>" />
            <asp:HiddenField ID="hid_wfb2se_Add_errorMessage1" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2se_Mod_errorMessage1%>" />
            <asp:HiddenField ID="hid_wfb2se_Add_LEVEL_PAY_LOWerrorMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2se_Add_LEVEL_PAY_LOWerrorMessage%>" />
            <asp:HiddenField ID="hid_wfb2se_Add_LEVEL_PAY_AVGerrorMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2se_Add_LEVEL_PAY_AVGerrorMessage%>" />
            <asp:HiddenField ID="hid_wfb2se_Add_LEVEL_PAY_UPerrorMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2se_Add_LEVEL_PAY_UPerrorMessage%>" />
            <asp:HiddenField ID="hid_wfb2se_required_items" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2se_required_items%>" />
            <asp:HiddenField ID="hid_wfb2se_LEVEL_CD" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2se_LEVEL_CD%>" />
            <asp:HiddenField ID="hid_wfb2se_Add_ABILITY_ADJerrorMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2se_Add_ABILITY_ADJerrorMessage%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
