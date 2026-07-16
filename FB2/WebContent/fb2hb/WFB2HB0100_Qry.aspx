<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0100_Qry.aspx.cs" Inherits="WebContent_WFB2HB_WFB2HB0100_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            var currentDate = new Date();
            $('.date').datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask("9999/99/99");
            $('#txt_DEPT_NAME').attr("readonly", true);
            $('#txt_PJOB_DESC').attr("readonly", true);
            $('#txt_WORK_SHIFT_DESC').attr("readonly", true);

            //工號取得姓名的ajax
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        cache: false,
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

            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NO").change(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_NAME').val("");
                }
            });

            //輪值表代號取得輪值表名稱的ajax
            $("#txt_WORK_SHIFT_CD").change(function () {
                $.ajax({
                    url: "../commgeo/WFB2GetWorkShiftData.ashx",
                    data: {
                        WORK_SHIFT_CD: $('#txt_WORK_SHIFT_CD').val()
                    },
                    type: "GET",
                    cache: false,
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "") {
                            $('#txt_WORK_SHIFT_DESC').val("");
                        }
                        else {
                            $('#txt_WORK_SHIFT_DESC').val(JData.WORK_SHIFT_DESC);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            });

            //職務代號取得職務名稱的ajax
            $("#txt_PJOB_CD").change(function () {
                $.ajax({
                    url: "../commgeo/WFB2GetPjobData.ashx",
                    data: {
                        PJOB_CD: $('#txt_PJOB_CD').val(),
                        START_DT: currentDate.format("yyyy/MM/dd")
                    },
                    type: "GET",
                    cache: false,
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "") {
                            $('#txt_PJOB_DESC').val("");
                        }
                        else {
                            $('#txt_PJOB_DESC').val(JData.PJOB_DESC);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            });

            gridviewScroll();
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
                freezesize: 7

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        function openUpload() {
            window.location.href("WFB2HB0100_Upload.aspx");
            return false;
        }

        function openLICENSEID() {
            window.location.href("WFB2HB0100_LICENSEID.aspx");
            return false;
        }

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_EMP_CD").val("-1");
            $("#ddl_EMP_CHG_CD").val("-1");
            $("#txt_LICENSE_ID").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#ddl_WS_CD").val("-1");
            $("#ddl_COMPANY_CD").val("-1");
            $("#ddl_PLANT_CD").val("-1");
            $("#ddl_LEVEL_CD").val("-1");
            $("#txt_PJOB_CD").val("");
            $("#txt_PJOB_DESC").val("");
            $("#ddl_WORK_CD").val("-1");
            $("#ddl_JPN_CD").val("-1");
            $("#txt_WORK_SHIFT_CD").val("");
            $("#txt_WORK_SHIFT_DESC").val("");
            $("#txt_JOIN_DT_S").val("");
            $("#txt_JOIN_DT_E").val("");
            $("#txt_LEAVE_DT_S").val("");
            $("#txt_LEAVE_DT_E").val("");
            $("#txt_BE_CONTRACT_DT_S").val("");
            $("#txt_BE_CONTRACT_DT_E").val("");
            $("#txt_BE_EMP_DT_S").val("");
            $("#txt_BE_EMP_DT_E").val("");
            $("ddl_EMP_CHG_CD").val("-1");
            $('#cb_IS_DUTY_CHECK').prop('checked', '');
            $('#cb_OVERTIME_CTL_CD').prop('checked', '');
            $('#cb_MODEL').prop('checked', '');
            $('#cb_IS_UPD_HEAD').prop('checked', '');
            $('#cb_UNION_PJOB_CD').prop('checked', '');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
        <tr height="100%" valign="top">
            <td>
                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                    <colgroup>
                        <col width="10%" />
                        <col width="12%" />
                        <col width="10%" />
                        <col width="12%" />
                        <col width="10%" />
                        <col width="12%" />
                        <col width="10%" />
                        <col width="24%" />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="30" Width="80px" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_EMP_CD" runat="server" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_LICENSE_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LICENSE_ID%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_LICENSE_ID" runat="server" Width="80px" MaxLength="20" ClientIDMode="Static"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DEPT_NO%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                                <asp:TextBox ID="txt_DEPT_NAME" runat="server" MaxLength="60" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <%--在職區分--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_CHG_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_EMP_CHG_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WS_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_WS_CD" runat="server" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_COMPANY_CD%>"></asp:Label>:</th>
                            <td>
                                <asp:DropDownList ID="ddl_COMPANY_CD" runat="server" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PLANT_CD%>"></asp:Label>:</th>
                            <td>
                                <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static">
                                </asp:DropDownList>

                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LEVEL_CD%>"></asp:Label>:</th>
                            <td>
                                <asp:DropDownList ID="ddl_LEVEL_CD" runat="server" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PJOB_CD%>"></asp:Label>:</th>
                            <td>
                                <asp:TextBox ID="txt_PJOB_CD" runat="server" MaxLength="4" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                <input id="btn_PJOB_CD" type="button" value="..." onclick="OpenSearch('Pjob_Search.aspx', 'txt_PJOB_CD', 'txt_PJOB_DESC', '');" />
                                <asp:TextBox ID="txt_PJOB_DESC" runat="server" MaxLength="30" BorderWidth="0" ClientIDMode="Static" Width="64px"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_WORK_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WORK_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_WORK_CD" runat="server" ClientIDMode="Static">
                                </asp:DropDownList>

                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_JPN_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_JPN_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_JPN_CD" runat="server" ClientIDMode="Static">
                                </asp:DropDownList>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WORK_SHIFT_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:TextBox ID="txt_WORK_SHIFT_CD" runat="server" MaxLength="2" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                <input id="btn_WORK_SHIFT_CD" type="button" value="..." onclick="OpenSearch('WorkShift_Search.aspx', 'txt_WORK_SHIFT_CD', 'txt_WORK_SHIFT_DESC', '');" />
                                <asp:TextBox ID="txt_WORK_SHIFT_DESC" runat="server" MaxLength="60" BorderWidth="0" Width="200px" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_JOIN_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:TextBox ID="txt_JOIN_DT_S" CssClass="date" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static"></asp:TextBox>
                                ～　
							    <asp:TextBox ID="txt_JOIN_DT_E" CssClass="date" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static"></asp:TextBox>
                                <asp:CustomValidator ID="qrytest1" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_JOIN_DT_FORMAT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_JOIN_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="qrytest2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_JOIN_DT_FORMAT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_JOIN_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_JOIN_DT_S"
                                    ControlToValidate="txt_JOIN_DT_E" ErrorMessage="<%$Resources:Resource,wfb2hb_JOIN_DT_S_Compare%>" Type="Date" Operator="GreaterThanEqual"
                                    Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_LEAVE_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LEAVE_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:TextBox ID="txt_LEAVE_DT_S" CssClass="date" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static"></asp:TextBox>
                                ～　
							    <asp:TextBox ID="txt_LEAVE_DT_E" CssClass="date" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_LEAVE_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_LEAVE_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_LEAVE_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_LEAVE_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_LEAVE_DT_S"
                                    ControlToValidate="txt_LEAVE_DT_E" ErrorMessage="<%$Resources:Resource,wfb2hb_LEAVE_DT_Compare%>" Type="Date" Operator="GreaterThanEqual"
                                    Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_BE_CONTRACT_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BE_CONTRACT_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:TextBox ID="txt_BE_CONTRACT_DT_S" CssClass="date" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static"></asp:TextBox>
                                ～　
							    <asp:TextBox ID="txt_BE_CONTRACT_DT_E" CssClass="date" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_BE_CONTRACT_DT_FORMAT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_BE_CONTRACT_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_BE_CONTRACT_DT_FORMAT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_BE_CONTRACT_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToCompare="txt_BE_CONTRACT_DT_S"
                                    ControlToValidate="txt_BE_CONTRACT_DT_E" ErrorMessage="<%$Resources:Resource,wfb2hb_BE_CONTRACT_DT_Compare%>" Type="Date" Operator="GreaterThanEqual"
                                    Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_BE_EMP_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BE_EMP_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:TextBox ID="txt_BE_EMP_DT_S" CssClass="date" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static"></asp:TextBox>
                                ～　
							        <asp:TextBox ID="txt_BE_EMP_DT_E" CssClass="date" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator5" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_BE_EMP_DT_FORMAT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_BE_EMP_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="CustomValidator6" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_BE_EMP_DT_FORMAT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_BE_EMP_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToCompare="txt_BE_EMP_DT_S"
                                    ControlToValidate="txt_BE_EMP_DT_E" ErrorMessage="<%$Resources:Resource,wfb2hb_BE_EMP_DT_Compare%>" Type="Date" Operator="GreaterThanEqual"
                                    Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                                    <colgroup>
                                        <col width="10%" />
                                        <col width="56%" />
                                        <col width="10%" />
                                        <col width="24%" />

                                    </colgroup>
                                    <tbody>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_SP_EMP" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SP_EMP%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label" colspan="3">
                                                <asp:CheckBox ID="cb_IS_DUTY_CHECK" runat="server" Text="<%$Resources:Resource,wfb2hb_cb_IS_DUTY_CHECK%>" ClientIDMode="Static" />
                                                <asp:CheckBox ID="cb_OVERTIME_CTL_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_cb_OVERTIME_CTL_CD%>" ClientIDMode="Static" />
                                                <asp:CheckBox ID="cb_MODEL" runat="server" Text="<%$Resources:Resource,wfb2hb_cb_MODEL%>" ClientIDMode="Static" />
                                                <asp:CheckBox ID="cb_IS_UPD_HEAD" runat="server" Text="<%$Resources:Resource,wfb2hb_cb_IS_UPD_HEAD%>" ClientIDMode="Static" />
                                                <asp:CheckBox ID="cb_UNION_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_cb_UNION_PJOB_CD%>" ClientIDMode="Static" />

                                            </td>

                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="Body_label" colspan="8">
                                <div id="init">
                                    <aces:Btn ID="WFB2HB0100Search" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0100Search%>" OnClick="WFB2HB0100Search_Click" ValidationGroup="GroupA" />
                                    <%--<asp:Button ID="WFB2HB0100Search" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0100Search%>" OnClick="WFB2HB0100Search_Click" ValidationGroup="GroupA" />--%>
                                    <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2hb_WFB2HB0100Clear%>" onclick="ClearAll();" />
                                     <aces:Btn ID="WFB2HB0100UpdHead" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0100UpdHead%>" OnClick="WFB2HB0100UpdHead_Click" OnClientClick="BlockUI();" />
                                    <aces:Btn ID="WFB2HB0100Upload" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0100Upload%>" OnClientClick="return openUpload();" />
                                    <aces:Btn ID="WFB2HB0100LICENSEID" runat="server" Text="外籍居留證作業" OnClientClick="return openLICENSEID();" />
                                   <%--  <asp:Button ID="WFB2HB0100LICENSEID" runat="server" Text="外勞居留證作業" OnClientClick="return openLICENSEID();" />
                                   <asp:Button ID="WFB2HB0100UpdHead" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0100UpdHead%>" OnClick="WFB2HB0100UpdHead_Click" />
                                    <asp:Button ID="WFB2HB0100Upload" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0100Upload%>" OnClientClick="return openUpload();" />--%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <hr />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>

            <td align="right" class="Body_label">
                <div id="init_grid">
                    <aces:Btn ID="WFB2HB0100Add" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0100Add%>" OnClick="WFB2HB0100Add_Click" />
                    <aces:Btn ID="WFB2HB0100Edit" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0100Edit%>" OnClick="WFB2HB0100Edit_Click" Visible="false" />
                    <aces:Btn ID="WFB2HB0100Detail" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0100Detail%>" OnClick="WFB2HB0100Detail_Click" Visible="false" />
                    
                    <%--<asp:Button ID="WFB2HB0100Add" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0100Add%>" OnClick="WFB2HB0100Add_Click" />
                    <asp:Button ID="WFB2HB0100Edit" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0100Edit%>" OnClick="WFB2HB0100Edit_Click" Visible="false" />
                    <asp:Button ID="WFB2HB0100Detail" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0100Detail%>" OnClick="WFB2HB0100Detail_Click" Visible="false" />--%>
                </div>

            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HB0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME" DefaultValue=""
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_EMP_CD" DefaultValue=""
                        Name="emp_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_LICENSE_ID" DefaultValue=""
                        Name="license_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO" DefaultValue=""
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                     <asp:ControlParameter ControlID="ddl_EMP_CHG_CD" DefaultValue=""
                        Name="emp_chg_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_WS_CD" DefaultValue=""
                        Name="ws_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_COMPANY_CD" DefaultValue=""
                        Name="company_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_PLANT_CD" DefaultValue=""
                        Name="plant_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_LEVEL_CD" DefaultValue=""
                        Name="level_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_PJOB_CD" DefaultValue=""
                        Name="pjob_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_WORK_CD" DefaultValue=""
                        Name="work_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_JPN_CD" DefaultValue=""
                        Name="jpn_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_WORK_SHIFT_CD" DefaultValue=""
                        Name="work_shift_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_JOIN_DT_S" DefaultValue=""
                        Name="join_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_JOIN_DT_E" DefaultValue=""
                        Name="join_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_LEAVE_DT_S" DefaultValue=""
                        Name="leave_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_LEAVE_DT_E" DefaultValue=""
                        Name="leave_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_BE_CONTRACT_DT_S" DefaultValue=""
                        Name="be_contract_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_BE_CONTRACT_DT_E" DefaultValue=""
                        Name="be_contract_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_BE_EMP_DT_S" DefaultValue=""
                        Name="be_emp_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_BE_EMP_DT_E" DefaultValue=""
                        Name="be_emp_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="cb_IS_DUTY_CHECK" DefaultValue=""
                        Name="is_duty_check" PropertyName="Checked" Type="Boolean" />
                    <asp:ControlParameter ControlID="cb_OVERTIME_CTL_CD" DefaultValue=""
                        Name="overtime_ctl_cd" PropertyName="Checked" Type="Boolean" />
                    <asp:ControlParameter ControlID="cb_MODEL" DefaultValue=""
                        Name="model" PropertyName="Checked" Type="Boolean" />
                    <asp:ControlParameter ControlID="cb_IS_UPD_HEAD" DefaultValue=""
                        Name="is_upd_head" PropertyName="Checked" Type="Boolean" />
                    <asp:ControlParameter ControlID="cb_UNION_PJOB_CD" DefaultValue=""
                        Name="union_pjob_cd" PropertyName="Checked" Type="Boolean" />
                   
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px" 
                OnPageIndexChanging="gv_result_PageIndexChanging"  OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <asp:BoundField DataField="COMPANY_CD" HeaderText="<%$Resources:Resource,wfb2hb_lb_COMPANY_CD%>" SortExpression="COMPANY_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="PLANT_CD" HeaderText="<%$Resources:Resource,wfb2hb_lb_PLANT_CD%>" SortExpression="PLANT_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DEPT_NAME" HeaderText="<%$Resources:Resource,wfb2hb_lb_DEPT_NAME%>" SortExpression="DEPT_NAME" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_CHG_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_CHG_CD%>" SortExpression="EMP_CHG_CD" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="LICENSE_ID" HeaderText="<%$Resources:Resource,wfb2hb_lb_LICENSE_ID%>" SortExpression="LICENSE_ID" HeaderStyle-Width="100px" ItemStyle-Width="100px" HtmlEncode="false" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="PASSPORT_ID" HeaderText="<%$Resources:Resource,wfb2hb_lb_PASSPORT_ID%>" SortExpression="PASSPORT_ID" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_CD%>" SortExpression="EMP_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="WS_CD" HeaderText="<%$Resources:Resource,wfb2hb_lb_WS_CD%>" SortExpression="WS_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="LEVEL_CD" HeaderText="<%$Resources:Resource,wfb2hb_lb_LEVEL_CD2%>" SortExpression="LEVEL_CD" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="PJOB_CD" HeaderText="<%$Resources:Resource,wfb2hb_lb_PJOB_CD%>" SortExpression="PJOB_CD" HeaderStyle-Width="130px" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="JPN_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_JPN_CD%>" SortExpression="JPN_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="JOIN_DT" HeaderText="<%$Resources:Resource,wfb2hb_lb_JOIN_DT%>" SortExpression="JOIN_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="LEAVE_DT" HeaderText="<%$Resources:Resource,wfb2hb_lb_LEAVE_DT%>" SortExpression="LEAVE_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="LEAVE_REASON_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_LEAVE_REASON%>" SortExpression="LEAVE_REASON" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="BE_CONTRACT_DT" HeaderText="<%$Resources:Resource,wfb2hb_lb_BE_CONTRACT_DT%>" SortExpression="BE_CONTRACT_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="BE_EMP_DT" HeaderText="<%$Resources:Resource,wfb2hb_lb_BE_EMP_DT%>" SortExpression="BE_EMP_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="WORK_SHIFT_CD" HeaderText="<%$Resources:Resource,wfb2hb_lb_WORK_SHIFT_CD%>" SortExpression="WORK_SHIFT_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="WORK_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_WORK_CD%>" SortExpression="WORK_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="SERVICE_YEARS" HeaderText="<%$Resources:Resource,wfb2hb_lb_SERVICE_YEARS%>" SortExpression="SERVICE_YEARS" HeaderStyle-Width="80px" ItemStyle-Width="80px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="WORK_YEARS" HeaderText="<%$Resources:Resource,wfb2hb_lb_WORK_YEARS%>" SortExpression="WORK_YEARS" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="IS_DUTY_CHECK" HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_DUTY_CHECK2%>" SortExpression="IS_DUTY_CHECK" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="OVERTIME_CTL_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_OVERTIME_CTL_CD2%>" SortExpression="OVERTIME_CTL_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="MODEL_YEAR" HeaderText="<%$Resources:Resource,wfb2hb_lb_MODEL_YEAR%>" SortExpression="MODEL_YEAR" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="IS_UPD_HEAD" HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_UPD_HEAD2%>" SortExpression="IS_UPD_HEAD" HeaderStyle-Width="80px" ItemStyle-Width="80px" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="DIRECT_HEAD_EMP_NAME" HeaderText="<%$Resources:Resource,wfb2hb_lb_DIRECT_HEAD_EMP_ID%>" SortExpression="DIRECT_HEAD_EMP_ID" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="UNION_PJOB_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_UNION_PJOB_CD%>" SortExpression="UNION_PJOB_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>
