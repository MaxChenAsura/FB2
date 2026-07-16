<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sf/WFB2SF0100_Qry.aspx.cs" Inherits="WebContent_fb2sf_WFB2SF0100_Qry" Culture="auto" UICulture="auto" %>

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
            $('#txt_NEW_EMP_NAME').attr("readonly", true);
            //修改換頁
            $('#txt_EDIT_VENDOR_ID').attr("readonly", true);
            $('#txt_EDIT_MEMO').attr("readonly", true);
            $('#txt_EDIT_MEMODESC').attr("readonly", true);
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
                }
                else {
                    $('#txt_NEW_EMP_NAME').val("");
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
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F",
                    headerrowcount: 1,
                    freezesize: 6
                });
                CheckBoxCheckAllByfreeze("cb_all_gv1_freezeheader", "cb_check_gv1");
            }
            else {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F"
                });
            }
            if ($("#HID_Freeze2").val() == "Y") {
                $('#<%=gv_result2.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F",
                    headerrowcount: 1,
                    freezesize: 4
                });
                CheckBoxCheckAllByfreeze("cb_all_gv2_freezeheader", "cb_check_gv2");
            }
            else {
                $('#<%=gv_result2.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F"
                });
            }
        }
        function add_no_freeze() {
            $("#HID_Freeze").val("N");
        }
        function add_no_freeze2() {
            $("#HID_Freeze2").val("N");
        }

        //gv_result檢查刪除勾選比數
        function CheckDelAction() {
            if (LookUpCheckboxs('gv_result') == 0) {
                alert($('#hid_wfb2sf_Del_NotChoiceMessage').val());
                return false;
            }
        }
        function checkDelClick() {
            if (confirm('確定要刪除?')) {
                $('#Del_AfterConfirm').click();
            }
            else {
                return false;
            }
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

        //gv_result檢查修改勾選比數
        function CheckModeifyAction() {
            if (LookUpCheckboxs('gv_result') != 1) {
                alert($('#hid_wfb2sf_Mod_NotChoiceMessage').val());
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
        //gv_result2檢查修改勾選比數(其他)
        function CheckModeifyAction_Other() {
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

        //EMP_ID不允空白
        function CheckEMP_ID(source, arguments) {
            if ($("#txt_NEW_EMP_ID").val() == "") {
                arguments.IsValid = false;
            }
            else if ($("#txt_NEW_EMP_NAME").val() == "") {
                arguments.IsValid = false;
            }
            else
                arguments.IsValid = true;
        }
        //工號輸入錯誤
        function CheckEMP_ID(source, arguments) {
            if ($("#txt_NEW_EMP_NAME").val() == "") {
                arguments.IsValid = false;
            }
            else
                arguments.IsValid = true;
        }
        //SALARY_RATE需>0
        function CheckSALARY_RATE(source, arguments) {
            if ($("#txt_EDIT_SALARY_RATE").val() != "") {
                if (parseInt($("#txt_EDIT_SALARY_RATE").val(), 10) <= 0 || parseInt($("#txt_EDIT_SALARY_RATE").val(), 10) > 100) {
                    arguments.IsValid = false;
                }
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
        }
        //CheckBONUS_RATE需>0
        function CheckBONUS_RATE(source, arguments) {
            if ($("#txt_EDIT_BONUS_RATE").val() != "") {
                if (parseInt($("#txt_EDIT_BONUS_RATE").val(), 10) <= 0 || parseInt($("#txt_EDIT_BONUS_RATE").val(), 10) > 100) {
                    arguments.IsValid = false;
                }
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
        }
        //SALARY_RATE_Add需>0
        function CheckSALARY_RATE_Add(source, arguments) {
            if ($("#txt_NEW_SALARY_RATE").val() != "") {
                if (parseInt($("#txt_NEW_SALARY_RATE").val(), 10) <= 0 || parseInt($("#txt_NEW_SALARY_RATE").val(), 10) > 100) {
                    arguments.IsValid = false;
                }
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
        }
        //CheckBONUS_RATE_Add需>0
        function CheckBONUS_RATE_Add(source, arguments) {
            if ($("#txt_NEW_BONUS_RATE").val() != "") {
                if (parseInt($("#txt_NEW_BONUS_RATE").val(), 10) <= 0 || parseInt($("#txt_NEW_BONUS_RATE").val(), 10) > 100) {
                    arguments.IsValid = false;
                }
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
        }
        //PAY_TARGET_DESC不可空白
        function CheckPAY_TARGET_DESC(source, arguments) {
            if ($("#ddl_PAY_TARGET_DESC").val() == "" || $("#ddl_PAY_TARGET_DESC").val() == "-1")
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        //if 明細grid.支付對象<>'E' then AMOUNT檢控不允<=0
        function CheckPAY_TARGET_DESC2(source, arguments) {
            if ($("#ddl_PAY_TARGET_DESC").val() != "E") {
                if ($("#txt_EDIT_AMOUNT").val() != undefined && parseInt($("#txt_EDIT_AMOUNT").val(), 10) <= 0) {
                    arguments.IsValid = false;
                }
                if ($("#txt_NEW_AMOUNT").val() != undefined && parseInt($("#txt_NEW_AMOUNT").val(), 10) <= 0) {
                    arguments.IsValid = false;
                }
            }
            else
                arguments.IsValid = true;
        }
        //if 明細grid.支付對象='E' then AMOUNT檢控 須等於零
        function CheckPAY_TARGET_DESC3(source, arguments) {
            if ($("#ddl_PAY_TARGET_DESC").val() == "E") {
                if ($("#txt_EDIT_AMOUNT").val() != undefined && parseInt($("#txt_EDIT_AMOUNT").val(), 10) != 0) {
                    arguments.IsValid = false;
                }
                if ($("#txt_NEW_AMOUNT").val() != undefined && parseInt($("#txt_NEW_AMOUNT").val(), 10) != 0) {
                    arguments.IsValid = false;
                }
            }
            else
                arguments.IsValid = true;
        }
        //gv_result新增塞值
        function addcheck() {
            $("#HID_Freeze").val("Y");
            $("#HID_NEW_EMP_ID").val($("#txt_NEW_EMP_ID").val());
            $("#HID_NEW_EMP_NAME").val($("#txt_NEW_EMP_NAME").val());
            $("#HID_NEW_DOC_NO").val($("#txt_NEW_DOC_NO").val());
            $("#HID_NEW_START_DT").val($("#txt_NEW_START_DT").val());
            $("#HID_NEW_SALARY_RATE").val($("#txt_NEW_SALARY_RATE").val());
            $("#HID_NEW_BONUS_RATE").val($("#txt_NEW_BONUS_RATE").val());
        }
        //gv_result2新增塞值
        function addcheck2() {
            $("#HID_Freeze2").val("Y");
            $("#HID_PAY_TARGET_DESC").val($("#ddl_PAY_TARGET_DESC").val());
            $("#HID_IS_VAILD").val($("#ddl_IS_VAILD").val());
            $("#HID_CHG_STATUS_DESC").val("N");
            $("#HID_NEW_DOC_NO2").val($("#txt_NEW_DOC_NO").val());
            $("#HID_NEW_CREDITOR").val($("#txt_NEW_CREDITOR").val());
            $("#HID_NEW_VENDOR_ID").val($("#txt_NEW_VENDOR_ID").val());
            $("#HID_NEW_AMOUNT").val($("#txt_NEW_AMOUNT").val());
            if ($("#lb_RATIO")[0] != undefined)
                $("#HID_NEW_RATIO").val($("#lb_RATIO")[0].innerText);
            if ($("#lb_EFFECT_SDT")[0] != undefined)
                $("#HID_NEW_EFFECT_SDT").val($("#lb_EFFECT_SDT")[0].innerText);
            if ($("#lb_EFFECT_EDT")[0] != undefined)
                $("#HID_NEW_EFFECT_EDT").val($("#lb_EFFECT_EDT")[0].innerText);
            $("#HID_NEW_MEMO").val($("#txt_NEW_MEMO").val());
            $("#HID_NEW_MEMODESC").val($("#txt_NEW_MEMODESC").val());
        }
        //清空
        function doClear() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_LEAVE_DT_S").val("");
            $("#txt_LEAVE_DT_E").val("");
            $("#txt_DOC_NO").val("");
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
                                <col width="30%" />
                                <col width="15%" />
                                <col width="15%" />
                                <col width="10%" />
                                <col width="15%" />
                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DOC_NO" runat="server" Text="<%$Resources:Resource,wfb2sf_LDOC_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DOC_NO" runat="server" MaxLength="30" ClientIDMode="Static" Text=""></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2sf_START_DT%>"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <uc1:UCDateTimeRange runat="server" ID="UCDateTimeRange" StartDateMaxLength="10" ClientIDMode="Static" ControlClientIDMode="Static" EndDateMaxLength="10" EndDateCssClass="ym" StartDateCssClass="ym"
                                            CompareValidatorOperator="GreaterThanEqual" CompareValidatorErrMesg="<%$Resources:Resource,wfb2sf_greaterthan_START_DT%>" ValidatorStartDateErrMesg="<%$Resources:Resource,wfb2sf_error_START_SDT%>"
                                            ValidatorEndDateErrMesg="<%$Resources:Resource,wfb2sf_error_START_EDT%>" ClientValidationFunctionE="CheckDate" ClientValidationFunctionS="CheckDate" ValidationGroup="GroupA" />
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
                                            <aces:Btn ID="WFB2SF0100Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2SF0100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />

                                            <%--<asp:Button ID="WFB2SF0100Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2SF0100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
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
                            <aces:Btn ID="WFB2SF0100Add" runat="server" Text="<%$Resources:Resource,wfb2sk_add%>" OnClick="WFB2SF0100Add_Click" Visible="true" OnClientClick="add_no_freeze(); BlockUI();" />
                            <aces:Btn ID="WFB2SF0100Delete" runat="server" Text="<%$Resources:Resource,wfb2sk_del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SF0100Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2SF0100Edit" runat="server" Text="<%$Resources:Resource,wfb2sk_edit%>" OnClick="WFB2SF0100Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction(); BlockUI();" />
                            <aces:Btn ID="WFB2SF0100OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" Visible="false" OnClick="WFB2SF0100OK_Click" OnClientClick="addcheck(); CheckValid(); " ValidationGroup="GroupB" />

                            <%--<asp:Button ID="WFB2SF0100Add" runat="server" Text="<%$Resources:Resource,wfb2sk_add%>" OnClick="WFB2SF0100Add_Click" Visible="true" OnClientClick="add_no_freeze(); BlockUI();" />
                            <asp:Button ID="WFB2SF0100Delete" runat="server" Text="<%$Resources:Resource,wfb2sk_del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SF0100Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2SF0100Edit" runat="server" Text="<%$Resources:Resource,wfb2sk_edit%>" OnClick="WFB2SF0100Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction(); BlockUI();" />
                            <asp:Button ID="WFB2SF0100OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" Visible="false" OnClick="WFB2SF0100OK_Click" OnClientClick="addcheck(); CheckValid(); " ValidationGroup="GroupB" />--%>
                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sk_cancel%>" Visible="false" OnClick="btn_cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                        </div>

                    </td>
                </tr>
                <tr>
                    <td>
                        <!--grid1-->
                        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                            SelectCountMethod="GetCount" TypeName="CFB2SF0100DAO" EnablePaging="True"
                            SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                            StartRowIndexParameterName="startRowIndex"
                            OnSelected="ods1_Selected">
                            <SelectParameters>
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                                    Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                                <asp:ControlParameter ControlID="UCDateTimeRange" DefaultValue=""
                                    Name="start_sdt" PropertyName="StartDateText" Type="String" ConvertEmptyStringToNull="false" />
                                <asp:ControlParameter ControlID="UCDateTimeRange" DefaultValue=""
                                    Name="start_edt" PropertyName="EndDateText" Type="String" ConvertEmptyStringToNull="false" />
                                <asp:ControlParameter ControlID="txt_DOC_NO" DefaultValue=""
                                    Name="doc_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                            </SelectParameters>
                        </asp:ObjectDataSource>

                        <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                            OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1300px"
                            OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="cb_all_gv1" Width="20px" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cb_check_gv1" Width="20px" runat="server" ClientIDMode="AutoID" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="text-align: center; width: 20px">
                                            <asp:CheckBox ID="cb_check" Width="20px" runat="server" Checked="true" />
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">

                                    <ItemTemplate>
                                        <asp:Label ID="lb_RowNumber" Width="40px" Style="text-align: center;" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_RowNumber" Width="100%" Style="text-align: center;" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lb_NewRowNumber" Width="100%" Style="text-align: center;" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                    </FooterTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label Width="50px" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: left;" ID="txt_NEW_EMP_ID" runat="server" MaxLength="5"
                                            ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_NEW_EMP_ID', 'txt_NEW_EMP_NAME', 'N', '');" />
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_required_EDIT_EMP_ID%>" ClientValidationFunction="CheckEMP_ID" ForeColor="Red"
                                            ControlToValidate="txt_NEW_EMP_ID" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label Width="50px" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: left;" ID="txt_NEW_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_DOC_NO%>" SortExpression="DOC_NO" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:Label Width="120px" Style="text-align: left;" ID="lb_DOC_NO" runat="server" Text='<%#Bind("DOC_NO")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_DOC_NO" runat="server" Text='<%#Bind("DOC_NO")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: left;" ID="txt_NEW_DOC_NO" runat="server" MaxLength="30"
                                            ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_NEW_DOC_NO%>"
                                            ControlToValidate="txt_NEW_DOC_NO" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_START_DT1%>" SortExpression="START_DT" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label Width="90px" Style="text-align: left;" ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: left;" ID="txt_EDIT_START_DT" runat="server" MaxLength="10" ClientIDMode="Static"
                                            CssClass="MandatoryField date ym" Text='<%#Bind("START_DT","{0:yyyy/MM/dd}")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_EDIT_START_DT%>"
                                            ControlToValidate="txt_EDIT_START_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_error_EDIT_START_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_EDIT_START_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2sf_error_EDIT_START_DT%>" ControlToValidate="txt_EDIT_START_DT" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])[/ /.](0[1-9]|[12][0-9]|3[01])" Display="None"></asp:RegularExpressionValidator>--%>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: left;" ID="txt_NEW_START_DT" runat="server" MaxLength="10"
                                            ClientIDMode="Static" CssClass="MandatoryField date ym"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Required_txt_NEW_START_DT" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_EDIT_START_DT%>"
                                            ControlToValidate="txt_NEW_START_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_error_EDIT_START_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_NEW_START_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                        <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2sf_error_EDIT_START_DT%>" ControlToValidate="txt_NEW_START_DT" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])[/ /.](0[1-9]|[12][0-9]|3[01])" Display="None"></asp:RegularExpressionValidator>--%>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_SALART_RATIO%>" SortExpression="SALARY_RATE" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                    <ItemTemplate>
                                        <asp:Label Width="110px" Style="text-align: right;" ID="lb_SALARY_RATE" runat="server" Text='<%#Bind("SALARY_RATE")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_EDIT_SALARY_RATE" runat="server" MaxLength="3"
                                            CssClass="MandatoryField number" ClientIDMode="Static" Text='<%#Bind("SALARY_RATE")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Required_txt_EDIT_SALARY_RATE" runat="server" ErrorMessage="月薪法扣率不可空白"
                                            ControlToValidate="txt_EDIT_SALARY_RATE" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="月薪法扣率必須大於0且小於等於100" ClientValidationFunction="CheckSALARY_RATE" ForeColor="Red"
                                            ControlToValidate="txt_EDIT_SALARY_RATE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="checknum" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="月薪法扣率格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_EDIT_SALARY_RATE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: right; ime-mode: disabled;" ID="txt_NEW_SALARY_RATE" runat="server" MaxLength="3"
                                            CssClass="MandatoryField number" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="月薪法扣率不可空白"
                                            ControlToValidate="txt_NEW_SALARY_RATE" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="月薪法扣率必須大於0且小於等於100" ClientValidationFunction="CheckSALARY_RATE_Add" ForeColor="Red"
                                            ControlToValidate="txt_NEW_SALARY_RATE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="checknum2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="月薪法扣率格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_NEW_SALARY_RATE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_BONUS_RATIO%>" SortExpression="BONUS_RATE" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                    <ItemTemplate>
                                        <asp:Label Width="110px" Style="text-align: right;" ID="lb_BONUS_RATE" runat="server" Text='<%#Bind("BONUS_RATE")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_EDIT_BONUS_RATE" runat="server" MaxLength="3"
                                            CssClass="MandatoryField number" ClientIDMode="Static" Text='<%#Bind("BONUS_RATE")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Required_txt_EDIT_BONUS_RATE" runat="server" ErrorMessage="獎金法扣率不可空白"
                                            ControlToValidate="txt_EDIT_BONUS_RATE" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="獎金法扣率必須大於0且小於等於100" ClientValidationFunction="CheckBONUS_RATE" ForeColor="Red"
                                            ControlToValidate="txt_EDIT_BONUS_RATE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="checknum3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="獎金法扣率格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_EDIT_BONUS_RATE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_NEW_BONUS_RATE" runat="server" MaxLength="3"
                                            CssClass="MandatoryField number" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Required_txt_NEW_BONUS_RATE" runat="server" ErrorMessage="獎金法扣率不可空白"
                                            ControlToValidate="txt_NEW_BONUS_RATE" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="獎金法扣率必須大於0且小於等於100" ClientValidationFunction="CheckBONUS_RATE_Add" ForeColor="Red"
                                            ControlToValidate="txt_NEW_BONUS_RATE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="checknum4" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="獎金法扣率格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_NEW_BONUS_RATE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_AMOUNT%>" SortExpression="AMOUNT" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:Label Width="70px" Style="text-align: right;" ID="lb_AMOUNT" runat="server" Text='<%#Bind("AMOUNT","{0:N0}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: right;" ID="lb_AMOUNT" runat="server" Text='<%#Bind("AMOUNT")%>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_TOTAL_AMT%>" SortExpression="TOTAL_AMT" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label Width="90px" Style="text-align: right;" ID="lb_TOTAL_AMT" runat="server" Text='<%#Bind("TOTAL_AMT","{0:N0}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: right;" ID="lb_TOTAL_AMT" runat="server" Text='<%#Bind("TOTAL_AMT")%>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_SURE_YN%>" SortExpression="SURE_YN" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label Width="50px" Style="text-align: left;" ID="lb_SURE_YN" runat="server" Text='<%#Bind("SURE_YN")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_SURE_YN" runat="server" Text='<%#Bind("SURE_YN")%>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_SUB_DESC%>" SortExpression="APPROVE_STATUS_DESC" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:Label Width="70px" Style="text-align: left;" ID="lb_APPROVE_STATUS_DESC" runat="server" ClientIDMode="Static" Text='<%#Bind("APPROVE_STATUS_DESC")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_APPROVE_STATUS_DESC" runat="server" ClientIDMode="Static" Text='<%#Bind("APPROVE_STATUS_DESC")%>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_APPROVE_DT%>" SortExpression="APPROVE_DT" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label Width="90px" Style="text-align: left;" ID="lb_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_EMP_NAME_APPROVE%>" SortExpression="APPROVE_BY_NAME" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:Label Width="70px" Style="text-align: left;" ID="lb_APPROVE_BY_NAME" runat="server" Text='<%#Bind("APPROVE_BY_NAME")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_APPROVE_BY_NAME" runat="server" Text='<%#Bind("APPROVE_BY_NAME")%>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_APP_REMARK%>" SortExpression="APP_REMARK" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:Label Width="120px" Style="text-align: left;" ID="lb_APP_REMARK" runat="server" Text='<%#Bind("APP_REMARK")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_APP_REMARK" runat="server" Text='<%#Bind("APP_REMARK")%>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_FUNCTION%>" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <aces:Btn ID="WFB2SF0100Dtl" runat="server"
                                            CommandName="ToDtl"
                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text="<%$Resources:Resource,wfb2sf_WFB2SF0100Dtl%>" />
                                        <aces:Btn ID="WFB2SF0100DataCheck" runat="server"
                                            CommandName="ToDataCheck"
                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text="<%$Resources:Resource,wfb2sf_WFB2SF0100DataCheck%>" />

                                        <%--<asp:Button ID="WFB2SF0100Dtl" runat="server"
                                            CommandName="ToDtl"
                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text="<%$Resources:Resource,wfb2sf_WFB2SF0100Dtl%>" />
                                        <asp:Button ID="WFB2SF0100DataCheck" runat="server"
                                            CommandName="ToDataCheck"
                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text="<%$Resources:Resource,wfb2sf_WFB2SF0100DataCheck%>" />--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <EmptyDataTemplate>
                                <div style="width: 1020px; overflow: auto">
                                    <table class="grid-view" width="1020px">
                                        <tr class="header">
                                            <td>
                                                <asp:CheckBox ID="cb_checkall" Width="20px" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" Width="40px" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" Width="50px" runat="server" Text="<%$Resources:Resource,wfb2ia_EMP_ID%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label17" Width="50px" runat="server" Text="<%$Resources:Resource,wfb2sf_EMP_NAME%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" Width="120px" runat="server" Text="<%$Resources:Resource,wfb2sf_DOC_NO%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label5" Width="90px" runat="server" Text="<%$Resources:Resource,wfb2sf_START_DT1%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" Width="110px" runat="server" Text="<%$Resources:Resource,wfb2sf_SALART_RATIO%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label7" Width="110px" runat="server" Text="<%$Resources:Resource,wfb2sf_BONUS_RATIO%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label14" Width="70px" runat="server" Text="<%$Resources:Resource,wfb2sf_AMOUNT%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label15" Width="90px" runat="server" Text="<%$Resources:Resource,wfb2sf_TOTAL_AMT%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label16" Width="50px" runat="server" Text="<%$Resources:Resource,wfb2sf_SURE_YN%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label18" Width="70px" runat="server" Text="<%$Resources:Resource,wfb2sf_SUB_DESC%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label19" Width="90px" runat="server" Text="<%$Resources:Resource,wfb2sf_APPROVE_DT%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label20" Width="70px" runat="server" Text="<%$Resources:Resource,wfb2sf_EMP_NAME_APPROVE%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label21" Width="120px" runat="server" Text="<%$Resources:Resource,wfb2sf_APP_REMARK%>"></asp:Label>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr class="normal">
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lb_EmptyRowNumber" runat="server" Text="1"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="50px" Style="text-align: left;" ID="txt_NEW_EMP_ID" runat="server" MaxLength="5"
                                                    ClientIDMode="Static" CssClass="MandatoryField" onblur="empCheck(this.value,'add');"></asp:TextBox>
                                                <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_NEW_EMP_ID', 'txt_NEW_EMP_NAME', 'N', '');" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_EDIT_EMP_ID%>"
                                                    ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="50px" Style="text-align: left;" ID="txt_NEW_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="120px" Style="text-align: left;" ID="txt_NEW_DOC_NO" runat="server" MaxLength="30"
                                                    ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_NEW_DOC_NO%>"
                                                    ControlToValidate="txt_NEW_DOC_NO" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="80px" Style="text-align: left;" ID="txt_NEW_START_DT" runat="server" MaxLength="30"
                                                    ClientIDMode="Static" CssClass="MandatoryField ym date"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_EDIT_START_DT%>"
                                                    ControlToValidate="txt_NEW_START_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="qrytest3" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="<%$Resources:Resource,wfb2sf_error_EDIT_START_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                    ControlToValidate="txt_NEW_START_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                        ErrorMessage="<%$Resources:Resource,wfb2sf_error_EDIT_START_DT%>" ControlToValidate="txt_NEW_START_DT" ForeColor="Red" ValidationGroup="GroupB"
                                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])[/ /.](0[1-9]|[12][0-9]|3[01])" Display="None"></asp:RegularExpressionValidator>--%>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="110px" Style="text-align: right;" ID="txt_NEW_SALARY_RATE" runat="server" MaxLength="3"
                                                    CssClass="MandatoryField number" ClientIDMode="Static"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="月薪法扣率不可空白"
                                                    ControlToValidate="txt_NEW_SALARY_RATE" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="CustomValidator5" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="月薪法扣率必須大於0且小於等於100" ClientValidationFunction="CheckSALARY_RATE_Add" ForeColor="Red"
                                                    ControlToValidate="txt_NEW_SALARY_RATE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                                <asp:CustomValidator ID="checknum5" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="月薪法扣率格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                                    ControlToValidate="txt_NEW_SALARY_RATE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="110px" Style="text-align: right;" ID="txt_NEW_BONUS_RATE" runat="server" MaxLength="3"
                                                    CssClass="MandatoryField number" ClientIDMode="Static"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="獎金法扣率不可空白"
                                                    ControlToValidate="txt_NEW_BONUS_RATE" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="CustomValidator6" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="獎金法扣率必須大於0且小於等於100" ClientValidationFunction="CheckBONUS_RATE_Add" ForeColor="Red"
                                                    ControlToValidate="txt_NEW_BONUS_RATE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                                <asp:CustomValidator ID="checknum6" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="獎金法扣率格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                                    ControlToValidate="txt_NEW_BONUS_RATE" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
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
                                        <aces:Btn ID="WFB2SF0101Add" runat="server" Text="<%$Resources:Resource,wfb2sk_add%>" OnClick="WFB2SF0101Add_Click" Visible="false" OnClientClick="add_no_freeze2(); BlockUI();" />
                                        <aces:Btn ID="WFB2SF0101Delete" runat="server" Text="<%$Resources:Resource,wfb2sk_del%>" OnClientClick="return CheckDelAction_Dtl();" OnClick="WFB2SF0101Delete_Click" Visible="false" />
                                        <aces:Btn ID="WFB2SF0101Edit" runat="server" Text="<%$Resources:Resource,wfb2sk_edit%>" OnClick="WFB2SF0101Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction_Dtl(); BlockUI();" />
                                        <aces:Btn ID="WFB2SF0102Edit" runat="server" Text="廠商CODE_原因_備註修改" OnClick="WFB2SF0102Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction_Other(); BlockUI();" />
                                        <aces:Btn ID="WFB2SF0101OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" Visible="false" OnClick="WFB2SF0101OK_Click" OnClientClick="addcheck2(); CheckValid(); " ValidationGroup="GroupC" />

                                        <%--<asp:Button ID="WFB2SF0101Add" runat="server" Text="<%$Resources:Resource,wfb2sk_add%>" OnClick="WFB2SF0101Add_Click" Visible="false" OnClientClick="add_no_freeze2(); BlockUI();" />
                                        <asp:Button ID="WFB2SF0101Delete" runat="server" Text="<%$Resources:Resource,wfb2sk_del%>" OnClientClick="return CheckDelAction_Dtl();" OnClick="WFB2SF0101Delete_Click" Visible="false" />
                                        <asp:Button ID="WFB2SF0101Edit" runat="server" Text="<%$Resources:Resource,wfb2sk_edit%>" OnClick="WFB2SF0101Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction_Dtl(); BlockUI();" />
                                        <asp:Button ID="WFB2SF0102Edit" runat="server" Text="廠商CODE_原因_備註修改" OnClick="WFB2SF0102Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction_Other(); BlockUI();" />
                                        <asp:Button ID="WFB2SF0101OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" Visible="false" OnClick="WFB2SF0101OK_Click" OnClientClick="addcheck2(); CheckValid(); " ValidationGroup="GroupC" />--%>
                                        <asp:Button ID="btn_cancel2" runat="server" Text="<%$Resources:Resource,wfb2sk_cancel%>" Visible="false" OnClick="btn_cancel2_Click" OnClientClick="return confirm('是否確定取消?');" />
                                    </div>

                                </td>
                            </tr>
                        </table>

                        <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="GetData2"
                            SelectCountMethod="GetCount2" TypeName="CFB2SF0100DAO" EnablePaging="True"
                            SortParameterName="sortExpression" OnSelecting="obs1_Selecting2"
                            StartRowIndexParameterName="startRowIndex"
                            OnSelected="ods1_Selected2">
                            <SelectParameters>
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:ControlParameter ControlID="HID_EMP_ID" DefaultValue=""
                                    Name="EMP_ID" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                            </SelectParameters>
                        </asp:ObjectDataSource>

                        <%--<asp:GridView ID="gv_result2" runat="server" AllowPaging="true" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True"
                OnRowDataBound="gv_result2_RowDataBound" OnRowCreated="gv_result2_RowCreated" Width="1200px"
                OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result2_DataBound">--%>

                        <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="true"
                            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result2_Sorting"
                            OnRowDataBound="gv_result2_RowDataBound" OnRowCreated="gv_result2_RowCreated" Width="1200px"
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
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_CHG_STATUS%>" SortExpression="CHG_STATUS_DESC" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:Label Width="60px" Style="text-align: left;" ID="lb_CHG_STATUS_DESC" runat="server" Text='<%#Bind("CHG_STATUS_DESC")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_CHG_STATUS_DESC" runat="server" Text="U-修改"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_CHG_STATUS_DESC" runat="server" Text="N-新增"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_DOC_NO%>" SortExpression="DOC_NO" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label Width="150px" Style="text-align: left;" ID="lb_DOC_NO" runat="server" Text='<%#Bind("DOC_NO")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_DOC_NO" runat="server" Text='<%#Bind("DOC_NO")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: left;" ID="txt_NEW_DOC_NO" runat="server" MaxLength="30" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_NEW_DOC_NO%>"
                                            ControlToValidate="txt_NEW_DOC_NO" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_PAY_TARGET%>" SortExpression="PAY_TARGET_DESC" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:Label Width="70px" Style="text-align: left;" ID="lb_PAY_TARGET_DESC" runat="server" Text='<%#Bind("PAY_TARGET_DESC")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList Width="100%" Style="text-align: left;" ID="ddl_PAY_TARGET_DESC" runat="server" ClientIDMode="Static" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_PAY_TARGET_DESC_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <asp:CustomValidator ID="CustomValidator7" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_required_PAY_TARGET_DESC%>" ClientValidationFunction="CheckPAY_TARGET_DESC" ForeColor="Red"
                                            ControlToValidate="ddl_PAY_TARGET_DESC" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator9" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_error_AMOUNT%>" ClientValidationFunction="CheckPAY_TARGET_DESC2" ForeColor="Red"
                                            ControlToValidate="ddl_PAY_TARGET_DESC" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator10" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_error_AMOUNT2%>" ClientValidationFunction="CheckPAY_TARGET_DESC3" ForeColor="Red"
                                            ControlToValidate="ddl_PAY_TARGET_DESC" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList Width="100%" Style="text-align: left;" ID="ddl_PAY_TARGET_DESC" runat="server" ClientIDMode="Static" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_PAY_TARGET_DESC_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <asp:CustomValidator ID="CustomValidator8" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_required_PAY_TARGET_DESC%>" ClientValidationFunction="CheckPAY_TARGET_DESC" ForeColor="Red"
                                            ControlToValidate="ddl_PAY_TARGET_DESC" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator11" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_error_AMOUNT%>" ClientValidationFunction="CheckPAY_TARGET_DESC2" ForeColor="Red"
                                            ControlToValidate="ddl_PAY_TARGET_DESC" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator12" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_error_AMOUNT2%>" ClientValidationFunction="CheckPAY_TARGET_DESC3" ForeColor="Red"
                                            ControlToValidate="ddl_PAY_TARGET_DESC" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_CREDITOR%>" SortExpression="CREDITOR" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                    <ItemTemplate>
                                        <asp:Label Width="110px" Style="text-align: left;" ID="lb_CREDITOR" runat="server" Text='<%#Bind("CREDITOR")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: left;" ID="txt_EDIT_CREDITOR" runat="server" CssClass="MandatoryField" MaxLength="30" ClientIDMode="Static" Text='<%#Bind("CREDITOR")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_CREDITOR%>"
                                            ControlToValidate="txt_EDIT_CREDITOR" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: left;" ID="txt_NEW_CREDITOR" runat="server" CssClass="MandatoryField" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_CREDITOR%>"
                                            ControlToValidate="txt_NEW_CREDITOR" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_VENDOR_ID%>" SortExpression="VENDOR_ID" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label Width="80px" Style="text-align: left;" ID="lb_VENDOR_ID" runat="server" Text='<%#Bind("VENDOR_ID")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="80px" Style="text-align: left;" ID="txt_EDIT_VENDOR_ID" runat="server" Text='<%#Bind("VENDOR_ID")%>'></asp:Label>
                                        <%--<asp:TextBox Width="100%" Style="text-align: left;" ID="txt_EDIT_VENDOR_ID" runat="server" MaxLength="10" ClientIDMode="Static" Text='<%#Bind("VENDOR_ID")%>'></asp:TextBox>--%>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: left;" ID="txt_NEW_VENDOR_ID" runat="server" MaxLength="10" ClientIDMode="Static"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_AMOUNT1%>" SortExpression="AMOUNT" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label Width="80px" Style="text-align: right;" ID="lb_AMOUNT" runat="server" Text='<%#Bind("AMOUNT","{0:N0}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_EDIT_AMOUNT" runat="server" MaxLength="7" CssClass="number7 MandatoryField" ClientIDMode="Static" Text='<%#Bind("AMOUNT")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_AMOUNT%>"
                                            ControlToValidate="txt_EDIT_AMOUNT" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="checknum7" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="債權金額格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_EDIT_AMOUNT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: right;" ID="txt_NEW_AMOUNT" runat="server" MaxLength="7" CssClass="number7 MandatoryField" ClientIDMode="Static" Text="0"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_AMOUNT%>"
                                            ControlToValidate="txt_NEW_AMOUNT" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="checknum8" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="債權金額格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_NEW_AMOUNT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_TOTAL_AMT1%>" SortExpression="TOTAL_AMT" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label Width="80px" Style="text-align: right;" ID="lb_TOTAL_AMT" ClientIDMode="Static" runat="server" Text='<%#Bind("TOTAL_AMT","{0:N0}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Width="80px" Style="text-align: right;" ID="lb_TOTAL_AMT" ClientIDMode="Static" runat="server" CssClass="number7" Text="0"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_RATIO%>" SortExpression="RATIO" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label Width="100px" Style="text-align: right;" ID="lb_RATIO" runat="server" Text='<%#Bind("RATIO")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: right;" ID="lb_RATIO" ClientIDMode="Static" runat="server" Text='<%#Bind("RATIO")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Width="100%" Style="text-align: right;" ID="lb_RATIO" ClientIDMode="Static" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_MEMO%>" SortExpression="MEMO" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:Label Width="120px" Style="text-align: left;" ID="lb_MEMO" runat="server" Text='<%#Bind("MEMO")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="120px" Style="text-align: left;" ID="txt_EDIT_MEMO" runat="server" Text='<%#Bind("MEMO")%>'></asp:Label>
                                        <%--<asp:TextBox Width="100%" Style="text-align: left;" ID="txt_EDIT_MEMO" runat="server" MaxLength="30" ClientIDMode="Static" Text='<%#Bind("MEMO")%>'></asp:TextBox>--%>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: left;" ID="txt_NEW_MEMO" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_EFFECT_SDT%>" SortExpression="EFFECT_SDT" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:Label Width="120px" Style="text-align: left;" ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT","{0:yyyy/MM/dd}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT","{0:yyyy/MM/dd}")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_EFFECT_SDT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_EFFECT_EDT%>" SortExpression="EFFECT_EDT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label Width="100px" Style="text-align: left;" ID="lb_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT","{0:yyyy/MM/dd}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT","{0:yyyy/MM/dd}")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Width="100%" Style="text-align: left;" ID="lb_EFFECT_EDT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_IS_VAILD%>" SortExpression="IS_VAILD" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label Width="80px" Style="text-align: left;" ID="lb_IS_VAILD" runat="server" ClientIDMode="Static" Text='<%#Bind("IS_VAILD")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList Width="100%" Style="text-align: left;" ID="ddl_IS_VAILD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_IS_VAILD_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList Width="100%" Style="text-align: left;" ID="ddl_IS_VAILD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_IS_VAILD_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_MEMODESC%>" SortExpression="MEMODESC" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:Label Width="120px" Style="text-align: left;" ID="lb_MEMODESC" runat="server" Text='<%#Bind("MEMODESC")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Width="120px" Style="text-align: left;" ID="txt_EDIT_MEMODESC" runat="server" Text='<%#Bind("MEMODESC")%>'></asp:Label>
                                        <%--<asp:TextBox Width="100%" Style="text-align: left;" ID="txt_EDIT_MEMODESC" runat="server" MaxLength="30" ClientIDMode="Static" Text='<%#Bind("MEMODESC")%>'></asp:TextBox>--%>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox Width="100%" Style="text-align: left;" ID="txt_NEW_MEMODESC" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <EmptyDataTemplate>
                                <div style="width: 1020px; overflow: auto">
                                    <table class="grid-view" width="1020px">
                                        <tr class="header">
                                            <td>
                                                <asp:CheckBox ID="cb_checkall" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2sf_CHG_STATUS%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sf_DOC_NO%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sf_PAY_TARGET%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sf_CREDITOR%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2sf_VENDOR_ID%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2sf_AMOUNT1%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sf_TOTAL_AMT1%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sf_RATIO%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sf_MEMO%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sf_EFFECT_SDT%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2sf_EFFECT_EDT%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2sf_IS_VAILD%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2sf_MEMODESC%>"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="normal">
                                            <td></td>
                                            <td>
                                                <asp:Label ID="Label22" Width="40px" runat="server" Text="1"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label Width="60px" Style="text-align: left;" ID="lb_CHG_STATUS_DESC" runat="server" Text="N-新增"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="150px" Style="text-align: left;" ID="txt_NEW_DOC_NO" runat="server" MaxLength="30" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_NEW_DOC_NO%>"
                                                    ControlToValidate="txt_NEW_DOC_NO" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList Width="70px" Style="text-align: left;" ID="ddl_PAY_TARGET_DESC" runat="server" ClientIDMode="Static" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_PAY_TARGET_DESC_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                <asp:CustomValidator ID="CustomValidator8" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="<%$Resources:Resource,wfb2sf_required_PAY_TARGET_DESC%>" ClientValidationFunction="CheckPAY_TARGET_DESC" ForeColor="Red"
                                                    ControlToValidate="ddl_PAY_TARGET_DESC" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator11" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="<%$Resources:Resource,wfb2sf_error_AMOUNT%>" ClientValidationFunction="CheckPAY_TARGET_DESC2" ForeColor="Red"
                                                    ControlToValidate="ddl_PAY_TARGET_DESC" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator12" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="<%$Resources:Resource,wfb2sf_error_AMOUNT2%>" ClientValidationFunction="CheckPAY_TARGET_DESC3" ForeColor="Red"
                                                    ControlToValidate="ddl_PAY_TARGET_DESC" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="110px" Style="text-align: left;" ID="txt_NEW_CREDITOR" runat="server" CssClass="MandatoryField" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_CREDITOR%>"
                                                    ControlToValidate="txt_NEW_CREDITOR" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="80px" Style="text-align: left;" ID="txt_NEW_VENDOR_ID" runat="server" MaxLength="10" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="80px" Style="text-align: right;" ID="txt_NEW_AMOUNT" runat="server" MaxLength="7" CssClass="number7 MandatoryField" ClientIDMode="Static" Text="0"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_AMOUNT%>"
                                                    ControlToValidate="txt_NEW_AMOUNT" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="checknum9" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="債權金額格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                                    ControlToValidate="txt_NEW_AMOUNT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                            </td>
                                            <td>
                                                <asp:Label Width="80px" Style="text-align: right;" ID="lb_TOTAL_AMT" ClientIDMode="Static" runat="server" CssClass="number7" Text="0"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label Width="100px" Style="text-align: right;" ID="lb_RATIO" ClientIDMode="Static" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="120px" Style="text-align: left;" ID="txt_NEW_MEMO" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label Width="120px" Style="text-align: left;" ID="lb_EFFECT_SDT" runat="server" ClientIDMode="Static"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label Width="100px" Style="text-align: left;" ID="lb_EFFECT_EDT" runat="server" ClientIDMode="Static"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList Width="80px" Style="text-align: left;" ID="ddl_IS_VAILD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_IS_VAILD_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                                    <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="120px" Style="text-align: left;" ID="txt_NEW_MEMODESC" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
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
            <asp:HiddenField ID="HID_VALUE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_EMP_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_DOC_NO" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_SURE_YN" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:HiddenField ID="HID_Freeze2" runat="server" ClientIDMode="Static" Value="Y" />
            <!--下拉HID-->
            <asp:HiddenField ID="HID_PAY_TARGET_DESC" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_IS_VAILD" runat="server" ClientIDMode="Static" />
            <!--新增HID-->
            <asp:HiddenField ID="HID_NEW_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_EMP_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_DOC_NO" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_START_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_SALARY_RATE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_BONUS_RATE" runat="server" ClientIDMode="Static" />
            <!--新增HID2-->
            <asp:HiddenField ID="HID_CHG_STATUS_DESC" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_DOC_NO2" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_CREDITOR" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_VENDOR_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_AMOUNT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_RATIO" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_EFFECT_SDT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_EFFECT_EDT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_MEMO" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_MEMODESC" runat="server" ClientIDMode="Static" />
            <!--檢核-->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupC" ShowSummary="false" />
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2sf_AlreadyCheckMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_AlreadyCheckMessage%>" />
            <asp:HiddenField ID="hid_wfb2sf_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_Del_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sf_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_Mod_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sf_AlreadyCheckMessage_Mod" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_AlreadyCheckMessage_Mod%>" />
            <asp:HiddenField ID="hid_wfb2sf_AMOUNT_RequiredEqualZeroMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_AMOUNT_RequiredEqualZeroMessage   %>" />
            <asp:HiddenField ID="hid_wfb2sf_NotAllowEditMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_NotAllowEditMessage%>" />
            <asp:Button ID="Del_AfterConfirm" runat="server" ClientIDMode="Static" OnClick="Del_AfterConfirm_Click" Style="display:none;" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
