<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2SB/WFB2SB2100_Qry.aspx.cs" Inherits="WebContent_fb2sb_WFB2SB2100_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>

    <script type="text/javascript">



        jQuery(document).ready(function () {

            iniForm();
        });


        function iniForm() {
            $('#txt_SALARY_NAME').attr("readonly", true);
            $("#txt_YEAR_MONTH_Add").datepicker({ dateFormat: 'yymm' });
            $("#txt_START_DT_S").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_START_DT_E").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $('.empid').mask('99999');
            $(".number").mask('9999/99/99');
            gridviewScroll();
            $.unblockUI();
            $('#txt_START_DT_S,#txt_START_DT_E').change(function () {
                if ($('#txt_START_DT_S').val() != '' && $('#txt_START_DT_E').val() != '') {
                    var b = new Date($('#txt_START_DT_S').val());
                    var a = new Date($('#txt_START_DT_E').val());
                    if (b > a) {
                        $(this).val("");
                        alert("起日不可大於迄日");
                    }
                }
            });

            $('#txt_SALARY_ID').change(function () {
                if ($('#txt_SALARY_ID').val().length <= 4) {
                    //ajax 取得薪資
                    $.ajax({
                        url: "../commgeo/WFB2GetSalaryData.ashx",
                        data: {
                            SALARY_ID: $('#txt_SALARY_ID').val()
                            , EMP_ID: $('#Hid_EMP_ID').val()

                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                alert(JData.errMsg);

                                $('#txt_SALARY_ID').val("");
                                $('#txt_SALARY_NAME').val("");
                            }
                            else {
                                $('#txt_SALARY_ID').val(JData.SALARY_ID);
                                $('#txt_SALARY_NAME').val(JData.SALARY_NAME);

                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                }
            });

            $('#txt_EMP_ID').change(function () {
                if ($('#txt_EMP_ID').val().length == 5) {
                    //ajax 取得員工基本資料
                    doEmpAjax2($(this));
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
                freezesize: 3
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }
        function ClearAll() {
            $('#txt_SALARY_ID').val("");
            $('#txt_SALARY_NAME').val("");
            $('#ddl_EMP_CD').val(-1);
            $('#txt_EMP_ID').val("");
            $('#txt_EMP_NAME').val("");
            $('#txt_START_DT_S').val("");
            $('#txt_START_DT_E').val("");
        }

        function CheckMODE_ID(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if (!re.test($("#txt_MODE_ID_Add").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }


        function CheckModeifyAction() {
            $("#hidwfb200_mode").val("mod");
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidWFB2IB_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckAddAction() {
            $("#hidwfb200_mode").val("add");
        }
        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1) {
                return confirm($('#hidwfb299_Del_ConfirmMessage').val());
            }
            else {
                alert($('#hidWFB2IB_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;


            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }
        function CheckDtlAction() {
            var processed = true;
            if (LookUpCheckboxs() == 1) {
                processed = true;
            } else {
                alert($("#hidwfb2_Dtl_NotChoiceMessage").val());
                processed = false;
                return processed;
            }

            if (confirm($("#hidwfb2_Del_ConfirmMessage").val())) {
                processed = true;
                $.unblockUI();
            } else {
                processed = false;
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hidWFB2IB_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hidWFB2IB_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hidWFB2IB_Save_ConfirmMessage').val());
        }

        function CheckdeleteAction() {

            if (LookUpCheckboxs() > 0) {
                for (var i = 0; i < ItemCheckBoxs.length; i++) {
                    if (choietr[i] != null) {
                        var ck = choietr[i] - 2;
                        if (ck >= 0 && $("[id=hid_PROCESS_STATUS]")[ck].value != "Y") {
                            return CheckDtlAction();
                        }
                    }
                }
            }
            else {
                alert($("#hidwfb299_Mod_NotChoiceMessage").val());

                return false;
            }
        }
        function CancelConfirm() {
            return confirm($("#hidfb2CancelConfirm").val());
        }
        var choietr = [];
        var ItemCheckBoxs = null;
        //檢查勾了幾個checkbox
        function LookUpCheckboxs() {
            //choietr = null;
            ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                choietr[i] = null;
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("_freezeitem") == -1 && ItemCheckBoxs[i].id.indexOf("_freezeheader") == -1) {
                    if (choietr[i] == null)
                        choietr[i] = i;
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function openSalaryID() {
            var salaryId = $("#txt_SALARY_ID").val();
            OpenSearch('SalaryItem_Search.aspx', 'txt_SALARY_ID', 'txt_SALARY_NAME', "SALARY_ID=" + salaryId + "&isPermissions=Y");
        }
        function doEmpAjax2(obj) {
            //ajax 取得員工基本資料
            if ($("#txt_EMP_ID").val().length == 5) {
                $.ajax({
                    url: "../commgeo/WFB2GetEmpData.ashx",
                    data: {
                        EMP_ID: obj.val()
                    },
                    type: "GET",
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "") {
                            $('#txt_EMP_ID').val("");
                            $('#txt_EMP_NAME').val("");
                            alert(JData.errMsg);
                        } else {
                            $('#txt_EMP_NAME').val(JData.EMP_NAME);
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
                                <col width="13%" />
                                <col width="25%" />
                                <col width="13%" />
                                <col width="49%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_SALARY_ID%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_ID" runat="server" MaxLength="4" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="Button8" type="button" value="..." onclick="openSalaryID();" />
                                        <asp:TextBox ID="txt_SALARY_NAME" runat="server" MaxLength="15" Columns="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        <asp:HiddenField ID="Hid_EMP_ID" runat="server" ClientIDMode="Static" />

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sb_lbl_EMP_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_EMP_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sb_lbl_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="empid"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', '', '');" />
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sb_lbl_EMP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="5" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="加扣款期間"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_START_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        ~
                                <asp:TextBox ID="txt_START_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_START_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="qry3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_START_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_START_DT_S"
                                            ControlToValidate="txt_START_DT_E" ErrorMessage="加扣款期間起日不可大於迄日" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>

                                    <td align="left" class=""></td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SB2100Search" runat="server" Text="<%$Resources:Resource,btn_Search%>" OnClick="WFB2SB2100Search_Click" ValidationGroup="GroupA" />
                                            <%--<asp:Button ID="WFB2SB2100Search" runat="server" Text="<%$Resources:Resource,btn_Search%>" OnClick="WFB2SB2100Search_Click" ValidationGroup="GroupA" />--%>
                                            <input id="WFB2SB2100Clear" type="button" value="<%$Resources:Resource,btn_clear%>" runat="server" onclick="ClearAll();" />
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
                            <aces:Btn ID="WFB2SB2100Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2SB2100Add_Click" />
                            <aces:Btn ID="WFB2SB2100Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClick="WFB2SB2100Delete_Click" OnClientClick="return CheckdeleteAction();" Visible="False" />
                            <aces:Btn ID="WFB2SB2100Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClick="WFB2SB2100Edit_Click" Visible="False" />
                            <%-- <asp:Button ID="WFB2SB2100Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2SB2100Add_Click" />
                            <asp:Button ID="WFB2SB2100Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClick="WFB2SB2100Delete_Click" OnClientClick="return CheckdeleteAction();" Visible="False" />
                            <asp:Button ID="WFB2SB2100Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClick="WFB2SB2100Edit_Click" Visible="False" />--%>
                        </div>

                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="Cfb2sb2100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="txt_EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_EMP_CD"
                        Name="ddl_EMP_CD" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME"
                        Name="txt_EMP_NAME" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SALARY_ID"
                        Name="txt_SALARY_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SALARY_NAME"
                        Name="txt_SALARY_NAME" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_START_DT_S"
                        Name="txt_START_DT_S" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_START_DT_E"
                        Name="txt_START_DT_E" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="2100px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField>
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
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2ib_RowNumber%>" HeaderStyle-Width="40px">
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

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_EMP_ID%>" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                                <asp:HiddenField ID="hid_SEQ_NO" runat="server" Value='<%#Bind("SEQ_NO")%>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_EMP_NAME%>" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                            </div>
                        </ItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_EMP_CD%>" SortExpression="DESC1">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_DESC1" runat="server" Text='<%# Convert.ToString(Eval("DESC1"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_SALARY_ID%>" SortExpression="SALARY_NAME">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_SALARY_NAME" runat="server" Text='<%#Bind("SALARY_NAME")%>'></asp:Label>
                                <asp:Label ID="lbl_SALARY_ID" runat="server" Text='<%#Bind("SALARY_ID")%>' Visible="false"></asp:Label>
                            </div>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_A%>" SortExpression="CHG_AMT_A">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lbl_CHG_AMT_A" runat="server" Text='<%#Bind("CHG_AMT_A","{0:N0}")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_START_DT%>" SortExpression="START_DT_A">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_START_DT_A" runat="server" CssClass="number" Text='<%# Convert.ToString(Eval("START_DT_A","{0:yyyy/MM/dd}"))%>'></asp:Label>

                            </div>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_END_DATE_A%>" SortExpression="END_DATE_A">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_END_DATE_A" runat="server" CssClass="number" Text='<%#Convert.ToString(Eval("END_DATE_B","{0:yyyy/MM/dd}"))%>'></asp:Label>

                            </div>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_PROCESS_STATUS%>" SortExpression="PROCESS_STATUS">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_PROCESS_STATUS" runat="server" Text='<%#Bind("DESC2")%>'></asp:Label>
                                <asp:HiddenField ID="hid_PROCESS_STATUS" runat="server" ClientIDMode="Static" Value='<%#Bind("PROCESS_STATUS")%>' />
                            </div>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CHG_STATUS%>" SortExpression="CHG_STATUS">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_CHG_STATUS" runat="server" Text='<%#Bind("DESC3")%>'></asp:Label>

                            </div>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_B%>" SortExpression="CHG_AMT_B">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lbl_CHG_AMT_B" runat="server" Text='<%#Bind("CHG_AMT_B","{0:N0}")%>'></asp:Label>

                            </div>
                        </ItemTemplate>


                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_END_DT_A%>" SortExpression="END_DATE_A">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_END_DT_B" runat="server" Text='<%# Convert.ToString(Eval("END_DATE_A","{0:yyyy/MM/dd}"))%>'></asp:Label>

                            </div>
                        </ItemTemplate>


                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_APPROVE_BY%>" SortExpression="APPROVE_BY">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_APPROVE_BY" runat="server" Text='<%#Convert.ToString(Eval("APPROVE_NAME"))%>'></asp:Label>
                                <asp:HiddenField ID="hid_APPROVE_BY" runat="server" Value='<%#Convert.ToString(Eval("APPROVE_BY"))%>' />
                                <asp:HiddenField ID="hid_CREATED_BY" runat="server" Value='<%#Convert.ToString(Eval("CREATED_BY"))%>' />
                            </div>
                        </ItemTemplate>


                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_APPROVE_DT%>" SortExpression="APPROVE_DT">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_APPROVE_DT" runat="server" Text='<%# Convert.ToString(Eval("APPROVE_DT","{0:yyyy/MM/dd}"))%>'></asp:Label>

                            </div>
                        </ItemTemplate>


                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="400">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <%--                                <asp:TextBox ID="lbl_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:TextBox>--%>
                                <asp:Label ID="lbl_REMARK" ClientIDMode="Static" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                                <%--                                <asp:TextBox ID="lb_REMARK" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("REMARK")%>' TextMode="MultiLine" Columns="45" Rows="3" ReadOnly="true" BorderWidth="0"></asp:TextBox>--%>
                            </div>
                        </ItemTemplate>


                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_APP_REMARK%>" SortExpression="APP_REMARK">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <%--                                <asp:Label ID="lbl_APP_REMARK" runat="server" Text='<%#Bind("APP_REMARK")%>'></asp:Label>--%>
                                <asp:TextBox ID="lbl_APP_REMARK" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("APP_REMARK")%>' TextMode="MultiLine" Width="100%" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                            </div>
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
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb200_mode" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2_Dtl_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidfb2CancelConfirm" Value="<%$Resources:Resource,wfb2CancelConfirm%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


