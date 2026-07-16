<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0500_Add.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0500_Add" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

     <style type="text/css">
        #txt_HR_CHG_CD {
            text-transform: uppercase;
        }
    </style>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date1").datepicker({ dateFormat: 'yy/mm' });
            $(".PROFESSION_ALLOWANCE").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });
            $(".MANAGEMENT_ALLOWANCE").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });
            $("#txt_HEAD_EMP_ID").mask("99999");
            $(".number2").mask("999");
            $(".date").mask("9999/99/99");
            $('#txt_HEAD_DEPT_NAME').attr("readonly", true);
            $('#txt_HEAD_EMP_NAME').attr("readonly", true);
            $('#txt_MAIN_LEAVE_DESC').attr("readonly", true);

            $("#txt_MAIN_LEAVE_CD").change(function () {
                $("#txt_MAIN_LEAVE_CD").val($("#txt_MAIN_LEAVE_CD").val().toUpperCase());
            });

            //工號取得姓名的ajax
            $("#txt_HEAD_EMP_ID").change(function () {
                if ($("#txt_HEAD_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_HEAD_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_HEAD_DEPT_NAME').val("");
                                $('#txt_HEAD_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_HEAD_EMP_NAME').val(JData.EMP_NAME);
                                $('#txt_HEAD_DEPT_NAME').val(JData.DEPT_NAME);

                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_HEAD_EMP_NAME').val("");
                    $('#txt_HEAD_DEPT_NAME').val("");
                }
            });
            $.unblockUI();
        }//月曆

        function getEmp() {
            var json = OpenEmpSearch('txt_HEAD_EMP_ID', 'txt_HEAD_EMP_NAME', 'N');
            if (json != undefined) {
                $("#txt_HEAD_DEPT_NAME").val(json.DEPT_NAME)
                $("#HID_DEPT_NO").val(json.DEPT_NO)
            }
        }

        function getMAIN_LEAVE_CD() {
            var json = OpenSearch('LeaveType_Search.aspx', 'txt_MAIN_LEAVE_CD', 'txt_MAIN_LEAVE_DESC', '', 'Y');

        }

        function LeaveType_Search(id, name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#txt_MAIN_LEAVE_CD").val(obj.CD);
                $("#txt_MAIN_LEAVE_DESC").val(obj.DESC);
                __doPostBack('Main_OnTextChanged', '');
                //return obj;
            }
        }
        function ShowRecord(obj) {

            $("#HID_DEPT_NO").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;
            if (!Page_ClientValidate("GroupA")) {
                processed = false;
            }

            if(!Page_ClientValidate("GroupB"))
                processed = false;

            if (processed) {
                if ($("#HID_LEAVE_COUNT_CD").val() == "E") {
                    if ($("#txt_FACT_HAPPEN_DT").val() == "") {
                        alert("子假別為E事件時，事實發生日必須輸入，不可空白");
                        processed = false;
                    }
                }
                if ($("#txt_APPLY_LEAVE_SDT").val() != $("#txt_APPLY_LEAVE_EDT").val()) {
                    alert("請假起迄日期應為同一天");
                    processed = false;
                }
            }

            if (processed)
                BlockUI();
            if (!processed)
                $.unblockUI();

            return processed;
        }

        function backToQry() {
            window.location.href = "WFB2DH0500_Qry.aspx";
        }
        function Checkconfirm(message) {
            if (confirm(message))
                backToQry();

            return false;
        }
        function openQryLeave(url) {
            window.open(url + "&parentFuncId=" + parentFuncID);
        }
    </script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
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
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_HEAD_EMP_ID" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_HEAD_EMP_ID%>"
                                ControlToValidate="txt_HEAD_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_HEAD_EMP_ID%>"
                                ControlToValidate="txt_HEAD_EMP_ID" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <input id="bt_HEAD_EMP_ID" type="button" value="..." onclick="getEmp();" />
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_HEAD_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>

                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DEPT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_HEAD_DEPT_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>


                        </td>

                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static">請假實績:</asp:Label>
                        </td>
                    </tr>
                    <%--主假別 --%>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_MAIN_LEAVE_CD" runat="server" MaxLength="2" Width="64px" ClientIDMode="Static" OnTextChanged="txt_MAIN_LEAVE_CD_TextChanged1" AutoPostBack="true" CssClass="MandatoryField" Enabled="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_MAIN_LEAVE_CD%>"
                                ControlToValidate="txt_MAIN_LEAVE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <input id="Button1" type="button" value="..." onclick="getMAIN_LEAVE_CD();" />
                            <asp:TextBox ID="txt_MAIN_LEAVE_DESC" runat="server" MaxLength="10" Width="50px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SUB_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_SUB_LEAVE_CD" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddl_SUB_LEAVE_CD_SelectedIndexChanged" AutoPostBack="true" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_SUB_LEAVE_CD%>"
                                ControlToValidate="ddl_SUB_LEAVE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEAVE_TIME_UNIT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_TIME_UNIT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lb_LEAVE_MIN_VALUE" Text="" runat="server" Width="30px" MaxLength="10" ClientIDMode="Static"></asp:Label>
                            <asp:Label ID="txt_LEAVE_TIME_UNIT" Text="" runat="server" Width="80px" MaxLength="10" ClientIDMode="Static"></asp:Label>
                            <asp:HiddenField ID="hid_LEAVE_TIME_UNIT" runat="server" ClientIDMode="Static" />
                        </td>

                    </tr>
                    <%-- 請假勤務日 --%>
                    <tr>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_APPLY_LEAVE_SDT"
                            ControlToValidate="txt_APPLY_LEAVE_EDT" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_CALENDAR_DT%>" Type="Date" Operator="GreaterThanEqual"
                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_SDT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_SDT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_APPLY_LEAVE_SDT" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date" OnTextChanged="txt_APPLY_LEAVE_SDT_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_APPLY_LEAVE_SDT%>"
                                ControlToValidate="txt_APPLY_LEAVE_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_APPLY_LEAVE_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_LEAVE_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_EDT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_EDT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_APPLY_LEAVE_EDT" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" CssClass="MandatoryField" Enabled="false"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_APPLY_LEAVE_EDT%>"
                                ControlToValidate="txt_APPLY_LEAVE_EDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_APPLY_LEAVE_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_LEAVE_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>
                        <td>
                            <%-- 計算--%>
                            <aces:Btn ID="WFB2DH0501COUNT" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500COUNT%>" OnClick="WFB2DH0500COUNT_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                        </td>
                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_STIME" runat="server" Text="<%$Resources:Resource,wfb2dh_APPLY_LEAVE_STIME1%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_APPLY_LEAVE_STIME_H" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>時
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_APPLY_LEAVE_STIME_H%>"
                        ControlToValidate="ddl_APPLY_LEAVE_STIME_H" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddl_APPLY_LEAVE_STIME_M" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>分
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_APPLY_LEAVE_STIME_M%>"
                        ControlToValidate="ddl_APPLY_LEAVE_STIME_M" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_ETIME" runat="server" Text="<%$Resources:Resource,wfb2dh_APPLY_LEAVE_ETIME1%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_APPLY_LEAVE_ETIME_H" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>時
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_APPLY_LEAVE_ETIME_H%>"
                                            ControlToValidate="ddl_APPLY_LEAVE_ETIME_H" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddl_APPLY_LEAVE_ETIME_M" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>分
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_APPLY_LEAVE_ETIME_M%>"
                                            ControlToValidate="ddl_APPLY_LEAVE_ETIME_M" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_TOTAL_TIME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_TOTAL_TIME%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DATE" runat="server" MaxLength="2" Width="30px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>日
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_DATE%>"
                        ControlToValidate="txt_DATE" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txt_HOUR" runat="server" MaxLength="2" Width="30px" ClientIDMode="Static" Text="" CssClass="MandatoryField"></asp:TextBox>時
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_HOUR%>"
                        ControlToValidate="txt_HOUR" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txt_MINUTE" runat="server" MaxLength="2" Width="30px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>分
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_MINUTE%>"
                        ControlToValidate="txt_MINUTE" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:HiddenField ID="hid_nn_total" runat="server" ClientIDMode="Static" />
                        </td>

                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_FACT_HAPPEN_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_FACT_HAPPEN_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_FACT_HAPPEN_DT" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator5" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_FACT_HAPPEN_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_FACT_HAPPEN_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_OVERTIME_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="2">
                            <asp:TextBox ID="txt_APPLY_OVERTIME_DT" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_APPLY_OVERTIME_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_OVERTIME_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            (＊請代休假時，需填入！)</td>

                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEAVE_REASON" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_REASON%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="4">
                            <asp:TextBox ID="txt_LEAVE_REASON" runat="server" MaxLength="100" Width="580px" TextMode="MultiLine" Rows="2" ClientIDMode="Static"></asp:TextBox>
                        </td>

                    </tr>

                    <tr>
                        <td align="left" class="Body_label">
                            <aces:Btn ID="WFB2DH0500LEAVECOUNT" runat="server" Text="<%$Resources:Resource,wfb2dh_btn_LEAVECOUNT%>" OnClick="btn_LEAVECOUNT_Click" ValidationGroup="GroupB" formnovalidate />

                            <%--<asp:Button ID="WFB2DH0500LEAVECOUNT" runat="server" Text="<%$Resources:Resource,wfb2dh_btn_LEAVECOUNT%>" OnClick="btn_LEAVECOUNT_Click" ValidationGroup="GroupB" formnovalidate />--%>
                        </td>

                        <td align="left" class="Body_label"></td>

                        <td align="left" class="Body_label"></td>
                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_APPROVE_DT1" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IFLOW_APPROVE_DT1%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_IFLOW_APPROVE_DT1" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_errformat_IFLOW_APPROVE_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_IFLOW_APPROVE_DT1" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PAY_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_PAY_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_PAY_DT" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator6" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_errformat_PAY_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_PAY_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IFLOW_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_IFLOW_NO" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" Text="　"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <%--刷卡比對狀態: --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CHECK_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_CHECK_STATUS%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_CHECK_STATUS" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" Text=""></asp:Label>
                        </td>
                        <%--計薪狀態: --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SALARY_SETTLE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SALARY_SETTLE_STATUS%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_SALARY_SETTLE_STATUS" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" Text="" Height="16px"></asp:Label>
                        </td>
                        <%--表單狀態: --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_FORM_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_FORM_STATUS%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_FORM_STATUS" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_REMARK%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="4">
                            <asp:TextBox ID="txt_REMARK" runat="server" MaxLength="100" Width="580px" TextMode="MultiLine" Rows="2" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td align="right" class="Body_label" colspan="5"></td>
                        <td>
                            <aces:Btn ID="WFB2DH0502Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2DH0500Save_Click" ValidationGroup="GroupA" OnClientClick="return saveCheck();" />

                            <%--<asp:Button ID="WFB2DH0502Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2DH0500Save_Click" ValidationGroup="GroupA" OnClientClick="return saveCheck();" />--%>
                            <asp:Button ID="WFB2DH0500Cancel" runat="server" Text="<%$Resources:Resource,btn_cancel%>" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2DH0500Cancel_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
    <asp:HiddenField ID="HID_DEPT_NO" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HID_LEAVE_COUNT_CD" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HID_LEAVE_ALLOW_CD" runat="server" ClientIDMode="Static" />
</asp:Content>
