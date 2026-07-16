<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC4200_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC4200_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".ym").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $('.ymd').mask('9999/99/99');
            $('.num1').mask('9');
            $('.num5').mask('99999');
            $('.num7').mask('9999999');
            $('#txt_EMP_NAME_Add').attr("readonly", true);
            gridviewScroll();
            $.unblockUI();

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

            //新增工號取得姓名的ajax
            $("#txt_EMP_ID_Add").change(function () {
                if ($("#txt_EMP_ID_Add").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID_Add').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_ID_Add').val("");
                                $('#txt_EMP_NAME_Add').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME_Add').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME_Add').val("");
                }
            });
        }
        function ShowRecord(obj) {
            if (obj == "grid1")
                $("#HID_PageRow").val($("#ddlPerPageRow").val());
            else if (obj == "grid2")
                $("#HID_PageRow2").val($("#ddlPerPageRow2").val());
            else if (obj == "grid3")
                $("#HID_PageRow3").val($("#ddlPerPageRow3").val());
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        function CheckAMOUNT(source, arguments) {
            var re = /^(0|[1-9][0-9]*)$/;
            if ($("#txt_AMOUNT_Add").val() != "") {
                if (!re.test($("#txt_AMOUNT_Add").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
        }
        function CheckTOTAL_AMT(source, arguments) {
            var re = /^(0|[1-9][0-9]*)$/;
            if ($("#txt_TOTAL_AMT_Add").val() != "") {
                if (!re.test($("#txt_TOTAL_AMT_Add").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
        }
        function CheckVALUE(source, arguments) {
            var re = /^(0|[1-9][0-9]*)$/;
            if ($("#txt_VALUE_Add").val() != "") {
                if (!re.test($("#txt_VALUE_Add").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
        }
        function CheckDelAction(checkboxID) {
            if (LookUpCheckboxs(checkboxID) > 0)
                return confirm($('#hidwfb2sc_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb2sc_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckModeifyAction(checkboxID) {
            if (LookUpCheckboxs(checkboxID) == 1)
                return true;
            else {
                alert($('#hidwfb2sc_Mod_NotChoiceMessage').val());
                return false;
            }
        }
        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hidwfb2sc_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hidwfb2sc_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hidwfb2sc_Save_ConfirmMessage').val());
        }

        function LookUpCheckboxs(checkboxID) {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf(checkboxID) != -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function delayedShow() {
            if (Page_IsValid != null && Page_IsValid == true) {
                BlockUI();
            }
        }
        function ClearAll() {
            $('#txt_EMP_NAME_search').val("");
            $('#txt_EMP_ID_search').val("");
            $('#txt_LEAVE_DT_S').val("");
            $('#txt_LEAVE_DT_E').val("");
            $('#ddl_ARREARS_TYPE_search').val("");
            return false;
        }
        function MySelectAllCheckboxes(spanChk, checkboxID) {
            elm = document.forms[0];
            for (i = 0; i <= elm.length - 1; i++) {

                if (elm[i].type == "checkbox" && elm[i].id != spanChk.id && elm[i].id.indexOf(checkboxID) != -1) {

                    if (elm.elements[i].checked != spanChk.checked)
                        $('#' + elm[i].id).prop('checked', spanChk.checked);
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                <colgroup>
                    <col width="15%" />
                    <col width="35%" />
                    <col width="15%" />
                    <col width="35%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_ID%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID_search" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_search', 'txt_EMP_NAME_search', 'N', '');" />

                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_NAME_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_NAME2%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_NAME_search" runat="server" ClientIDMode="Static" Width="64px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEBIT_DT_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_DEBIT_DT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <uc1:UCDateTimeRange runat="server" ID="UCDateTimeRange1" StartDateMaxLength="10" ClientIDMode="Static" ControlClientIDMode="Static" EndDateMaxLength="10" EndDateCssClass="ymd" StartDateCssClass="ymd"
                                CompareValidatorOperator="GreaterThanEqual" CompareValidatorErrMesg="<%$Resources:Resource,wfb2sc_lb_DEBIT_DT_isError%>" ValidatorStartDateErrMesg="<%$Resources:Resource,wfb2sc_lb_DEBIT_DTS_isNull%>"
                                ValidatorEndDateErrMesg="<%$Resources:Resource,wfb2sc_lb_DEBIT_DTE_isNull%>" ClientValidationFunctionE="CheckDate" ClientValidationFunctionS="CheckDate" ValidationGroup="GroupA" />
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ARREARS_TYPE_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_ARREARS_TYPE%>"></asp:Label>
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_ARREARS_TYPE_search" runat="server" Width="100px" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="4">
                            <div id="init">
                                <aces:Btn ID="WFB2SC4200Search1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Search%>" OnClick="WFB2SC4200Search1_Click" ValidationGroup="GroupA" />
                                <%--<asp:Button ID="WFB2SC4200Search1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Search%>" OnClick="WFB2SC4200Search1_Click" ValidationGroup="GroupA" />--%>
                                <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_clear%>" OnClientClick="return ClearAll();" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right" class="Body_label">
                            <div id="init_grid">
                                <aces:Btn ID="WFB2SC4200Add1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC4200Add1_Click" />
                                <aces:Btn ID="WFB2SC4200Delete1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction('cb_check1');" OnClick="WFB2SC4200Delete1_Click" Visible="False" />
                                <aces:Btn ID="WFB2SC4200Edit1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Edit%>" OnClientClick="return CheckModeifyAction('cb_check1');" OnClick="WFB2SC4200Edit1_Click" Visible="False" />
                                <aces:Btn ID="WFB2SC4200Save1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Save%>" Visible="false" OnClick="WFB2SC4200Save1_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                <%--<asp:Button ID="WFB2SC4200Add1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC4200Add1_Click" />
                            <asp:Button ID="WFB2SC4200Delete1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction('cb_check1');" OnClick="WFB2SC4200Delete1_Click" Visible="False" />
                            <asp:Button ID="WFB2SC4200Edit1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Edit%>" OnClientClick="return CheckModeifyAction('cb_check1');" OnClick="WFB2SC4200Edit1_Click" Visible="False" />
                            <asp:Button ID="WFB2SC4200Save1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Save%>" Visible="false" OnClick="WFB2SC4200Save1_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                                <asp:Button ID="btn_cancel1" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_cancel%>" Visible="false" OnClick="btn_cancel1_Click" OnClientClick="return confirm($('#hidwfb2sc_Cancel_ConfirmMessage').val());" />
                            </div>
                        </td>
                    </tr>
                </tbody>

            </table>
            <!--grid1-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SC4200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID_search" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME_search" DefaultValue=""
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UCDateTimeRange1" DefaultValue=""
                        Name="debit_sdt" PropertyName="StartDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UCDateTimeRange1" DefaultValue=""
                        Name="debit_edt" PropertyName="EndDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_ARREARS_TYPE_search" DefaultValue=""
                        Name="arrears_type" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1300px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:MySelectAllCheckboxes(this,'cb_check1');" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check1" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="40px" Style="text-align: left;" ID="txt_EMP_ID_Add" CssClass="MandatoryField" runat="server" MaxLength="5" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_Add', 'txt_EMP_NAME_Add', 'N', '');" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_EMP_ID_isNull%>"
                                ControlToValidate="txt_EMP_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_EMP_NAME2%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="60px" ID="txt_EMP_NAME_Add" BorderWidth="0" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--積欠日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_DEBIT_DT2%>" SortExpression="DEBIT_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEBIT_DT" runat="server" Text='<%#Bind("DEBIT_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="80px" ID="txt_DEBIT_DT_Add" CssClass="ymd date MandatoryField" runat="server" MaxLength="8" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_DEBIT_DT_isNull%>"
                                ControlToValidate="txt_DEBIT_DT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_DEBIT_DT_isError2%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_DEBIT_DT_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--總金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_AMOUNT%>" SortExpression="AMOUNT" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_AMOUNT" runat="server" Text='<%#Bind("AMOUNT","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="60px" Style="text-align: right;" ID="txt_AMOUNT_Add" CssClass="MandatoryField num7" ClientIDMode="Static" MaxLength="7" runat="server" Text='<%#Bind("AMOUNT")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_AMOUNT_isNull%>"
                                ControlToValidate="txt_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_AMOUNT_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_AMOUNT_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="60px" Style="text-align: right;" ID="txt_AMOUNT_Add" CssClass="MandatoryField num7" runat="server" MaxLength="7" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_AMOUNT_isNull%>"
                                ControlToValidate="txt_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_AMOUNT_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_AMOUNT_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--積欠種類--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_ARREARS_TYPE2%>" SortExpression="ARREARS_TYPE" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_ARREARS_TYPE" runat="server" Text='<%#Bind("ARREARS_TYPE_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList Width="100%" ID="ddl_ARREARS_TYPE_Add" CssClass="MandatoryField" ClientIDMode="Static" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_ARREARS_TYPE2_isNull%>"
                                ControlToValidate="ddl_ARREARS_TYPE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList Width="100%" ID="ddl_ARREARS_TYPE_Add" CssClass="MandatoryField" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_ARREARS_TYPE2_isNull%>"
                                ControlToValidate="ddl_ARREARS_TYPE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--優先順序--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_CAL_ORDER%>" SortExpression="CAL_ORDER" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_CAL_ORDER" runat="server" Text='<%#Bind("CAL_ORDER")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="30px" ID="txt_CAL_ORDER_Add" ClientIDMode="Static" CssClass="num1" MaxLength="1" runat="server" Text='<%#Bind("CAL_ORDER")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="30px" ID="txt_CAL_ORDER_Add" runat="server" CssClass="num1" MaxLength="1" ClientIDMode="Static"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--累計已扣金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_TOTAL_AMT%>" SortExpression="TOTAL_AMT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TOTAL_AMT" runat="server" Text='<%#Bind("TOTAL_AMT","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="80px" Style="text-align: right;" ID="txt_TOTAL_AMT_Add" ClientIDMode="Static" CssClass="num7" MaxLength="7" runat="server" Text='<%#Bind("TOTAL_AMT")%>' />
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_TOTAL_AMT_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_TOTAL_AMT_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="80px" Style="text-align: right;" ID="txt_TOTAL_AMT_Add" runat="server" CssClass="num7" MaxLength="7" ClientIDMode="Static"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_TOTAL_AMT_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_TOTAL_AMT_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--還款方式--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_REPAY_TYPE%>" SortExpression="REPAY_TYPE" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_REPAY_TYPE" runat="server" Text='<%#Bind("REPAY_TYPE_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList Width="100%" ID="ddl_REPAY_TYPE_Add" CssClass="MandatoryField" ClientIDMode="Static" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_REPAY_TYPE_isNull%>"
                                ControlToValidate="ddl_REPAY_TYPE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList Width="100%" ID="ddl_REPAY_TYPE_Add" CssClass="MandatoryField" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_REPAY_TYPE_isNull%>"
                                ControlToValidate="ddl_REPAY_TYPE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--比率金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_VALUE%>" SortExpression="VALUE" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_VALUE" runat="server" Text='<%#Bind("VALUE","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="60px" Style="text-align: right;" ID="txt_VALUE_Add" CssClass="MandatoryField num5" ClientIDMode="Static" MaxLength="5" runat="server" Text='<%#Bind("VALUE")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_VALUE_isNull%>"
                                ControlToValidate="txt_VALUE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_VALUE_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_VALUE_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="60px" Style="text-align: right;" ID="txt_VALUE_Add" CssClass="MandatoryField num5" runat="server" MaxLength="5" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_VALUE_isNull%>"
                                ControlToValidate="txt_VALUE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_VALUE_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_VALUE_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--扣款來源--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_REPAY_SRC%>" SortExpression="REPAY_SRC" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_REPAY_SRC" runat="server" Text='<%#Bind("REPAY_SRC_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList Width="100%" ID="ddl_REPAY_SRC_Add" ClientIDMode="Static" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList Width="100%" ID="ddl_REPAY_SRC_Add" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--其他條件--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_OTHER_COND%>" SortExpression="OTHER_COND" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_OTHER_COND" runat="server" Text='<%#Bind("OTHER_COND_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList Width="100%" ID="ddl_OTHER_COND_Add" ClientIDMode="Static" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList Width="100%" ID="ddl_OTHER_COND_Add" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--是否生效--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_IS_VAILD%>" SortExpression="IS_VAILD" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_IS_VAILD" runat="server" Text='<%#Bind("IS_VAILD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList Width="100%" Style="text-align: center;" ID="ddl_IS_VAILD_Add" ClientIDMode="Static" runat="server">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="N" Value="N"></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList Width="100%" Style="text-align: center;" ID="ddl_IS_VAILD_Add" runat="server" ClientIDMode="Static">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="N" Value="N"></asp:ListItem>
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--功能--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_function%>" HeaderStyle-Width="180px" ItemStyle-Width="180px">
                        <ItemTemplate>
                            <aces:Btn ID="WFB2SC4200Owe" runat="server" Width="80px"
                                CommandName="ToOwe"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sc_WFB2SC4200Owe%>" />
                            <aces:Btn ID="WFB2SC4200Repay" runat="server" Width="80px"
                                CommandName="ToRepay"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sc_WFB2SC4200Repay%>" />
                            <%-- <asp:Button ID="WFB2SC4200Owe" runat="server" Width="80px"
                                            CommandName="ToOwe"
                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text="<%$Resources:Resource,wfb2sc_WFB2SC4200Owe%>" />
                                        <asp:Button ID="WFB2SC4200Repay" runat="server" Width="80px"
                                            CommandName="ToRepay"
                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            Text="<%$Resources:Resource,wfb2sc_WFB2SC4200Repay%>" />--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div style="width: 1020px; overflow: auto;">
                        <table class="grid-view" width="1350px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                            <tr class="header">
                                <td width="30px"></td>
                                <td width="40px">
                                    <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb2sc_RowNumber%>"></asp:Label>
                                </td>
                                <td width="60px">
                                    <asp:Label ID="lblHeaderEMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_ID%>"></asp:Label>
                                </td>
                                <td width="80px">
                                    <asp:Label ID="lblHeaderEMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_NAME2%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lblHeaderDEBIT_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_DEBIT_DT2%>"></asp:Label>
                                </td>
                                <td width="80px">
                                    <asp:Label ID="lblHeaderAMOUNT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_AMOUNT%>"></asp:Label>
                                </td>
                                <td width="120px">
                                    <asp:Label ID="lblHeaderARREARS_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_ARREARS_TYPE2%>"></asp:Label>
                                </td>
                                <td width="80px">
                                    <asp:Label ID="lblHeaderCAL_ORDER" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_CAL_ORDER%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lblHeaderTOTAL_AMT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_TOTAL_AMT%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lblHeaderREPAY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_REPAY_TYPE%>"></asp:Label>
                                </td>
                                <td width="80px">
                                    <asp:Label ID="lblHeaderVALUE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_VALUE%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lblHeaderREPAY_SRC" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_REPAY_SRC%>"></asp:Label>
                                </td>
                                <td width="200px">
                                    <asp:Label ID="lblHeaderOTHER_COND" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_OTHER_COND%>"></asp:Label>
                                </td>
                                <td width="60px">
                                    <asp:Label ID="lblHeaderIS_VAILD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_IS_VAILD%>"></asp:Label>
                                </td>
                                <td></td>
                            </tr>
                            <tr class="normal">
                                <td></td>
                                <td></td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:TextBox Width="40px" Style="text-align: center;" ID="txt_EMP_ID_Add" CssClass="MandatoryField" runat="server" MaxLength="5" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_Add', 'txt_EMP_NAME_Add', 'N', '');" runat="server" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_EMP_ID_isNull%>"
                                            ControlToValidate="txt_EMP_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:TextBox Width="60px" ID="txt_EMP_NAME_Add" runat="server" BorderWidth="0" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center; width: 100%">
                                        <asp:TextBox Width="80px" ID="txt_DEBIT_DT_Add" MaxLength="8" runat="server" CssClass="ymd date MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_DEBIT_DT_isNull%>"
                                            ControlToValidate="txt_DEBIT_DT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_DEBIT_DT_isError2%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_DEBIT_DT_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center; width: 100%">
                                        <asp:TextBox Width="60px" Style="text-align: right;" ID="txt_AMOUNT_Add" CssClass="MandatoryField num7" runat="server" MaxLength="7" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_AMOUNT_isNull%>"
                                            ControlToValidate="txt_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator11" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_AMOUNT_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_AMOUNT_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center; width: 100px">
                                        <asp:DropDownList ID="ddl_ARREARS_TYPE_Add" MaxLength="1" runat="server"></asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center; width: 100%">
                                        <asp:TextBox Width="30px" ID="txt_CAL_ORDER_Add" MaxLength="1" CssClass="num1" runat="server"></asp:TextBox>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center; width: 100%">
                                        <asp:TextBox Width="80px" Style="text-align: right;" ID="txt_TOTAL_AMT_Add" runat="server" CssClass="num7" MaxLength="7" ClientIDMode="Static"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator12" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_TOTAL_AMT_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_TOTAL_AMT_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center; width: 80px">
                                        <asp:DropDownList ID="ddl_REPAY_TYPE_Add" runat="server"></asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center; width: 100%">
                                        <asp:TextBox Width="60px" Style="text-align: right;" ID="txt_VALUE_Add" CssClass="MandatoryField num5" runat="server" MaxLength="5" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_VALUE_isNull%>"
                                            ControlToValidate="txt_VALUE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator13" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_VALUE_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_VALUE_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center; width: 80px">
                                        <asp:DropDownList ID="ddl_REPAY_SRC_Add" runat="server"></asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center; width: 180px">
                                        <asp:DropDownList ID="ddl_OTHER_COND_Add" runat="server"></asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center; width: 40px">
                                        <asp:DropDownList Width="100%" Style="text-align: center;" ID="ddl_IS_VAILD_Add" ClientIDMode="Static" runat="server">
                                            <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px; padding-bottom: 30px;">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('grid1')" ClientIDMode="Static" AutoPostBack="true">
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

            <!--grid2-->
            <table width="1020px" border="0" cellspacing="1" cellpadding="1">
                <tr>
                    <td align="right">
                        <div id="init_grid2">
                            <aces:Btn ID="WFB2SC4200Add2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC4200Add2_Click" />
                            <aces:Btn ID="WFB2SC4200Delete2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction('cb_check2');" OnClick="WFB2SC4200Delete2_Click" Visible="false" />
                            <aces:Btn ID="WFB2SC4200Save2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Save%>" Visible="false" OnClick="WFB2SC4200Save2_Click" OnClientClick="CheckValid();" ValidationGroup="GroupB" />
                            <%--<asp:Button ID="WFB2SC4200Add2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC4200Add2_Click" />
                                        <asp:Button ID="WFB2SC4200Delete2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction('cb_check2');" OnClick="WFB2SC4200Delete2_Click" Visible="false" />
                                        <asp:Button ID="WFB2SC4200Save2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Save%>" Visible="false" OnClick="WFB2SC4200Save2_Click" OnClientClick="CheckValid();" ValidationGroup="GroupB" />--%>
                            <asp:Button ID="btn_cancel2" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_cancel%>" Visible="false" OnClick="btn_cancel2_Click" OnClientClick="return confirm($('#hidwfb2sc_Cancel_ConfirmMessage').val());" />
                        </div>
                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="getData2"
                SelectCountMethod="getCount2" TypeName="CFB2SC4200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting2"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected2">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="HID_EMP_ID" DefaultValue=""
                        Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="HID_DEBIT_DT" DefaultValue=""
                        Name="debit_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated2" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:MySelectAllCheckboxes(this,'cb_check2');" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check2" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--欠薪年月--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_OWE_SALARY_YM%>" SortExpression="SALARY_YM" HeaderStyle-Width="400px" ItemStyle-Width="400px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_OWE_SALARY_YM" runat="server" CssClass="ym" ClientIDMode="Static" Text='<%#Bind("SALARY_YM")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="100px" Style="text-align: left;" ID="txt_OWE_SALARY_YM_Add" ClientIDMode="Static" CssClass="ym MandatoryField" MaxLength="6" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_OWE_SALARY_YM_isNull%>"
                                ControlToValidate="txt_OWE_SALARY_YM_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_OWE_SALARY_YM_isError%>"
                                ControlToValidate="txt_OWE_SALARY_YM_Add" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--欠款金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_OWE_AMOUNT%>" SortExpression="AMOUNT" HeaderStyle-Width="400px" ItemStyle-Width="400px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_OWE_AMOUNT" runat="server" Text='<%#Bind("AMOUNT","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="100px" Style="text-align: right;" ID="txt_OWE_AMOUNT_Add" CssClass="MandatoryField num7" MaxLength="7" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_OWE_AMOUNT_isNull%>"
                                ControlToValidate="txt_OWE_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator5" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_OWE_AMOUNT_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_OWE_AMOUNT_Add" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td width="30px"></td>
                            <td width="40px">
                                <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb2sc_RowNumber%>"></asp:Label>
                            </td>
                            <td width="400px">
                                <asp:Label ID="lblHeaderOWE_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_OWE_SALARY_YM%>"></asp:Label>
                            </td>
                            <td width="400px">
                                <asp:Label ID="lblHeaderOWE_AMOUNT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_OWE_AMOUNT%>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox ID="txt_OWE_SALARY_YM_Add" Style="text-align: left" ClientIDMode="Static" CssClass="ym MandatoryField" MaxLength="6" Width="100px" runat="server" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_OWE_SALARY_YM_isNull%>"
                                        ControlToValidate="txt_OWE_SALARY_YM_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_OWE_SALARY_YM_isError%>"
                                        ControlToValidate="txt_OWE_SALARY_YM_Add" ForeColor="Red" ValidationGroup="GroupB"
                                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox ID="txt_OWE_AMOUNT_Add" Style="text-align: right;" ClientIDMode="Static" CssClass="MandatoryField num7" MaxLength="7" Width="100px" runat="server" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_OWE_AMOUNT_isNull%>"
                                        ControlToValidate="txt_OWE_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator14" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2sc_lb_OWE_AMOUNT_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                        ControlToValidate="txt_OWE_AMOUNT_Add" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                </div>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage2" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow2" runat="server" onchange="javascript:ShowRecord('grid2')" ClientIDMode="Static" AutoPostBack="true">
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

            <!--grid3-->
            <table width="1020px" border="0" cellspacing="1" cellpadding="1">
                <tr>
                    <td align="right">
                        <div id="init_grid3">
                            <%--<asp:Button ID="WFB2SC4200Add3" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC4200Add3_Click" />
                                        <asp:Button ID="WFB2SC4200Delete3" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction('cb_check3');" OnClick="WFB2SC4200Delete3_Click" Visible="False" />
                                        <asp:Button ID="WFB2SC4200Edit3" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Edit%>" OnClientClick="return CheckModeifyAction('cb_check3');" OnClick="WFB2SC4200Edit3_Click" Visible="False" />
                                        <asp:Button ID="WFB2SC4200Save3" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Save%>" Visible="false" OnClick="WFB2SC4200Save3_Click" OnClientClick="CheckValid();" ValidationGroup="GroupC" formnovalidate />--%>
                            <aces:Btn ID="WFB2SC4200Add3" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC4200Add3_Click" />
                            <aces:Btn ID="WFB2SC4200Delete3" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction('cb_check3');" OnClick="WFB2SC4200Delete3_Click" Visible="False" />
                            <aces:Btn ID="WFB2SC4200Edit3" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Edit%>" OnClientClick="return CheckModeifyAction('cb_check3');" OnClick="WFB2SC4200Edit3_Click" Visible="False" />
                            <aces:Btn ID="WFB2SC4200Save3" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Save%>" Visible="false" OnClick="WFB2SC4200Save3_Click" OnClientClick="CheckValid();" ValidationGroup="GroupC" formnovalidate />
                            <asp:Button ID="btn_cancel3" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_cancel%>" Visible="false" OnClick="btn_cancel3_Click" OnClientClick="return confirm($('#hidwfb2sc_Cancel_ConfirmMessage').val());" />
                        </div>
                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods3" runat="server" SelectMethod="getData3"
                SelectCountMethod="getCount3" TypeName="CFB2SC4200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting3"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected3">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="HID_EMP_ID" DefaultValue=""
                        Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="HID_DEBIT_DT" DefaultValue=""
                        Name="debit_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result3" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated3" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:MySelectAllCheckboxes(this,'cb_check3');" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check3" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--發薪日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>" SortExpression="SALARY_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_Repay_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="100px" Style="text-align: left;" ID="txt_Repay_SALARY_DT_Add" CssClass="ymd date MandatoryField" runat="server" MaxLength="10" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_isNull%>"
                                ControlToValidate="txt_Repay_SALARY_DT_Add" ForeColor="Red" ValidationGroup="GroupC" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="qrytest1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_Repay_SALARY_DT_Add" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--發薪類別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>" SortExpression="SALARY_TYPE" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_Repay_SALARY_TYPE" runat="server" Text='<%#Bind("SALARY_TYPE_DESC")%>'></asp:Label>
                            <asp:HiddenField ID="hid_Repay_SALARY_TYPE" runat="server" Value='<%#Bind("SALARY_TYPE")%>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList Width="100px" ID="ddl_Repay_SALARY_TYPE_Add" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--扣薪年月--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_YM%>" SortExpression="REPAY_YM" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_Repay_REPAY_YM" CssClass="ym" ClientIDMode="Static" runat="server" Text='<%#Bind("REPAY_YM")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_Repay_REPAY_YM" CssClass="ym" ClientIDMode="Static" runat="server" Text='<%#Bind("REPAY_YM")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="100px" Style="text-align: left;" ID="txt_Repay_REPAY_YM_Add" CssClass="ym MandatoryField" runat="server" MaxLength="6" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_YM_isNull%>"
                                ControlToValidate="txt_Repay_REPAY_YM_Add" ForeColor="Red" ValidationGroup="GroupC" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator33" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_YM_isError%>"
                                ControlToValidate="txt_Repay_REPAY_YM_Add" ForeColor="Red" ValidationGroup="GroupC"
                                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--應領金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_Repay_ORG_AMT%>" SortExpression="ORG_AMT" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_Repay_ORG_AMT" runat="server" Text='<%#Bind("ORG_AMT","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="100px" Style="text-align: right;" ID="txt_Repay_ORG_AMT_Add" CssClass="MandatoryField num7" ClientIDMode="Static" MaxLength="7" runat="server" Text='<%#Bind("ORG_AMT")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_ORG_AMT_isNull%>"
                                ControlToValidate="txt_Repay_ORG_AMT_Add" ForeColor="Red" ValidationGroup="GroupC" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator7" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_ORG_AMT_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_Repay_ORG_AMT_Add" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="100px" Style="text-align: right;" ID="txt_Repay_ORG_AMT_Add" CssClass="MandatoryField num7" runat="server" MaxLength="7" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_ORG_AMT_isNull%>"
                                ControlToValidate="txt_Repay_ORG_AMT_Add" ForeColor="Red" ValidationGroup="GroupC" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator7" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_ORG_AMT_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_Repay_ORG_AMT_Add" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--扣款金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_AMT%>" SortExpression="REPAY_AMT" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_Repay_REPAY_AMT" runat="server" Text='<%#Bind("REPAY_AMT","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="100px" Style="text-align: right;" ID="txt_Repay_REPAY_AMT_Add" CssClass="MandatoryField num7" ClientIDMode="Static" MaxLength="7" runat="server" Text='<%#Bind("REPAY_AMT")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_AMT_isNull%>"
                                ControlToValidate="txt_Repay_REPAY_AMT_Add" ForeColor="Red" ValidationGroup="GroupC" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator9" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_AMT_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_Repay_REPAY_AMT_Add" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="100px" Style="text-align: right;" ID="txt_Repay_REPAY_AMT_Add" CssClass="MandatoryField num7" runat="server" MaxLength="7" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_AMT_isNull%>"
                                ControlToValidate="txt_Repay_REPAY_AMT_Add" ForeColor="Red" ValidationGroup="GroupC" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator9" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_AMT_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_Repay_REPAY_AMT_Add" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--扣款項目--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_Repay_SALARY_ID%>" SortExpression="SALARY_ID" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_Repay_SALARY_ID" runat="server" Text='<%#Bind("SALARY_ID_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList Width="100px" ID="ddl_Repay_SALARY_ID_Add" ClientIDMode="Static" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList Width="100px" ID="ddl_Repay_SALARY_ID_Add" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--扣款日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_DT%>" SortExpression="REPAY_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_Repay_REPAY_DT" runat="server" Text='<%#Bind("REPAY_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Width="100px" Style="text-align: left;" ID="txt_Repay_REPAY_DT_Add" CssClass="ymd date MandatoryField" ClientIDMode="Static" MaxLength="10" runat="server" Text='<%#Bind("REPAY_DT","{0:yyyy/MM/dd}")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_DT_isNull%>"
                                ControlToValidate="txt_Repay_REPAY_DT_Add" ForeColor="Red" ValidationGroup="GroupC" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="qrytest3" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_DT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_Repay_REPAY_DT_Add" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="100px" Style="text-align: left;" ID="txt_Repay_REPAY_DT_Add" CssClass="ymd date MandatoryField" runat="server" MaxLength="10" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator39" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_DT_isNull%>"
                                ControlToValidate="txt_Repay_REPAY_DT_Add" ForeColor="Red" ValidationGroup="GroupC" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="qrytest4" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_DT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_Repay_REPAY_DT_Add" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td width="30px"></td>
                            <td width="40px">
                                <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb2sc_RowNumber%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="lblHeaderRepay_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="lblHeaderRepay_SALARY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="lblHeaderRepay_REPAY_YM" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_YM%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="lblHeaderRepay_ORG_AMT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_Repay_ORG_AMT%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="lblHeaderRepay_REPAY_AMT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_AMT%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="lblHeaderRepay_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_Repay_SALARY_ID%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="lblHeaderRepay_REPAY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_DT%>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox Width="100px" Style="text-align: center;" ID="txt_Repay_SALARY_DT_Add" CssClass="ymd date MandatoryField" runat="server" MaxLength="10" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_isNull%>"
                                        ControlToValidate="txt_Repay_SALARY_DT_Add" ForeColor="Red" ValidationGroup="GroupC" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="qrytest2" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_Repay_SALARY_DT_Add" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:DropDownList Width="100px" Style="text-align: center;" ID="ddl_Repay_SALARY_TYPE_Add" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox Width="100px" Style="text-align: right;" ID="txt_Repay_REPAY_YM_Add" CssClass="ym MandatoryField" runat="server" MaxLength="6" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_YM_isNull%>"
                                        ControlToValidate="txt_Repay_REPAY_YM_Add" ForeColor="Red" ValidationGroup="GroupC" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator41" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_YM_isError%>"
                                        ControlToValidate="txt_Repay_REPAY_YM_Add" ForeColor="Red" ValidationGroup="GroupC"
                                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </FooterTemplate>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox Width="100px" Style="text-align: right;" ID="txt_Repay_ORG_AMT_Add" CssClass="MandatoryField num7" runat="server" MaxLength="7" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_YM_isNull%>"
                                        ControlToValidate="txt_Repay_ORG_AMT_Add" ForeColor="Red" ValidationGroup="GroupC" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator42" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_ORG_AMT_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                        ControlToValidate="txt_Repay_ORG_AMT_Add" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox Width="100px" Style="text-align: right;" ID="txt_Repay_REPAY_AMT_Add" CssClass="MandatoryField num7" runat="server" MaxLength="7" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_ORG_AMT_isNull%>"
                                        ControlToValidate="txt_Repay_REPAY_AMT_Add" ForeColor="Red" ValidationGroup="GroupC" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator43" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_AMT_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                        ControlToValidate="txt_Repay_REPAY_AMT_Add" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:DropDownList Width="100px" Style="text-align: center;" ID="ddl_Repay_SALARY_ID_Add" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox Width="100px" Style="text-align: right;" ID="txt_Repay_REPAY_DT_Add" CssClass="ymd date MandatoryField" runat="server" MaxLength="10" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator44" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_DT_isNull%>"
                                        ControlToValidate="txt_Repay_REPAY_DT_Add" ForeColor="Red" ValidationGroup="GroupC" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="qrytest5" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2sc_lb_Repay_REPAY_DT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_Repay_REPAY_DT_Add" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                </div>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage3" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow3" runat="server" onchange="javascript:ShowRecord('grid3')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                            <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                            <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount3" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow3" runat="server" ClientIDMode="Static" />

            <asp:HiddenField ID="HID_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_DEBIT_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_AMOUNT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_TOTAL_AMT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Del_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Mod_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Cancel_ConfirmMessage%>" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupC" ShowSummary="false" />
            <!--resource)-->
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
