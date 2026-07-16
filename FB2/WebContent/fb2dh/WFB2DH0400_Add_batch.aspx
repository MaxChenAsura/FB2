<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0400_Add_batch.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0400_Add_batch" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask("9999/99/99");
            $(".number2").mask("999");
            $("#txt_DD").mask("999");
            $("#txt_HH").mask("999");
            $("#txt_MM").mask("999");
            $('#txt_MAIN_LEAVE_DESC').attr("readonly", true);
            $('#txt_DEPT_NAME').attr("readonly", true);
            $('#txt_WORK_SHIFT_DESC').attr("readonly", true);
            $('#txt_NEW_EMP_NAME').attr("readonly", true);
            $('#txt_APPLY_LEAVE_EDT').attr("readonly", true);
            $('#txt_DUTY_TIME').attr("readonly", true);
            $('#txt_LEAVE_TIME_UNIT').attr("readonly", true);

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
                                $('#txt_NEW_PLANT_CD').text("");
                                $('#txt_NEW_DEPT_NO').text("");
                                $('#txt_NEW_WORK_CD').text("");
                                $('#txt_NEW_WORK_SHIFT_CD').text("");
                                alert(JData.errMsg);
                            }
                            else {

                                $('#txt_NEW_EMP_NAME').val(JData.EMP_NAME);
                                $('#txt_NEW_PLANT_CD').text(JData.PLANT_NAME);
                                $('#txt_NEW_DEPT_NO').text(JData.DEPT_NAME);
                                $('#txt_NEW_WORK_CD').text(JData.WORK_CD);
                                $('#txt_NEW_WORK_SHIFT_CD').text(JData.WORK_SHIFT_NAME);
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
            $.unblockUI();
            gridviewScroll();

        }
        function getEmp() {
            OpenEmpSearch('txt_NEW_EMP_ID', 'txt_NEW_EMP_NAME', 'N','Y');
            //var json = OpenEmpSearch('txt_NEW_EMP_ID', 'txt_NEW_EMP_NAME', 'N');
            //if (json != undefined) {
            //    $("#txt_NEW_DEPT_NO").text(json.DEPT_NAME);
            //    $("#txt_NEW_PLANT_CD").text(json.PLANT_NAME);
            //    $("#txt_NEW_WORK_SHIFT_CD").text(json.WORK_SHIFT_DESC);
            //}
        }

        function returnEMPValueToPage(eid, ename, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            //alert(returnValue);
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $('#' + eid).val(obj.EMP_ID.trim());
                doEmpAjax();
                //$('#' + ename).val(obj.EMP_NAME.trim());
                //$("#txt_NEW_DEPT_NO").text(obj.DEPT_NAME);
                //$("#txt_NEW_PLANT_CD").text(obj.PLANT_NAME);
                //$("#txt_NEW_WORK_SHIFT_CD").text(obj.WORK_SHIFT_DESC);

            }
        }

        function doEmpAjax() {
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


                            $('#txt_NEW_EMP_NAME').text("");
                            $('#txt_NEW_PLANT_CD').text("");
                            $('#txt_NEW_DEPT_NO').text("");
                            $('#txt_NEW_WORK_CD').text("");
                            $('#txt_NEW_WORK_SHIFT_CD').text("");
                            $('#txt_NEW_WORK_CD').text("");
                            alert(JData.errMsg);
                        }
                        else {

                            $('#txt_NEW_EMP_NAME').text(JData.EMP_NAME);
                            $('#txt_NEW_PLANT_CD').text(JData.PLANT_NAME);
                            $('#txt_NEW_DEPT_NO').text(JData.DEPT_NAME);
                            $('#txt_NEW_WORK_CD').text(JData.WORK_CD);
                            $('#txt_NEW_WORK_SHIFT_CD').text(JData.WORK_SHIFT_NAME);
                            $('#txt_NEW_WORK_CD').text(JData.WORK_CD);
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
        }

        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "999",
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 0

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");

        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        

        function getWorkShift() {
            OpenSearch('WorkShift_Search.aspx', 'txt_WORK_SHIFT_CD', 'txt_WORK_SHIFT_DESC', '');
            //var json = OpenSearch('WorkShift_Search.aspx', 'txt_WORK_SHIFT_CD', 'txt_WORK_SHIFT_DESC', '');           
        }

        function WorkShift_Search(obj_cd, obj_desc, value) {
            var returnValue = value;
            if (returnValue == undefined) {
                returnValue = window.returnValue;
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                window.parent.$('#' + obj_cd).val(obj.CD);
                window.parent.$('#' + obj_desc).val(obj.DESC);
                __doPostBack('<%#txt_WORK_SHIFT_CD.ClientID %>', 'OnTextChanged');
            }
        }

        //清空畫面
        function ClearAll() {
            $("#ddl_PLANT_CD").val("-1");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#ddl_WS_CD").val("-1");
            $("#ddl_WORK_CD").val("-1");
            $("#txt_WORK_SHIFT_CD").val("");
            $("#txt_WORK_SHIFT_DESC").val("");
            $("#txt_DUTY_TIME").val("");
        }

        function getMAIN_LEAVE_CD() {
            //OpenSearch('LeaveType_Search.aspx', 'txt_MAIN_LEAVE_CD', 'txt_MAIN_LEAVE_DESC', '');
            var json = OpenSearch('LeaveType_Search.aspx', 'txt_MAIN_LEAVE_CD', 'txt_MAIN_LEAVE_DESC', '', 'Y');
            __doPostBack('<%#txt_MAIN_LEAVE_CD.ClientID %>', 'OnTextChanged');
        }

        function LeaveType_Search(obj_cd, obj_desc, value) {
            var returnValue = value;
            if (returnValue == undefined) {
                returnValue = window.returnValue;
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                window.parent.$('#' + obj_cd).val(obj.CD);
                window.parent.$('#' + obj_desc).val(obj.DESC);
                __doPostBack('<%#txt_MAIN_LEAVE_CD.ClientID %>', 'OnTextChanged');
            }
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb299_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
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
        function backToQry() {
            window.location.href = "WFB2DH0400_Qry.aspx";
        }

        //儲存前檢查
        function saveCheck() {
            var processed = true;


            if ($("#txt_APPLY_LEAVE_SDT").val() != $("#txt_APPLY_LEAVE_EDT").val()) {
                alert("請假起迄日期應為同一天");
                processed = false;
            }
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }

        //查詢前檢查
        function searchCheck() {
            var processed = true;
            if (Page_ClientValidate("GroupB")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }

        function setEDT() {
            $("#txt_APPLY_LEAVE_EDT").val($("#txt_APPLY_LEAVE_SDT").val());
        }

        //主假別用視窗挑選回傳的資料
        function LeaveType_Search(id, name, json) {
            var returnValue = json;
            if (json == undefined) {
                returnValue = 'undefined';
            }
            //alert(returnValue);
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#txt_MAIN_LEAVE_CD").val(obj.CD);
                $("#txt_MAIN_LEAVE_DESC").val(obj.DESC);
            } else {

            }
            __doPostBack('<%#txt_MAIN_LEAVE_CD.ClientID %>', 'OnTextChanged');
            //__doPostBack('leaveType', '');
        }

    </script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                <%--查詢條件欄位寬度--%>
                <colgroup>
                    <col width="12%" />
                    <col width="21%" />
                    <col width="12%" />
                    <col width="21%" />
                    <col width="12%" />
                    <col width="22%" />
                </colgroup>
                <tbody>
                    <tr>
                        <td>
                            <asp:Label ID="lb_Leave_Apply" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Leave_Add_batch%>"></asp:Label>:
                        </td>
                    </tr>
                    <tr>
                        <%-- 主假別 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_MAIN_LEAVE_CD" runat="server" CssClass="MandatoryField" MaxLength="2" class="MandatoryField" Width="42px" ClientIDMode="Static" OnTextChanged="txt_MAIN_LEAVE_CD_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_MAIN_LEAVE_CD %> "
                                ControlToValidate="txt_MAIN_LEAVE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                            <input id="Button1" type="button" value="..." onclick="getMAIN_LEAVE_CD();" />
                            <asp:TextBox ID="txt_MAIN_LEAVE_DESC" runat="server" MaxLength="10" Width="100px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <%-- 子假別 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SUB_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_SUB_LEAVE_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static" OnSelectedIndexChanged="ddl_SUB_LEAVE_CD_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_SUB_LEAVE_CD %> "
                                ControlToValidate="ddl_SUB_LEAVE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                        </td>
                        <%-- 時間單位 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEAVE_TIME_UNIT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_TIME_UNIT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_LEAVE_TIME_UNIT" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                            <asp:HiddenField ID="hid_LEAVE_TIME_UNIT" runat="server" />
                        </td>
                    </tr>

                    <tr>
                         <%-- 勤務日期 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CALENDAR_DT" runat="server" Text="勤務日期"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_CALENDAR_DT" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="81px" ClientIDMode="Static" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="勤務日期不可空白"
                                ControlToValidate="txt_CALENDAR_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <%-- 請假勤務日-起 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_SDT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_SDT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_APPLY_LEAVE_SDT" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="81px" ClientIDMode="Static" onchange="setEDT();"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="Unknown1" runat="server" ErrorMessage="<% $Resources:Resource,wfb2dh_Required_APPLY_LEAVE_SDT2 %> "
                                ControlToValidate="txt_APPLY_LEAVE_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_errformat_APPLY_LEAVE_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_LEAVE_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<% $Resources:Resource,wfb2dh_Required_APPLY_LEAVE_SDT2 %> "
                                ControlToValidate="txt_APPLY_LEAVE_SDT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_errformat_APPLY_LEAVE_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_LEAVE_SDT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_EDT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_EDT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_APPLY_LEAVE_EDT" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="81px" Enabled="false" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<% $Resources:Resource,wfb2dh_Required_APPLY_LEAVE_EDT2 %> "
                                ControlToValidate="txt_APPLY_LEAVE_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ErrorMessage="<% $Resources:Resource,wfb2dh_errformat_APPLY_LEAVE_EDT%>" ControlToValidate="txt_APPLY_LEAVE_SDT" ForeColor="Red"
                                ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>--%>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_errformat_APPLY_LEAVE_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_LEAVE_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>

                         <%-- 計算 
                        <td align="left" class="Body_label">
                            <aces:Btn ID="WFB2DH0404Calculate" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Calculate%>" OnClick="WFB2DH0400Calculate_Click" />
                        </td>
                        --%>
                    </tr>

                    <tr>
                        <td></td>
                        <td></td>    
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_STIME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_STIME%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_hours" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_SH %> "
                                ControlToValidate="ddl_hours" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:Label ID="lb_Hour" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Hour%>"></asp:Label>
                            <asp:DropDownList ID="ddl_minutes" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_SM %> "
                                ControlToValidate="ddl_minutes" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:Label ID="lb_Min" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Min%>"></asp:Label>
                        </td>
                        
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_ETIME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_ETIME%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_hours2" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_EH %> "
                                ControlToValidate="ddl_hours2" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:Label ID="lb_Hour2" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Hour%>"></asp:Label>
                            <asp:DropDownList ID="ddl_minutes2" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_EM %> "
                                ControlToValidate="ddl_minutes2" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:Label ID="lb_Min2" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Min%>"></asp:Label>
                        </td>
                         <%-- 計算 
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_TOTAL_TIME_APPROVE" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_TOTAL_TIME_APPROVE%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DD" runat="server" CssClass="MandatoryField" Columns="1" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_DD %> "
                                ControlToValidate="txt_DD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:Label ID="lb_Date" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Date%>"></asp:Label>

                            <asp:TextBox ID="txt_HH" runat="server" CssClass="MandatoryField" Columns="1" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_HH %> "
                                ControlToValidate="txt_HH" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:Label ID="lb_Hour3" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Hour%>"></asp:Label>

                            <asp:TextBox ID="txt_MM" runat="server" CssClass="MandatoryField" Columns="1" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_MM %> "
                                ControlToValidate="txt_MM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:Label ID="lb_Min3" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Min%>"></asp:Label>

                            <!--	0 日 8 時 0 分	-->
                        </td>--%>
                             
                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEAVE_REASON" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_REASON%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="12 ">
                            <asp:TextBox ID="txt_LEAVE_REASON" runat="server" TextMode="MultiLine" MaxLength="6" Columns="80" ClientIDMode="Static"></asp:TextBox>

                        </td>
                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_REMARK%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="12 ">
                            <asp:TextBox ID="txt_REMARK" runat="server" TextMode="MultiLine" MaxLength="6" Columns="80" ClientIDMode="Static"></asp:TextBox>

                        </td>
                    </tr>
                </tbody>
            </table>
            <%-- 人員選取 --%>
            <fieldset style="padding: 5px; padding-top: 10px; border-color: red; width: 1000px;">
                <legend class="Body_label"><font color="Red" size="+1">人員選取</font></legend>
                <table width="1000px" border="0">
                    <colgroup>
                        <col width="10%" />
                        <col width="21%" />
                        <col width="10%" />
                        <col width="27%" />
                        <col width="10%" />
                        <col width="22%" />
                    </colgroup>
                    <tbody>
                        <tr>
                            <%-- 工廠區分 --%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_PLANT_CD%>"></asp:Label>:
                            </th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                             <%-- 部門 --%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_DEPT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DEPT%>"></asp:Label>:
                            </th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                <input id="Button14" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                                <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                            </td>
                             <%-- 職種 --%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_WS_CD%>"></asp:Label>:
                            </th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_WS_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <%-- 工數區分 --%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_WORK_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_WORK_CD%>"></asp:Label>:
                            </th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_WORK_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                             <%-- 輪值表別 --%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_WORK_SHIFT_CD%>"></asp:Label>:
                            </th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_WORK_SHIFT_CD" runat="server" MaxLength="2" Width="64px" ClientIDMode="Static" OnTextChanged="txt_WORK_SHIFT_CD_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <input id="btn_WORK_SHIFT_CD" type="button" value="..." onclick="getWorkShift();" />
                                <asp:TextBox ID="txt_WORK_SHIFT_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <%-- 勤務起訖時間 --%>
                            <th align="left" class="Body_TableHeader">勤務起訖時間:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_DUTY_TIME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                            </td>
                                
                        </tr>
                        <tr>
                            <%-- 班別 --%>
                            <th align="left" class="Body_TableHeader">班別:</th>
                            <td align="left" class="Body_label" colspan='4'>
                                 <asp:DropDownList ID="ddl_SHIFT_CD" runat="server"  ClientIDMode="Static"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="Body_label" colspan="4">
                                <asp:RadioButtonList ID="rbl_APPLY_TYPE" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static" OnSelectedIndexChanged="rbl_APPLY_TYPE_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2dh_Radiobutton1%>" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2dh_Radiobutton2%>" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>

                        <tr>
                            <td align="right" colspan="6">
                                <%--<aces:Btn ID="WFB2DH0404Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Search%>" OnClick="WFB2DH0400Search_Click" Visible="true" />--%>

                                <asp:Button ID="WFB2DH0404Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Search%>" OnClick="WFB2DH0400Search_Click" Visible="false"   OnClientClick="return searchCheck();" />
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dh_btn_clear%>" onclick="ClearAll();" />
                                <br />
                               <%-- <aces:Btn ID="WFB2DH0404Add" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Add%>" OnClick="WFB2DH0400Add_Click" Visible="false" />
                                <aces:Btn ID="WFB2DH0404Delete" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Delete%>" OnClick="WFB2DH0400Delete_Click" Visible="false" OnClientClick="return CheckDelAction();" />--%>

                                <asp:Button ID="WFB2DH0404Add" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Add%>" OnClick="WFB2DH0400Add_Click" Visible="false" />
                            <asp:Button ID="WFB2DH0404Delete" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Delete%>" OnClick="WFB2DH0400Delete_Click" Visible="false" OnClientClick="return CheckDelAction();" />

                                <br />
                                <aces:Btn ID="WFB2DH0404Save" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Save%>" OnClick="WFB2DH0400Save_Click" Visible="false" />

                                <%--<asp:Button ID="WFB2DH0404Save" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Save%>" OnClick="WFB2DH0400Save_Click" Visible="false" />--%>
                                <asp:Button ID="WFB2DH0400Cancel2" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Cancel%>" OnClick="WFB2DH0400Cancel2_Click" Visible="false" OnClientClick="return confirm('是否確定取消?');" />
                        </tr>
                    </tbody>
                </table>
                <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getBatchData" SelectCountMethod="getBatchCount"
                    TypeName="CFB2DH0400DAO" EnablePaging="True" SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                    OnSelected="ods1_Selected">
                    <SelectParameters>
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                        <asp:Parameter Name="maximumRows" Type="Int32" />
                        <asp:ControlParameter ControlID="ddl_PLANT_CD"
                            Name="plant_cd" PropertyName="SelectedValue" Type="String" />
                        <asp:ControlParameter ControlID="txt_DEPT_NO" DefaultValue="" ConvertEmptyStringToNull="false"
                            Name="dept_no" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="ddl_WS_CD" DefaultValue=""
                            Name="ws_cd" PropertyName="SelectedValue" Type="String" />
                        <asp:ControlParameter ControlID="ddl_WORK_CD" DefaultValue=""
                            Name="work_cd" PropertyName="SelectedValue" Type="String" />
                        <asp:ControlParameter ControlID="txt_WORK_SHIFT_CD" DefaultValue="" ConvertEmptyStringToNull="False"
                            Name="work_shift_cd" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="hid_AddEMP"
                            Name="AddEmp" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                        <asp:ControlParameter ControlID="hid_DeleteEMP"
                            Name="DeleteEmp" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                         <asp:ControlParameter ControlID="ddl_SHIFT_CD" DefaultValue=""
                            Name="shift_cd" PropertyName="SelectedValue" Type="String" />
                         <asp:ControlParameter ControlID="txt_APPLY_LEAVE_SDT" DefaultValue="" ConvertEmptyStringToNull="False"
                            Name="apply_leave_dt" PropertyName="Text" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>

                <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                    AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                    OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="980px"
                    OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="20px">
                            <HeaderTemplate>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_RowNumber%>" HeaderStyle-Width="20px">
                            <ItemTemplate>
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <%--                <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="10px" />--%>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txt_NEW_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                <input id="bt_EMP_ID" type="button" value="..." onclick="getEmp();" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_EMP_ID%>"
                                    ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txt_NEW_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" Width="60px"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_PLANT_CD%>" SortExpression="PLANT_CD" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lb_PLANT_CD" runat="server" Text='<%#Bind("PLANT_CD")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="txt_NEW_PLANT_CD" runat="server" MaxLength="4"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_FULL_NAME")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="txt_NEW_DEPT_NO" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_WORK_CD%>" SortExpression="WORK_CD" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lb_WORK_CD" runat="server" Text='<%#Bind("WORK_CD")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="txt_NEW_WORK_CD" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_WORK_SHIFT_CD%>" SortExpression="WORK_SHIFT_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text='<%#Bind("WORK_SHIFT_CD")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="txt_NEW_WORK_SHIFT_CD" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <EmptyDataTemplate>


                        <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                            <tr class="header">
                                <td></td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dh_RowNumber%>" Width="20px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>" Width="40px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>" Width="60px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_PLANT_CD%>" Width="60px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DEPT_NO%>" Width="100px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_WORK_CD%>" Width="60px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_WORK_SHIFT_CD%>" Width="100px"></asp:Label>
                                </td>
                            </tr>
                            <tr class="normal">
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                    <input id="bt_EMP_ID" type="button" value="..." onclick="getEmp();" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_EMP_NAME" runat="server" MaxLength="10" Width="80px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="txt_NEW_PLANT_CD" runat="server" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="txt_NEW_DEPT_NO" runat="server" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="txt_NEW_WORK_CD" runat="server" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="txt_NEW_WORK_SHIFT_CD" runat="server" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <HeaderStyle CssClass="GridviewScrollHeader" />
                    <PagerStyle CssClass="GridviewScrollPager" />
                    <FooterStyle CssClass="normal" />
                </asp:GridView>
                <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                    <tr valign="top">

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
            </fieldset>
            <table border="0" width="1020px">
                <tr align="right">
                    <td>
                        <aces:Btn ID="WFB2DH0404Confirm" runat="server" OnClick="WFB2DH0400Confirm_Click" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Confirm%>" OnClientClick="return saveCheck();" />
                        <%--<asp:Button ID="WFB2DH0404Confirm" runat="server" OnClick="WFB2DH0400Confirm_Click" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Confirm%>" OnClientClick="return saveCheck();"/>--%>
                        <asp:Button ID="WFB2DH0400Cancel" runat="server" OnClick="WFB2DH0400Cancel_Click" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Cancel%>" OnClientClick="return confirm('是否確定取消?');" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:HiddenField ID="hid_AddEMP" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_DeleteEMP" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <caption>

                <!-- END: Create a line -->
                <%--<asp:HiddenField ID="hid_DEPT_NO" runat="server" ClientIDMode="Static" />--%>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ShowSummary="false" ValidationGroup="GroupA" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ShowSummary="false" ValidationGroup="GroupB" />
            </caption>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DH0404Confirm"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
