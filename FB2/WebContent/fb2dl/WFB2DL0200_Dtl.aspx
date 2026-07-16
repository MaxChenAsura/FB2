<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2dl/WFB2DL0200_Dtl.aspx.cs" Inherits="WebContent_fb2dl_WFB2DL0200_Dtl" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $('.date').datepicker({ dateFormat: 'yy/mm/dd' });
            $('#txt_LEAVE_PLAN_DT_Add').mask('9999/99/99');
            $("#txt_EMP_LEAVE_TARGET").mask('999.99');
            $('#txt_LEAVE_PLAN_YEAR').mask('9999');
            gridviewScroll();
            $.unblockUI();
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "350",
                barcolor: "#7F7F7F"
            });
        }
        function CheckORDER_SEQ(source, arguments) {
            var re = /^[\d]+$/;
            if ($("#txt_ORDER_SEQ_Add").val() != "") {                                               //排序有值做檢查
                if (!re.test($("#txt_ORDER_SEQ_Add").val())) {                                       //輸入的為數字
                    arguments.IsValid = false;
                }
                else {
                    if ($("#txt_ORDER_SEQ_Add").val() >= 1 && $("#txt_ORDER_SEQ_Add").val() <= 999)    //輸入的數字在1~999間
                        arguments.IsValid = true;
                    else
                        arguments.IsValid = false;
                }
            }
            else {
                arguments.IsValid = true;
            }
        }
        function CheckSave() {
            if ($("#txt_EMP_LEAVE_TARGET").val() != "") {
                if (!CheckIsNumber($("#txt_EMP_LEAVE_TARGET").val())) {
                    alert('個人年休目標數(時)輸入格式錯誤(999.99)!');
                    return false;
                }
                else {
                    if (parseFloat($("#txt_EMP_LEAVE_TARGET").val(), 10) < parseFloat($("#lb_LEAVED_HOUR_txt").text(), 10)) {
                        alert('已排休數(時)不可大於個人年休目標數(時)!');
                        return false;
                    }
                    else
                        return confirm($('#hidwfb2dl_Save_ConfirmMessage').val());
                }
            }
            else
                return confirm($('#hidwfb2dl_Save_ConfirmMessage').val());
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb2dl_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb2dl_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2dl_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2dl_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function checkEMP_ID() {
            if ($("#txt_LEAVE_PLAN_YEAR").val() != "") {
                var re = /^\d\d\d\d+$/;
                var today = new Date();
                var currentYear = today.getFullYear();
                if (re.test($("#txt_LEAVE_PLAN_YEAR").val())) {
                    if ($("#txt_LEAVE_PLAN_YEAR").val() >= 1911 && $("#txt_LEAVE_PLAN_YEAR").val() <= currentYear + 1) {
                        return true;
                    }
                    else {
                        alert('排休年度輸入錯誤,長度需為四碼(最大至明年)');
                        return false;
                    }
                }
                else {
                    alert('排休年度輸入錯誤,長度需為四碼(最大至明年)');
                    return false;
                }
            }
            else {
                alert($("#hidwfb2dl_EMP_ID_beforeMesssage").val());
                return false;
            }
        }
        function backConfirm() {
            if ($('#hid_state').val() == "detail") {
                return true;
            }
            else {
                return confirm($('#hidwfb2dl_cancel_ConfirmMessage').val())
            }
        }
        function addCheck() {
            if ($("#txt_LEAVE_PLAN_YEAR").val() == "" || $("#txt_EMP_ID").val() == "") {
                alert($("#hidwfb2dl_addDetail_beforeMesssage").val());
                return false;
            }
            else
                return true;
        }
        function openEMP_ID() {
            if (checkEMP_ID()) {
                OpenEmpSearch('txt_EMP_ID', 'txt_qry_EMP_NAME', 'N','Y');
                //__doPostBack('getEMP', '');
                //return true;
            }
            else
                return false;
        }
        function returnEMPValueToPage(eid, ename, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            //alert(returnValue);
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                window.parent.$('#' + eid).val(obj.EMP_ID.trim());
                window.parent.$('#' + ename).val(obj.EMP_NAME.trim());
                __doPostBack('getEMP', '');
            }
        }
        function checkconfirm(type) {
            if (confirm(type)) {
                __doPostBack('doSave', '');
            }
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
                                <col width="15%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="25%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--排休年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_PLAN_YEAR" runat="server" Text="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_YEAR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LEAVE_PLAN_YEAR" CssClass="MandatoryField" MaxLength="4" Width="80" runat="server" 
                                            ClientIDMode="Static" OnTextChanged="txt_LEAVE_PLAN_YEAR_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_YEAR_isNull%>"
                                            ControlToValidate="txt_LEAVE_PLAN_YEAR" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <%--工號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dl_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" CssClass="MandatoryField" MaxLength="5" Width="80"
                                            OnTextChanged="txt_EMP_ID_TextChanged" onclick="return checkEMP_ID();" AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="return openEMP_ID();" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_EMP_ID_importNull%>"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <%--姓名--%>
                                        <asp:Label ID="lb_EMP_NAME" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--部門代號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2dl_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_DEPT_NO_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                        <%--部門名稱--%>
                                        <asp:Label ID="lb_DEPT_NAME" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--公司年度目標數(日)--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_PLAN_TARGET" runat="server" Text="<%$Resources:Resource,wfb2dl_lb_COMPANY_PLAN_TARGET%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:Label ID="lb_COMPANY_PLAN_TARGET_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--個人年休目標數(時)--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_LEAVE_TARGET" runat="server" Text="<%$Resources:Resource,wfb2dl_lb_EMP_LEAVE_TARGET%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_LEAVE_TARGET" runat="server" MaxLength="8" ClientIDMode="Static"></asp:TextBox>

                                    </td>
                                    <%--IFLOW單號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2dl_IFLOW_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:Label ID="lb_IFLOW_NO_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>

                                <tr>
                                    <%--已排休數(時)--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVED_HOUR" runat="server" Text="<%$Resources:Resource,wfb2dl_lb_SUM_LEAVE_PLAN_HRS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_LEAVED_HOUR_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <td align="right" class="Body_label" colspan="4">
                                        <div id="init_grid">
                                            <aces:Btn ID="WFB2DL0200Add" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Add%>" OnClick="WFB2DL0200Add_Click" OnClientClick="return addCheck();" />
                                            <aces:Btn ID="WFB2DL0200Delete" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DL0200Delete_Click" Visible="false" />
                                            <aces:Btn ID="WFB2DL0200Edit" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2DL0200Edit_Click" Visible="false" />
                                            <aces:Btn ID="WFB2DL0200OK" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Ok1%>" Visible="false" OnClick="WFB2DL0200OK_Click" OnClientClick="CheckValid();" ValidationGroup="GroupB" />
                                            <%-- <asp:Button ID="WFB2DL0200Add" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Add%>" OnClick="WFB2DL0200Add_Click" OnClientClick="return addCheck();" />
                                            <asp:Button ID="WFB2DL0200Delete" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DL0200Delete_Click" Visible="false" />
                                            <asp:Button ID="WFB2DL0200Edit" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2DL0200Edit_Click" Visible="false" />
                                            <asp:Button ID="WFB2DL0200OK" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Ok1%>" Visible="false" OnClick="WFB2DL0200OK_Click" OnClientClick="CheckValid();" ValidationGroup="GroupB" />--%>
                                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2dl_btn_back%>" Visible="false" OnClick="btn_cancel_Click" OnClientClick="return confirm($('#hidwfb2dl_cancel_ConfirmMessage').val());" />
                                        </div>
                                    </td>
                                </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="40px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2dl_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_DT%>">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_LEAVE_PLAN_DT" runat="server" Text='<%#Bind("LEAVE_PLAN_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                         <%--排休日期--%>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_LEAVE_PLAN_DT_Add" runat="server" ClientIDMode="Static" MaxLength="8" CssClass="date MandatoryField"
                                     Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_DT_isNull%>"
                                    ControlToValidate="txt_LEAVE_PLAN_DT_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_DT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_LEAVE_PLAN_DT_Add" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--排休型態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_CD%>">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_LEAVE_PLAN_CD" runat="server" Text='<%#Bind("LEAVE_PLAN_CD")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_LEAVE_PLAN_CD_Add" runat="server" MaxLength="1" ClientIDMode="Static"
                                    OnSelectedIndexChanged="ddl_LEAVE_PLAN_CD_Add_SelectedIndexChanged" AutoPostBack="true" CssClass="MandatoryField" Width="100px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_CD_isNull%>"
                                    ControlToValidate="ddl_LEAVE_PLAN_CD_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                </asp:RequiredFieldValidator>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_LEAVE_PLAN_CD_Add" runat="server" ClientIDMode="Static" MaxLength="1" CssClass="MandatoryField"
                                     OnSelectedIndexChanged="ddl_LEAVE_PLAN_CD_Add_SelectedIndexChanged" AutoPostBack="true" Width="100px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_CD_isNull%>"
                                    ControlToValidate="ddl_LEAVE_PLAN_CD_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--排休時數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_HRS%>">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_LEAVE_PLAN_HRS_Add" runat="server" Text='<%#Bind("LEAVE_PLAN_HRS")%>' ClientIDMode="Static"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_LEAVE_PLAN_HRS_Add" runat="server" ClientIDMode="Static"></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td width="30px"></td>
                            <td width="40px">
                                <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb2dl_RowNumber%>"></asp:Label>
                            </td>
                            <%--排休日期--%>
                            <td width="300px">
                                <asp:Label ID="lblLEAVE_PLAN_DT" runat="server" Text="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_DT%>"></asp:Label>
                            </td>
                            <%--排休型態--%>
                            <td width="300px">
                                <asp:Label ID="lblLEAVE_PLAN_CD" runat="server" Text="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_CD%>"></asp:Label>
                            </td>
                            <%--排休時數--%>
                            <td width="300px">
                                <asp:Label ID="lblLEAVE_PLAN_HRS" runat="server" Text="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_HRS%>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_LEAVE_PLAN_DT_Add" runat="server" ClientIDMode="Static" MaxLength="8" CssClass="date MandatoryField"
                                        Width="100px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_DT_isNull%>"
                                        ControlToValidate="txt_LEAVE_PLAN_DT_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_DT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_LEAVE_PLAN_DT_Add" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:DropDownList ID="ddl_LEAVE_PLAN_CD_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField"
                                        OnSelectedIndexChanged="ddl_LEAVE_PLAN_CD_Add_SelectedIndexChanged" AutoPostBack="true" Width="100px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_CD_isNull%>"
                                        ControlToValidate="ddl_LEAVE_PLAN_CD_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:Label ID="lb_LEAVE_PLAN_HRS_Add" runat="server" ClientIDMode="Static"></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server">
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10000_Rows%>" Value="" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                <tr>
                    <td style="height: 10px;"></td>
                </tr>
                <tr>
                    <td align="right" class="Body_label">
                        <aces:Btn ID="WFB2DL0200Save" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Save%>" Visible="false" OnClick="WFB2DL0200Save_Click" OnClientClick="return CheckSave();" />
                        <%--<asp:Button ID="WFB2DL0200Save" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Save%>" Visible="false" OnClick="WFB2DL0200Save_Click" OnClientClick="return CheckSave();" />--%>
                        <asp:Button ID="btn_back" runat="server" OnClientClick="return backConfirm();" OnClick="btn_back_Click" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hid_ws_cd" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_level_cd" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_calendar_cd" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_pjob_flow_level" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_available_value" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_IS_Continue_three" runat="server" ClientIDMode="Static" />

            <asp:HiddenField ID="hid_state" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qdatakey" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_deleteKeyList" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2dl_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb2dl_Mod_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb2dl_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2dl_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2dl_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2dl_cancel_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_EMP_ID_beforeMesssage" Value="<%$Resources:Resource,wfb2dl_EMP_ID_beforeMesssage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_addDetail_beforeMesssage" Value="<%$Resources:Resource, wfb2dl_addDetail_beforeMesssage%>" />
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DL0200Edit" />
            <%--<asp:PostBackTrigger ControlID="ddl_LEAVE_PLAN_CD_Add" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


