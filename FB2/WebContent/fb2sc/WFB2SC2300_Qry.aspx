<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2300_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2300_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            // getYearMonth()
            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".ym").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $('.ymd').mask('9999/99/99');
            $('#txt_LEAVE_DT_S').mask('9999/99/99');
            $('#txt_LEAVE_DT_E').mask('9999/99/99');
            $(".number").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });
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
            if (obj == "grid1")
                $("#HID_PageRow").val($("#ddlPerPageRow").val());
            else if (obj == "grid2")
                $("#HID_PageRow2").val($("#ddlPerPageRow2").val());
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
                barcolor: "#7F7F7F",
                headerrowcount: 1,
                freezesize: 5

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check2");
        }
        function getYearMonth() {
            var datetime = new Date();
            var year = datetime.getFullYear().toString();
            var month = datetime.getMonth() + 1;
            if (month < 10)
                month = "0" + month;
            $("#txt_DATA_YM_search").val(year + month);
        }
        function CheckDelAction(checkboxID) {
            if (LookUpCheckboxs(checkboxID) > 0) {
                if ($('#txt_REMARK_DESC').val().trim().length > 0 && $('#txt_REMARK_DESC').val().trim().length < 300) { //檢查備註有值
                    if (confirm($('#hidwfb2sc_Del_ConfirmMessage').val()))
                        BlockUI();
                    else
                        return false;
                }
                else {
                    alert($('#hidwfb2sc_REMARK_isNull').val());
                    return false;
                }
            }
            else {
                alert($('#hidwfb2sc_Del_NotChoiceMessage').val());
                return false;
            }
        }
        function CheckDelAction2(checkboxID) {
            if (LookUpCheckboxs(checkboxID) > 0) {
                if ($('#txt_REMARK_DESC').val().trim().length > 0 && $('#txt_REMARK_DESC').val().trim().length < 300) { //檢查備註有值
                    if (confirm($('#hidwfb2sc_Del_ConfirmMessage').val()))
                        BlockUI();
                    else
                        return false;
                }
                else {
                    alert($('#hidwfb2sc_REMARK_isNull').val());
                    return false;
                }
            }
            else {
                alert($('#hidwfb2sc_Del_NotChoiceMessage').val());
                return false;
            }
        }
        function CheckExecuteAction(checkboxID) {
            if (LookUpCheckboxs(checkboxID) > 0) {
                if ($('#txt_REMARK_DESC').val().trim().length > 0 && $('#txt_REMARK_DESC').val().trim().length < 300) //檢查備註有值
                {
                    if (confirm($('#hidwfb2sc_Execute_ConfirmMessage').val()))
                        BlockUI();
                    else
                        return false;
                }
                else {
                    alert($('#hidwfb2sc_REMARK_isNull').val());
                    return false;
                }
            }
            else {
                alert($('#hidwfb2sc_Execute_NotChoiceMessage').val());
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
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf(checkboxID) != -1 && ItemCheckBoxs[i].id.indexOf("_freezeitem") == -1 && ItemCheckBoxs[i].id.indexOf("_freezeheader") == -1) {
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
            $('#txt_SALARY_DT_search').val($('#hid_salary_dt_search').val());
            $('#txt_DATA_YM_search').val($('#hid_salary_ym_search').val());
            $('#txt_PAY_KIND').val("");
            $('#txt_SALARY_NAME_search').val("");
            $('#txt_EMP_NAME_search').val("");
            $('#txt_EMP_ID_search').val("");
            $('#ddl_SALARY_TYPE_search').val("");
            $('#ddl_COMPANY_CD_search').val("");
            $('#ddl_CFN_PAY_search').val("");
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
        function paykindCheck(value) {
            __doPostBack('question', '');
        }
        //function checkEMP_NAMEandID(source, arguments) {
        //    if ($("#txt_EMP_ID_search").val() == "" && $("#txt_EMP_NAME_search").val() == "")
        //        arguments.IsValid = false;
        //    else
        //        arguments.IsValid = true;
        //}
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
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="30%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--發薪類別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_TYPE_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_TYPE_search" runat="server" class="MandatoryField" ClientIDMode="Static"
                                            OnSelectedIndexChanged="ddl_SALARY_TYPE_search_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_ddl_SALARY_TYPE%>"
                                            ControlToValidate="ddl_SALARY_TYPE_search" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <%--發薪日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_DT_search" runat="server" MaxLength="8" CssClass="ymd date MandatoryField" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator0" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_isNull%>"
                                            ControlToValidate="txt_SALARY_DT_search" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_DT_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                    <%--資料年月--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YM_search" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_DATA_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DATA_YM_search" runat="server" MaxLength="6" CssClass="ym" ClientIDMode="Static" Width="64px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_DATA_YM_isError%>"
                                            ControlToValidate="txt_DATA_YM_search" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--聘用單位--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_COMPANY_CD2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_COMPANY_CD_search" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <%--發放項目--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_PAY_KIND" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static" onblur="paykindCheck(this.value);"></asp:TextBox>
                                        <input id="bt_PAY_KIND_SEARCH" type="button" value="..." onclick="OpenSearch('Salary9999_Search.aspx', 'txt_PAY_KIND', 'txt_SALARY_NAME_search', '', '');" />
                                    </td>
                                    <%--項目名稱--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_NAME_search" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_NAME_search" runat="server" MaxLength="20" ClientIDMode="Static" Width="100px" ReadOnly="true" Enabled="false"></asp:TextBox>
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
                                    <%--確定發薪--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CFN_PAY_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_CFN_PAY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_CFN_PAY_search" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Y"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--薪資項目--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="2">
                                        <asp:TextBox ID="txt_SALARY_ID" runat="server" ClientIDMode="Static" CssClass="" MaxLength="4" Width="80px"
                                            OnTextChanged="txt_SALARY_ID_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <input id="bt_SALARY_ID" type="button" value="..." onclick="OpenSearch('SalaryItem_Search.aspx', 'txt_SALARY_ID', 'txt_SALARY_NAME', '', '');" runat="server" />
                                        <asp:TextBox ID="txt_SALARY_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="100px"></asp:TextBox>
                                    </td>
                                    <td align="right" class="Body_label" colspan="3">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SC2300Search1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Search%>" OnClick="WFB2SC2300Search1_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                            <%--<asp:Button ID="WFB2SC2300Search1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Search%>" OnClick="WFB2SC2300Search1_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_clear%>" OnClientClick="return ClearAll();" />
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
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <tr>
                                <th align="left" style="width: 10%;">
                                    <asp:Label ID="lb_REMARK_DESC" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_REMARK%>" Visible="False"></asp:Label>
                                </th>
                                <td align="left" style="width: 45%;">
                                    <asp:TextBox ID="txt_REMARK_DESC" Height="40px" Width="500px" TextMode="MultiLine" runat="server" ClientIDMode="Static" Visible="False"></asp:TextBox>
                                </td>
                                <td align="right" class="Body_label" style="width: 45%;">
                                    <div id="init_grid">

                                        <aces:Btn ID="WFB2SC2300Delete1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction('cb_check1');" OnClick="WFB2SC2300Delete1_Click" Visible="False" />
                                        <aces:Btn ID="WFB2SC2300Execute1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2300Execute1%>" OnClientClick="return CheckExecuteAction('cb_check1');" OnClick="WFB2SC2300Execute1_Click" Visible="False" />
                                        <aces:Btn ID="WFB2SC2300Execute2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2300Execute2%>" OnClientClick="return CheckExecuteAction('cb_check1');" OnClick="WFB2SC2300Execute2_Click" Visible="False" />
                                        <aces:Btn ID="WFB2SC2300Execute3" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2300Execute3%>" OnClientClick="return CheckExecuteAction('cb_check1');" OnClick="WFB2SC2300Execute3_Click" Visible="False" />
                                        <aces:Btn ID="WFB2SC2300Execute4" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2300Execute4%>" OnClientClick="return CheckExecuteAction('cb_check1');" OnClick="WFB2SC2300Execute4_Click" Visible="False" />


                                        <%--                                        
                                            <asp:Button ID="WFB2SC2300Delete1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction('cb_check1');" OnClick="WFB2SC2300Delete1_Click" Visible="False" />
                                        <asp:Button ID="WFB2SC2300Execute1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2300Execute1%>" OnClientClick="return CheckExecuteAction('cb_check1');" OnClick="WFB2SC2300Execute1_Click" Visible="False" />
                                        <asp:Button ID="WFB2SC2300Execute2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2300Execute2%>" OnClientClick="return CheckExecuteAction('cb_check1');" OnClick="WFB2SC2300Execute2_Click" Visible="False" />
                                        <asp:Button ID="WFB2SC2300Execute3" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2300Execute3%>" OnClientClick="return CheckExecuteAction('cb_check1');" OnClick="WFB2SC2300Execute3_Click" Visible="False" />
                                        <asp:Button ID="WFB2SC2300Execute4" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2300Execute4%>" OnClientClick="return CheckExecuteAction('cb_check1');" OnClick="WFB2SC2300Execute4_Click" Visible="False" />
                                        --%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <!--grid1-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SC2300BO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_SALARY_DT_search" DefaultValue=""
                        Name="salary_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_SALARY_TYPE_search" DefaultValue=""
                        Name="salary_type" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_DATA_YM_search" DefaultValue=""
                        Name="data_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_COMPANY_CD_search" DefaultValue=""
                        Name="company_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_PAY_KIND" DefaultValue=""
                        Name="pay_kind" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_SALARY_NAME_search" DefaultValue=""
                        Name="salary_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID_search" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME_search" DefaultValue=""
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_CFN_PAY_search" DefaultValue=""
                        Name="cfn_pay" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                     <asp:ControlParameter ControlID="txt_SALARY_ID" DefaultValue=""
                        Name="salary_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1000px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:MySelectAllCheckboxes(this,'cb_check1');" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check1" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發薪日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_SALARY_DT%>" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: left;" ID="lb_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                            <asp:HiddenField ID="hid_CFN_PAY" runat="server" Value='<%#Bind("CFN_PAY")%>' />
                            <asp:HiddenField ID="hid_DATA_YM" runat="server" Value='<%#Bind("DATA_YM")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--發放項目--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>" SortExpression="PAY_KIND" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label Width="120px" Style="text-align: left;" ID="lb_PAY_KIND" runat="server" Text='<%#Bind("PAY_KIND_DESC")%>'></asp:Label>
                            <asp:HiddenField ID="hid_SALARY_TYPE" runat="server" Value='<%#Bind("SALARY_TYPE")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_EMP_NAME2%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--聘用單位--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_COMPANY_CD2%>" SortExpression="COMPANY_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_COMPANY_CD" runat="server" Text='<%#Bind("COMPANY_SNAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--員工區分--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_EMP_CD%>" SortExpression="EMP_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: left;" ID="lb_EMP_CD" runat="server" Text='<%#Bind("EMP_CD_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--入社日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_JOIN_DT%>" SortExpression="JOIN_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: left;" ID="lb_JOIN_DT" runat="server" Text='<%#Bind("JOIN_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--離社日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_LEAVE_DT%>" SortExpression="LEAVE_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: left;" ID="lb_LEAVE_DT" runat="server" Text='<%#Bind("LEAVE_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--總金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_AMOUNT%>" SortExpression="AMOUNT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: right;" ID="lb_AMOUNT" runat="server" Text='<%#Bind("AMOUNT","{0:n0}")%>'></asp:Label>
                            <asp:HiddenField ID="hid_PAY_ID" runat="server" ClientIDMode="Static" Value='<%#Bind("PAY_ID")%>' />
                            <asp:HiddenField ID="hid_PAY_KIND" runat="server" ClientIDMode="Static" Value='<%#Bind("PAY_KIND")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--功能--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_function%>" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <aces:Btn ID="WFB2SC2300Detail" runat="server" Width="80px"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Detail%>" />
                            <%-- <asp:Button ID="WFB2SC2300Detail" runat="server" Width="80px"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Detail%>" />--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
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
                <tr>
                    <td style="height: 10px;"></td>
                </tr>
            </table>


            <!--grid2-->
            <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                <tr>
                    <td align="right" class="Body_label">
                        <div id="init_grid2">
                            <aces:Btn ID="WFB2SC2300Add2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC2300Add2_Click" />
                            <aces:Btn ID="WFB2SC2300Delete2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction2('cb_check2');" OnClick="WFB2SC2300Delete2_Click" Visible="False" />
                            <aces:Btn ID="WFB2SC2300Edit2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Edit%>" OnClientClick="return CheckModeifyAction('cb_check2');" OnClick="WFB2SC2300Edit2_Click" Visible="False" />
                            <%-- <asp:Button ID="WFB2SC2300Add2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC2300Add2_Click" />
                            <asp:Button ID="WFB2SC2300Delete2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction2('cb_check2');" OnClick="WFB2SC2300Delete2_Click" Visible="False" />--%>
                            <%--<asp:Button ID="WFB2SC2300Edit2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Edit%>" OnClientClick="return CheckModeifyAction('cb_check2');" OnClick="WFB2SC2300Edit2_Click" Visible="False" />--%>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="getData2"
                SelectCountMethod="getCount2" TypeName="CFB2SC2300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting2"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected2">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="HID_SALARY_DT" DefaultValue=""
                        Name="salary_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="HID_SALARY_TYPE" DefaultValue=""
                        Name="salary_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="HID_EMP_ID" DefaultValue=""
                        Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="HID_PAY_KIND" DefaultValue=""
                        Name="pay_kind" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result2" runat="server" AllowPaging="true" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated2" Width="1440px"
                OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound2">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check2" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                            <asp:HiddenField ID="hid_DATA_YM" runat="server" Value='<%#Bind("DATA_YM")%>' />
                            <asp:HiddenField ID="hid_SALARY_ID" runat="server" Value='<%#Bind("SALARY_ID")%>' />
                            <asp:HiddenField ID="hid_PAY_KIND" runat="server" Value='<%#Bind("PAY_KIND")%>' />
                            <asp:HiddenField ID="hid_PROCESS_STATUS" runat="server" Value='<%#Bind("PROCESS_STATUS")%>' />
                            <asp:HiddenField ID="hid_SEQ_NO" runat="server" Value='<%#Bind("SEQ_NO")%>' />
                            <asp:HiddenField ID="hid_CHG_AMT_A" runat="server" Value='<%#Bind("CHG_AMT_A")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>"  HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <%--薪資項目--%>
                    <asp:BoundField DataField="SALARY_ID_NAME" HeaderText="<%$Resources:Resource,wfb2sc_SALARY_ID%>" SortExpression="SALARY_ID" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="140px" ItemStyle-Width="140px" />
                    <%--金額--%>
                    <asp:BoundField DataField="CHG_AMT_A" HeaderText="<%$Resources:Resource,wfb2sc_lb_AMOUNT2%>" DataFormatString="{0:n0}" SortExpression="CHG_AMT_A" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <%--來源--%>
                    <asp:BoundField DataField="DATA_SRC_DESC" HeaderText="<%$Resources:Resource,wfb2sc_lb_DATA_SRC%>" SortExpression="DATA_SRC" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <%--加減項--%>
                    <asp:BoundField DataField="IS_PLUS" HeaderText="<%$Resources:Resource,wfb2sc_IS_PLUS%>" SortExpression="IS_PLUS" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="60px" ItemStyle-Width="60px" />
                    <%--應稅--%>
                    <asp:BoundField DataField="IS_TAX" HeaderText="<%$Resources:Resource,wfb2sc_IS_TAX_Y%>" SortExpression="IS_TAX" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <%--確定發薪--%>
                    <asp:BoundField DataField="CFN_PAY" HeaderText="<%$Resources:Resource,wfb2sc_lb_CFN_PAY%>" SortExpression="CFN_PAY" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="70px" ItemStyle-Width="70px" />
                    <%--處理狀態--%>
                    <asp:BoundField DataField="PROCESS_STATUS_DESC" HeaderText="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS2%>" SortExpression="PROCESS_STATUS_DESC" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="70px" ItemStyle-Width="70px" />
                    <%--異動狀態--%>
                    <asp:BoundField DataField="CHG_STATUS_DESC" HeaderText="<%$Resources:Resource,wfb2sc_lb_CHG_STATUS%>" SortExpression="CHG_STATUS_DESC" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                    <%--異動前金額--%>
                    <asp:BoundField DataField="CHG_AMT_B" HeaderText="<%$Resources:Resource,wfb2sc_lb_CHG_AMT_B%>" DataFormatString="{0:n0}" SortExpression="CHG_AMT_B" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="90px" ItemStyle-Width="90px" />
                    <%--異動原因--%>
                    <asp:BoundField DataField="REMARK" HeaderText="<%$Resources:Resource,wfb2sc_lb_REMARK2%>" SortExpression="REMARK" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="160px" ItemStyle-Width="160px" />
                    <%--核可人員--%>
                    <asp:BoundField DataField="APPROVE_BY_NAME" HeaderText="<%$Resources:Resource,wfb2sc_APPROVE_BY%>" SortExpression="APPROVE_BY" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                    <%--核可日期--%>
                    <asp:BoundField DataField="APPROVE_DT" HeaderText="<%$Resources:Resource,wfb2sc_APPROVE_DT%>" SortExpression="APPROVE_DT" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-Width="100px" DataFormatString="{0:yyyy/MM/dd}" />
                    <%--主管核定備註--%>
                    <asp:BoundField DataField="APP_REMARK" HeaderText="<%$Resources:Resource,wfb2sc_APP_REMARK%>" SortExpression="APP_REMARK" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="160px" ItemStyle-Width="160px" />
                    <%--發放項目--%>
                    <asp:BoundField DataField="PAY_KIND_DESC" HeaderText="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>" SortExpression="PAY_KIND" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" ItemStyle-Width="120px" />
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage2" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:DropDownList ID="ddlPerPageRow2" runat="server">
                            <asp:ListItem Text="每頁10000筆" Value="" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lb_TotalCount2" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>


            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
            <%--記住預設發新日期、年月--%>
            <asp:HiddenField ID="hid_salary_dt_search" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_salary_ym_search" runat="server" ClientIDMode="Static" />

            <asp:HiddenField ID="HID_SALARY_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_SALARY_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PAY_KIND" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_IsClose" runat="server" ClientIDMode="Static" />

            <asp:HiddenField ID="HID_length" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Del_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Mod_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Cancel_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_REMARK_isNull" Value="<%$Resources:Resource,wfb2sc_REMARK_isNull%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Execute_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Execute_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Execute_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Execute_NotChoiceMessage%>" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <!--resource)-->
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
