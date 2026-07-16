<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sf/WFB2SF1200_Qry.aspx.cs" Inherits="WebContent_fb2sf_WFB2SF1200_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ym').mask('9999/99/99');
            $('.number').mask('999');
            $('.number7').mask('9999999');
            gridviewScroll();
            $.unblockUI();
            $('#txt_EMP_NAME').attr("readonly", true);
            $('#txt_OPELATION_NAME').attr("readonly", true);
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
            //工號取得姓名的ajax
            $("#txt_OPELATION_BY").change(function () {
                if ($("#txt_OPELATION_BY").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_OPELATION_BY').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_OPELATION_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_OPELATION_NAME').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                }
                else {
                    $('#txt_OPELATION_NAME').val("");
                }
            });
        }

        function ShowRecord(obj) {
            if (obj == "ddlPerPageRow")
                $("#HID_PageRow").val($("#ddlPerPageRow").val());
            if (obj == "ddlPerPageRow2")
                $("#HID_PageRow2").val($("#ddlPerPageRow2").val());
        }

        //凍結視窗
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F",
                headerrowcount: 1,
                freezesize: 1

            });
            $('#<%=gv_result2.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F",
                headerrowcount: 1,
                freezesize: 1

            });
            CheckBoxCheckAllByfreeze("cb_all_gv1_freezeheader", "cb_check_gv1");
            CheckBoxCheckAllByfreeze("cb_all_gv2_freezeheader", "cb_check_gv2");
        }
        //發薪類別不可空白
        function CheckSALARY_TYPE(source, arguments) {
            if ($("#ddl_SALARY_TYPE").val() == "-1")
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        //法扣金額不允小於等於零
        function Check_NEW_AMOUNT(source, arguments) {
            if (parseInt($("#txt_NEW_AMOUNT").val(), 10) <= 0)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        //法扣金額不允負數
        function Check_EDIT_AMOUNT(source, arguments) {
            if (parseInt($("#txt_EDIT_AMOUNT").val(), 10) < 0)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }

        //發文字號不可空白
        function CheckDOC_NO(source, arguments) {
            if ($("#txt_NEW_DOC_NO").val() == "")
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        //gv_result2檢查刪除勾選比數
        function CheckDelAction_Dtl() {
            if (LookUpCheckboxs('gv_result2') <= 0) {
                alert($('#hid_wfb2sf_Del_NotChoiceMessage').val());
                return false;
            }
            else {
                if (confirm('確定要刪除?'))
                    return true;
                else
                    return false;
            }
        }

        //gv_result2檢查修改勾選比數
        function CheckModeifyAction_Dtl() {
            if (LookUpCheckboxs('gv_result2') != 1) {
                alert($('#hid_wfb2sf_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        var ItemCheckBoxs = null;
        var ItemCheckBoxs_bygrid = [];
        var index = -1;
        //檢查勾了幾個checkbox
        function LookUpCheckboxs(type) {
            ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            if (type == "gv_result") {
                for (var i = 0; i < ItemCheckBoxs.length; i++) {
                    if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_check_gv1") != -1 && ItemCheckBoxs[i].id.indexOf("_freezeitem") != -1 && ItemCheckBoxs[i].id.indexOf("_freezeheader") == -1) {
                        HaveCheck++;
                    }
                }
                return HaveCheck;
            }
            if (type == "gv_result2") {
                for (var i = 0; i < ItemCheckBoxs.length; i++) {
                    if (ItemCheckBoxs[i].id.indexOf("cb_check_gv2") != -1) {
                        index++;
                    }
                    if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_check_gv2") != -1 && ItemCheckBoxs[i].id.indexOf("_freezeitem") != -1 && ItemCheckBoxs[i].id.indexOf("_freezeheader") == -1) {
                        HaveCheck++;
                    }
                }
                index = -1;
                return HaveCheck;
            }

        }

        //gv_result2新增塞值
        function addcheck2() {
            $("#HID_NEW_DOC_NO2").val($("#txt_NEW_DOC_NO").val());
            $("#HID_NEW_AMOUNT").val($("#txt_NEW_AMOUNT").val());
            $("#HID_SEQ").val($("#txt_NEW_SEQ").val());
        }
        //清空
        function doClear() {
            $("#txt_SALARY_DT").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_OPELATION_BY").val("");
            $("#txt_OPELATION_NAME").val("");
            $("#ddl_SALARY_TYPE").val("-1");
            $("#ddl_SURE_YN").val("-1");
            return false;
        }
        //ID開窗
        function OpenDOC_NO_Search(obj_doc_no, obj_seq, obj_pay_target, obj_creditor) {
            OpenSearch('DOC_NO_Search.aspx', obj_doc_no, obj_seq, "EMP_ID=" + $("#HID_EMP_ID").val() + "&DOC_NO=" + $("#txt_DOC_NO").val() + "&CREDITOR=" + $("#txt_NEW_CREDITOR").val(),'Y');
            //if (json != undefined) {
            //    $("#" + obj_doc_no).val(json.CD);
            //    $("#" + obj_seq).val(json.DESC);
            //    $("#" + obj_pay_target).val(json.Val1);
            //    $("#" + obj_creditor).val(json.Val2);
            //}
        }
        function DOC_NO_Search(obj_cd, obj_desc, value) {
            var returnValue = value;
            if (returnValue == undefined) {
                returnValue = window.returnValue;
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#" + obj_cd).val(obj.CD);
                $("#" + obj_desc).val(obj.DESC);
                $("#txt_NEW_PAY_TARGET").val(json.Val1);
                $("#txt_NEW_CREDITOR").val(json.Val2);
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
                                <col width="5%" />
                                <col width="15%" />
                                <col width="5%" />
                                <col width="15%" />
                                <col width="20%" />
                                <col width="40%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sf_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_SALARY_DT" runat="server" MaxLength="8" Width="80px" CssClass="MandatoryField ym date" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_SALARY_DT%>"
                                            ControlToValidate="txt_SALARY_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None" ClientIDMode="Static"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_error_SALARY_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sf_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_required_ddl_SALARY_TYPE%>" ClientValidationFunction="CheckSALARY_TYPE" ForeColor="Red"
                                            ControlToValidate="ddl_SALARY_TYPE" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OPELATION_BY" runat="server" Text="<%$Resources:Resource,wfb2sf_lb_OPELATION_BY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OPELATION_BY" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_OPELATION_BY_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_OPELATION_BY', 'txt_OPELATION_NAME', 'N', '');" />
                                        <asp:TextBox ID="txt_OPELATION_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SURE_YN" runat="server" Text="確認否"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SURE_YN" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SF1200Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2SF1200Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <aces:Btn ID="WFB2SF1200Execute" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF1200Execute%>" OnClick="WFB2SF1200Execute_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />

                                            <%--<asp:Button ID="WFB2SF1200Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2SF1200Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sk_clear%>" OnClientClick="return doClear();" />
                                            <%--<asp:Button ID="WFB2SF1200Execute" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF1200Execute%>" OnClick="WFB2SF1200Execute_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
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
                    <td>
                        <!--grid1-->
                        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                            SelectCountMethod="GetCount" TypeName="CFB2SF1200DAO" EnablePaging="True"
                            SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                            StartRowIndexParameterName="startRowIndex"
                            OnSelected="ods1_Selected">
                            <SelectParameters>
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:ControlParameter ControlID="txt_SALARY_DT" DefaultValue=""
                                    Name="salary_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                                <asp:ControlParameter ControlID="ddl_SALARY_TYPE" DefaultValue=""
                                    Name="salary_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                                <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                                    Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                                <asp:ControlParameter ControlID="txt_OPELATION_BY" DefaultValue=""
                                    Name="opelation_by" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                                <asp:ControlParameter ControlID="ddl_SURE_YN" DefaultValue=""
                                    Name="sure_yn" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                            </SelectParameters>
                        </asp:ObjectDataSource>

                        <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                            OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                            OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="cb_all_gv1" Width="20px" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cb_check_gv1" Width="20px" Style="text-align: center;" runat="server" ClientIDMode="AutoID" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_RowNumber" Width="40px" Style="text-align: center;" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label Width="50px" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                                        <asp:HiddenField ID="HID_SALARY_DT" runat="server" ClientIDMode="Static" Value='<%#Bind("SALARY_DT")%>' />
                                        <asp:HiddenField ID="HID_SALARY_TYPE" runat="server" ClientIDMode="Static" Value='<%#Bind("SALARY_TYPE")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label Width="50px" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_PAY_KIND%>" SortExpression="PAY_KIND" HeaderStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label Width="80px" Style="text-align: left;" ID="lb_PAY_KIND" runat="server" Text='<%#Bind("PAY_KIND")%>'></asp:Label>
                                        <asp:HiddenField ID="HID_PAY_KIND_ID" runat="server" ClientIDMode="Static" Value='<%#Bind("PAY_KIND_ID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_ORG_AMT%>" SortExpression="ORG_AMT" HeaderStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label Width="90px" Style="text-align: right;" ID="lb_ORG_AMT" runat="server" Text='<%#Bind("ORG_AMT","{0:N0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_DEBIT_AMT%>" SortExpression="DEBIT_AMT" HeaderStyle-Width="110px">
                                    <ItemTemplate>
                                        <asp:Label Width="110px" Style="text-align: right;" ID="lb_DEBIT_AMT" runat="server" Text='<%#Bind("DEBIT_AMT","{0:N0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_SURE_YN%>" SortExpression="SURE_YN" HeaderStyle-Width="110px">
                                    <ItemTemplate>
                                        <asp:Label Width="110px" Style="text-align: left;" ID="lb_SURE_YN" runat="server" Text='<%#Bind("SURE_YN")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_OPELATION_BY%>" SortExpression="OPELATION_BY" HeaderStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:Label Width="70px" Style="text-align: left;" ID="lb_OPELATION_BY" runat="server" Text='<%#Bind("OPELATION_BY")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_FUNCTION%>" HeaderStyle-Width="200px">
                                    <ItemTemplate>
                                        <aces:Btn ID="WFB2SF1200Dtl" runat="server"
                                            CommandName="ToDtl"
                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text="<%$Resources:Resource,wfb2sf_WFB2SF0100Dtl%>" />
                                        <aces:Btn ID="WFB2SF1200DataCheck" runat="server"
                                            CommandName="ToDataCheck"
                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text="<%$Resources:Resource,wfb2sf_WFB2SF0100DataCheck%>" />

                                        <%--<asp:Button ID="WFB2SF1200Dtl" runat="server"
                                            CommandName="ToDtl"
                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text="<%$Resources:Resource,wfb2sf_WFB2SF0100Dtl%>" />
                                        <asp:Button ID="WFB2SF1200DataCheck" runat="server"
                                            CommandName="ToDataCheck"
                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text="<%$Resources:Resource,wfb2sf_WFB2SF0100DataCheck%>" />--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle CssClass="GridviewScrollPager" />
                        </asp:GridView>
                        <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                            <tr height="100%" valign="top">
                                <td class="GridviewScrollPager TD">
                                    <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('ddlPerPageRow')" ClientIDMode="Static" AutoPostBack="true">
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
                    </td>
                </tr>
                <tr>
                    <td>
                        <!--grid2-->
                        <table>
                            <tr>

                                <td align="right" class="Body_label">

                                    <div id="Div1">
                                        <aces:Btn ID="WFB2SF1200Add" runat="server" Text="<%$Resources:Resource,wfb2sk_add%>" OnClick="WFB2SF1200Add_Click" Visible="false" OnClientClick="BlockUI();" />
                                        <aces:Btn ID="WFB2SF1200Delete" runat="server" Text="<%$Resources:Resource,wfb2sk_del%>" OnClientClick="return CheckDelAction_Dtl();" OnClick="WFB2SF1200Delete_Click" Visible="false" />
                                        <aces:Btn ID="WFB2SF1200Edit" runat="server" Text="<%$Resources:Resource,wfb2sk_edit%>" OnClick="WFB2SF1200Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction_Dtl(); BlockUI();" />
                                        <aces:Btn ID="WFB2SF1200OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" Visible="false" OnClick="WFB2SF1200OK_Click" OnClientClick="addcheck2(); CheckValid(); " ValidationGroup="GroupC" />

                                        <%--<asp:Button ID="WFB2SF1200Add" runat="server" Text="<%$Resources:Resource,wfb2sk_add%>" OnClick="WFB2SF1200Add_Click" Visible="false" OnClientClick="BlockUI();" />
                                        <asp:Button ID="WFB2SF1200Delete" runat="server" Text="<%$Resources:Resource,wfb2sk_del%>" OnClientClick="return CheckDelAction_Dtl();" OnClick="WFB2SF1200Delete_Click" Visible="false" />
                                        <asp:Button ID="WFB2SF1200Edit" runat="server" Text="<%$Resources:Resource,wfb2sk_edit%>" OnClick="WFB2SF1200Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction_Dtl(); BlockUI();" />
                                        <asp:Button ID="WFB2SF1200OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" Visible="false" OnClick="WFB2SF1200OK_Click" OnClientClick="addcheck2(); CheckValid(); " ValidationGroup="GroupC" />--%>
                                        <asp:Button ID="btn_cancel2" runat="server" Text="<%$Resources:Resource,wfb2sk_cancel%>" Visible="false" OnClick="btn_cancel2_Click" OnClientClick="return confirm('是否確定取消?');" />
                                    </div>

                                </td>
                            </tr>
                        </table>

                        <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="GetData2"
                            SelectCountMethod="GetCount2" TypeName="CFB2SF1200DAO" EnablePaging="True"
                            SortParameterName="sortExpression" OnSelecting="obs1_Selecting2"
                            StartRowIndexParameterName="startRowIndex"
                            OnSelected="ods1_Selected2">
                            <SelectParameters>
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:ControlParameter ControlID="HID_EMP_ID" DefaultValue=""
                                    Name="EMP_ID" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                                <asp:ControlParameter ControlID="HID_PAY_KIND" DefaultValue=""
                                    Name="PAY_KIND" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                                <asp:ControlParameter ControlID="HID_GV1_SALARY_DT" DefaultValue=""
                                    Name="SALARY_DT" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                                <asp:ControlParameter ControlID="HID_GV1_SALARY_TYPE" DefaultValue=""
                                    Name="SALARY_TYPE" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                            </SelectParameters>
                        </asp:ObjectDataSource>

                        <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="true"
                            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result2_Sorting"
                            OnRowDataBound="gv_result2_RowDataBound" OnRowCreated="gv_result2_RowCreated" Width="1020px"
                            OnPageIndexChanging="gv_result2_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result2_DataBound">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="cb_all_gv2" Width="20px" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cb_check_gv2" Width="100%" Style="text-align: center;" runat="server" ClientIDMode="AutoID" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="cb_check" Width="100%" Style="text-align: center;" runat="server" Checked="true" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_RowNumber" Width="40px" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_RowNumber" Width="100%" Style="text-align: center;" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lb_NewRowNumber" Width="100%" Style="text-align: center;" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_DOC_NO%>" SortExpression="DOC_NO" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:Label Width="120px" Style="text-align: left;" ID="lb_DOC_NO" runat="server" Text='<%#Bind("DOC_NO")%>'></asp:Label>
                                        <asp:HiddenField ID="HID_qdatakey3" runat="server" ClientIDMode="Static" Value='<%#Bind("qdatakey3")%>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_DOC_NO" runat="server" Text='<%#Bind("DOC_NO")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: left;" ID="txt_NEW_DOC_NO" runat="server" CssClass="MandatoryField" MaxLength="30" Enabled="false" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_DOC_NO_SEARCH" type="button" value="..." onclick="OpenDOC_NO_Search('txt_NEW_DOC_NO', 'txt_NEW_SEQ', 'txt_NEW_PAY_TARGET', 'txt_NEW_CREDITOR');" />
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_required_NEW_DOC_NO%>" ClientValidationFunction="CheckDOC_NO" ForeColor="Red"
                                            ControlToValidate="txt_NEW_DOC_NO" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_SEQ%>" SortExpression="SEQ" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label Width="80px" Style="text-align: center;" ID="lb_SEQ" runat="server" Text='<%#Bind("SEQ")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: center;" ID="lb_SEQ" runat="server" Text='<%#Bind("SEQ")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: center;" ID="txt_NEW_SEQ" runat="server" MaxLength="3" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_PAY_TARGET%>" SortExpression="PAY_TARGET" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label Width="150px" Style="text-align: left;" ID="lb_PAY_TARGET" runat="server" Text='<%#Bind("PAY_TARGET")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_PAY_TARGET" runat="server" Text='<%#Bind("PAY_TARGET")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: left;" ID="txt_NEW_PAY_TARGET" runat="server" MaxLength="1" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_CREDITOR%>" SortExpression="CREDITOR" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label Width="150px" Style="text-align: left;" ID="lb_CREDITOR" runat="server" Text='<%#Bind("CREDITOR")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_CREDITOR" runat="server" Text='<%#Bind("CREDITOR")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: left;" ID="txt_NEW_CREDITOR" runat="server" MaxLength="60" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_AMOUNT3%>" SortExpression="AMOUNT" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:Label Width="120px" Style="text-align: right;" ID="lb_AMOUNT" runat="server" Text='<%#Bind("AMOUNT","{0:N0}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_EDIT_AMOUNT" runat="server" MaxLength="7" CssClass="number7 MandatoryField" ClientIDMode="Static" Text='<%#Bind("AMOUNT")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_AMOUNT%>"
                                            ControlToValidate="txt_EDIT_AMOUNT" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_Greaterthen0_txt_EDIT_AMOUNT%>" ClientValidationFunction="Check_EDIT_AMOUNT" ForeColor="Red"
                                            ControlToValidate="txt_EDIT_AMOUNT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="checknum" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="法扣金額格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_EDIT_AMOUNT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_NEW_AMOUNT" runat="server" MaxLength="7" CssClass="number7 MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_AMOUNT%>"
                                            ControlToValidate="txt_NEW_AMOUNT" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_Greaterthen0_txt_NEW_AMOUNT%>" ClientValidationFunction="Check_NEW_AMOUNT" ForeColor="Red"
                                            ControlToValidate="txt_NEW_AMOUNT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="checknum2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="法扣金額格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_NEW_AMOUNT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_DEPT_ACCT_ID%>" SortExpression="DEPT_ACCT_ID" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label Width="150px" Style="text-align: left;" ID="lb_DEPT_ACCT_ID" ClientIDMode="Static" runat="server" Text='<%#Bind("DEPT_ACCT_ID")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_DEPT_ACCT_ID" ClientIDMode="Static" runat="server" Text='<%#Bind("DEPT_ACCT_ID")%>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_ACCT_ID%>" SortExpression="ACCT_ID" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label Width="150px" Style="text-align: left;" ID="lb_ACCT_ID" runat="server" Text='<%#Bind("ACCT_ID")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_ACCT_ID" ClientIDMode="Static" runat="server" Text='<%#Bind("ACCT_ID")%>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <EmptyDataTemplate>
                                <table class="grid-view" width="1020px">
                                    <tr class="header">
                                        <td>
                                            <asp:CheckBox ID="cb_checkall" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sf_DOC_NO%>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sf_SEQ%>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sf_PAY_TARGET%>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2sf_CREDITOR%>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2sf_AMOUNT3%>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sf_DEPT_ACCT_ID%>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sf_ACCT_ID%>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="normal">
                                        <td></td>
                                        <td>
                                            <asp:Label ID="Label22" runat="server" Text="1"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox Width="120px" Style="text-align: left;" ID="txt_NEW_DOC_NO" runat="server" CssClass="MandatoryField" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                            <input id="bt_DOC_NO_SEARCH" type="button" value="..." onclick="OpenDOC_NO_Search('txt_NEW_DOC_NO', 'txt_NEW_SEQ', 'txt_NEW_PAY_TARGET', 'txt_NEW_CREDITOR');" />
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2sf_required_NEW_DOC_NO%>" ClientValidationFunction="CheckDOC_NO" ForeColor="Red"
                                                ControlToValidate="txt_NEW_DOC_NO" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox Width="120px" Style="text-align: right;" ID="txt_NEW_SEQ" runat="server" MaxLength="3" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox Width="150px" Style="text-align: left;" ID="txt_NEW_PAY_TARGET" runat="server" MaxLength="1" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox Width="150px" Style="text-align: left;" ID="txt_NEW_CREDITOR" runat="server" MaxLength="60" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox Width="80px" Style="text-align: right;" ID="txt_NEW_AMOUNT" runat="server" MaxLength="7" CssClass="number7 MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_AMOUNT%>"
                                                ControlToValidate="txt_NEW_AMOUNT" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2sf_Greaterthen0_txt_NEW_AMOUNT%>" ClientValidationFunction="Check_NEW_AMOUNT" ForeColor="Red"
                                                ControlToValidate="txt_NEW_AMOUNT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                            <asp:CustomValidator ID="checknum3" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="法扣金額格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                                ControlToValidate="txt_NEW_AMOUNT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle CssClass="GridviewScrollPager" />
                        </asp:GridView>
                        <table id="OnePage2" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                            <tr height="100%" valign="top">
                                <td class="GridviewScrollPager TD">
                                    <asp:DropDownList ID="ddlPerPageRow2" runat="server" onchange="javascript:ShowRecord('ddlPerPageRow2')" ClientIDMode="Static" AutoPostBack="true">
                                        <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                                        <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px"></td>
                                <td style="font-size: 14px;">
                                    <asp:Label ID="lb_TotalCount2" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>





            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PAY_KIND" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_DOC_NO" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_GV1_DATAKEY" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_GV1_SALARY_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_GV1_SALARY_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_SEQ" runat="server" ClientIDMode="Static" />
            <!--下拉HID-->
            <asp:HiddenField ID="HID_PAY_TARGET_DESC" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_IS_VAILD" runat="server" ClientIDMode="Static" />
            <!--新增HID2-->
            <asp:HiddenField ID="HID_NEW_DOC_NO2" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_AMOUNT" runat="server" ClientIDMode="Static" />
            <!--檢核-->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupC" ShowSummary="false" />
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2sf_AlreadyCheckMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_AlreadyCheckMessage%>" />
            <asp:HiddenField ID="hid_wfb2sf_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_Del_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sf_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_Mod_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sf_AlreadyCheckMessage_Mod" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_AlreadyCheckMessage_Mod%>" />
            <asp:HiddenField ID="hid_wfb2sf_AMOUNT_RequiredEqualZeroMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_AMOUNT_RequiredEqualZeroMessage   %>" />
            <asp:HiddenField ID="hid_wfb2sf_NotAllowEditMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_NotAllowEditMessage%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
