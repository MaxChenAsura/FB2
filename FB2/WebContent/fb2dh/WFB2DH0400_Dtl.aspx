<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0400_Dtl.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0400_Dtl" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //iniForm(); //月曆
        });

        //function iniForm() {
        //    $(".date").datepicker({ dateFormat: 'yy/mm/dd' });

        //    $(".number2").mask("999");
        //    $.unblockUI();

        //}//月曆


        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function getEmp() {
            var json = OpenEmpSearch('txt_HEAD_EMP_ID', 'txt_HEAD_EMP_NAME', 'N');
            if (json != undefined) {
                $("#txt_DEPT_NAME").val(json.DEPT_NAME)
                $("#hid_DEPT_NO").val(json.DEPT_NO)
            }

            //alert("所屬部門代號:" + json.DEPT_NO + "\n" + "所屬部門名稱:" + json.DEPT_NAME + "\n" +
            //    "員工區分:" + json.EMP_CD + "\n" + "資格代號:" + json.LEVEL_CD + "\n" +
            //    "級數代號:" + json.GRADE_CD + "\n" + "職務代號:" + json.PJOB_CD + "\n" +
            //    "員工區分:" + json.JOIN_DT + "\n" + "資格代號:" + json.BE_EMP_DT + "\n" +
            //    "職種:" + json.WS_CD + "\n" + "在職狀態:" + json.EMP_STATUS + "\n");
        }
        function getEmp() {
            var json = OpenEmpSearch('txt_HEAD_EMP_ID', 'txt_HEAD_EMP_NAME', 'N');
            if (json != undefined) {
                $("#txt_DEPT_NAME").val(json.DEPT_NAME)
                $("#hid_DEPT_NO").val(json.DEPT_NO)
            }
        }
    </script>

    <style type="text/css">
        .auto-style1 {
            width: 268435136px;
        }

        .date {
        }
    </style>


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
                    <%--工號、姓名、部門--%>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_HEAD_EMP_ID" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_HEAD_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DEPT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_DEPT_NAME" runat="server" MaxLength="13" BorderWidth="0" ClientIDMode="Static"></asp:Label>
                        </td>
                    </tr>
                    <%--請假申請--%>
                    <tr>
                        <td>
                            <asp:Label ID="lb_Leave_Apply" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Leave_Apply%>"></asp:Label>:
                        </td>
                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_MAIN_LEAVE_DESC" runat="server" MaxLength="10" Width="100px" BorderWidth="0" ClientIDMode="Static"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SUB_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="ddl_SUB_LEAVE_CD" runat="server" ClientIDMode="Static"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEAVE_TIME_UNIT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_TIME_UNIT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lb_LEAVE_MIN_VALUE" Text="" runat="server" Width="30px" MaxLength="10" ClientIDMode="Static"></asp:Label>
                            <asp:Label ID="txt_LEAVE_TIME_UNIT" runat="server" BorderWidth="0" MaxLength="6" Width="42px" ClientIDMode="Static"></asp:Label>
                        </td>
                    </tr>

                   

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_SDT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_SDT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_APPLY_LEAVE_SDT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static"></asp:Label>

                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_EDT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_EDT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_APPLY_LEAVE_EDT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static"></asp:Label>
                        </td>

                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_STIME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_STIME%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="ddl_hours" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:Label ID="lb_Hour" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Hour%>"></asp:Label>
                            <asp:Label ID="ddl_minutes" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:Label ID="lb_Min" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Min%>"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_ETIME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_ETIME%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="ddl_hours2" runat="server" ClientIDMode="Static"></asp:Label>
                      
                            <asp:Label ID="lb_Hour2" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Hour%>"></asp:Label>
                            <asp:Label ID="ddl_minutes2" runat="server" ClientIDMode="Static"></asp:Label>
                           
                            <asp:Label ID="lb_Min2" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Min%>"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_TOTAL_TIME_APPROVE" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_TOTAL_TIME%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <!--	0 日 8 時 0 分	-->
                            <asp:Label ID="txt_DATE" runat="server" MaxLength="2" Width="30px" ClientIDMode="Static"></asp:Label>日
                            <asp:Label ID="txt_HOUR" runat="server" MaxLength="2" Width="30px" ClientIDMode="Static" Text=""></asp:Label>時
                            <asp:Label ID="txt_MINUTE" runat="server" MaxLength="2" Width="30px" ClientIDMode="Static"></asp:Label>分
                        </td>
                    </tr>
                     <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_FACT_HAPPEN_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_FACT_HAPPEN_DT%>"></asp:Label>:
                        </th>

                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_FACT_HAPPEN_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_OVERTIME_DT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="4">
                            <asp:Label ID="txt_APPLY_OVERTIME_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:Label>
                        </td>

                    </tr>
                    <tr height="50px">
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEAVE_REASON" runat="server" TextMode="MultiLine" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_REASON%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="12 ">
                            <asp:Label ID="txt_LEAVE_REASON" runat="server" TextMode="MultiLine" Rows="3" MaxLength="6" Columns="80" BorderWidth="0" ClientIDMode="Static" Enabled="false"></asp:Label>

                        </td>
                    </tr>

                    <tr>
                         <%-- 核淮日期 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IFLOW_APPROVE_DT%>"></asp:Label>
                            :
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_IFLOW_APPROVE_DT" runat="server" Text="" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:Label>
                        </td>
                         <%-- 發薪日期 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SALARY_GIVE_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SALARY_GIVE_DT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_SALARY_GIVE_DT" runat="server" Text=""  MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:Label>
                        </td>
                         <%-- 申請單號 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IFLOW_NO%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_IFLOW_NO" runat="server" Text="" BorderWidth="0" MaxLength="15" ClientIDMode="Static"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <%-- 刷卡比對狀態 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CHECK_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_CHECK_STATUS%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_CHECK_STATUS" runat="server" ClientIDMode="Static"></asp:Label>
                        </td>
                         <%-- 計薪狀態 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SALARY_SETTLE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SALARY_SETTLE_STATUS%>"></asp:Label>
                            :
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_SALARY_SETTLE_STATUS" runat="server" BorderWidth="0" ClientIDMode="Static"  Width="81px"></asp:Label>
                        </td>
                        <%-- 表單狀態 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_FORM_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_FORM_STATUS%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_FORM_STATUS" runat="server" BorderWidth="0"  Width="81px" ClientIDMode="Static"></asp:Label>
                        </td>
                    </tr>

                    <tr height="50px">
                        <%-- 備註 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" TextMode="MultiLine" Text="<%$Resources:Resource,wfb2dh_lb_REMARK%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="12 ">
                            <asp:Label ID="txt_REMARK" runat="server" TextMode="MultiLine" MaxLength="6" Columns="80" ClientIDMode="Static" Width="182px"></asp:Label>

                        </td>
                    </tr>

                    <tr>
                        <td align="right" colspan="10">
                            <asp:Button ID="WFB2DH0400Cancel" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Back%>" OnClick="WFB2DH0400Cancel_Click" /></td>
                    </tr>

                </tbody>


            </table>
            <asp:HiddenField ID="hid_DEPT_NO" runat="server" ClientIDMode="Static" />
            <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />--%>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
