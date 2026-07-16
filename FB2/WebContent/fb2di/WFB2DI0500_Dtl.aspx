<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0500_Dtl.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0500_Dtl" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $.unblockUI();
        }

        function openQry() {
            window.location.href("WFB2DI0500_Qry.aspx");
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <table cellspacing="3" cellpadding="0" width="1020">
        <tr>
            <td colspan="3">
                <table cellspacing="1" cellpadding="1" width="1020">
                    <!--======================================================================================-->
                    <tr>
                        <td align="right">
                            <div id="mrm" style="display: inline; overflow: auto; width: 974px;">

                                <table cellspacing="1" cellpadding="1" width="1020" border="0">
                                    <colgroup>
                                        <col width="11%" />
                                        <col width="15%" />
                                        <col width="11%" />
                                        <col width="15%" />
                                        <col width="11%" />
                                        <col width="15%" />
                                        <col width="11%" />
                                        <col width="11%" />
                                    </colgroup>

                                    <tbody>

                                        <tr>
                                            <%-- 工號 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_ID%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_EMP_ID" runat="server" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <%--姓名 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_NAME%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <%--部門 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_DEPT_NO" runat="server" BorderWidth="0" Width="140px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <%--加班管制對象 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_OVERTIME_CTL_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_CTL_CD2%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_OVERTIME_CTL_CD" runat="server" BorderWidth="0" Width="140px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%-- 勤務日期 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_DT3%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_APPLY_OVERTIME_DT" runat="server" CssClass="date" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <%-- 勤務日期類型 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_DT_TYPE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DT_TYPE2%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_DT_TYPE" runat="server" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <%-- 班別 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_SHIFT_CD%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label" colspan="2">
                                                <asp:TextBox ID="txt_SHIFT_CD" runat="server" Width="230px" ClientIDMode="Static" BorderWidth="0" Height="30px"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <%-- 加班類型 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_OVERTIME_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_CD%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_OVERTIME_CD" runat="server" ClientIDMode="Static" Enabled="false"></asp:DropDownList>
                                            </td>
                                            <%-- 加班日期類型 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_OVERTIME_DT_TYPE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_DT_TYPE2%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_OVERTIME_DT_TYPE" runat="server" ClientIDMode="Static" BorderWidth="0" Width="144px"></asp:TextBox>
                                            </td>
                                            <%-- 代休假日期 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_REPLACE_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_REPLACE_DT%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_REPLACE_DT" runat="server" BorderWidth="0" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                            </td>
                                            <th align="left"></th>
                                            <td align="left" class="Body_label"></td>
                                        </tr>

                                        <tr>
                                            <%-- 是否申告換休 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_IS_APPLY" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IS_APPLY%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_IS_APPLY" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Enabled="false">
                                                    <asp:ListItem Text="N-否" Value="N"></asp:ListItem>
                                                    <asp:ListItem Text="Y-是" Value="Y"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <%-- 加班特殊狀況 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_O_SPECIAL_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_O_SPECIAL_CD%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_O_SPECIAL_CD" runat="server"  ClientIDMode="Static" Enabled="false"></asp:DropDownList>
                                            </td>

                                        </tr>

                                        <tr>
                                            <%-- 加班事由 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_OVERTIME_REASON" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_REASON%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label" colspan="7">
                                                <asp:TextBox ID="txt_OVERTIME_REASON" TextMode="MultiLine" runat="server" BorderWidth="0" Width="610px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td align="right">
                            <div id="Div1" style="display: inline; overflow: auto; width: 974px;">
                                <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                                    <colgroup>
                                        <col width="11%" />
                                        <col width="15%" />
                                        <col width="11%" />
                                        <col width="15%" />
                                        <col width="11%" />
                                        <col width="15%" />
                                        <col width="11%" />
                                        <col width="11%" />
                                    </colgroup>
                                    <tbody>
                                        <tr>
                                            <%-- 勤前加班起迄 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_BEFORE_TIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BEFORE_TIME2%>"></asp:Label>:
                                            </th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_BEFORE_TIME" runat="server" CssClass="date" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_BEFORE_STIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Enabled="false"></asp:DropDownList>
                                                <asp:Label ID="lb_BEFORE_STIME_HOUR_S" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                                <asp:DropDownList ID="ddl_BEFORE_STIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Enabled="false"></asp:DropDownList>
                                                <asp:Label ID="lb_BEFORE_STIME_MINUTE_S" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                            </td>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_BEFORE_ETIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Enabled="false"></asp:DropDownList>
                                                <asp:Label ID="lb_BEFORE_ETIME_HOUR_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                                <asp:DropDownList ID="ddl_BEFORE_ETIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Enabled="false"></asp:DropDownList>
                                                <asp:Label ID="lb_BEFORE_ETIME_MINUTE_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label></td>
                                            <%-- 勤前時數 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_BEFORE_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BEFORE_HOUR%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_BEFORE_HOUR" runat="server" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                            </td>
                                            <td>&nbsp</td>
                                        </tr>

                                        <tr>
                                            <%-- 勤後加班起迄 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_AFTER_TIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_AFTER_TIME2%>"></asp:Label>:
                                            </th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_AFTER_TIME" runat="server" CssClass="date" Width="100px" ClientIDMode="Static"  BorderWidth="0"></asp:TextBox>
                                            </td>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_AFTER_STIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Enabled="false"></asp:DropDownList>
                                                <asp:Label ID="lb_AFTER_STIME_HOUR_S" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                                <asp:DropDownList ID="ddl_AFTER_STIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Enabled="false"></asp:DropDownList>
                                                <asp:Label ID="lb_AFTER_STIME_MINUTES" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                            </td>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_AFTER_ETIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Enabled="false"></asp:DropDownList>
                                                <asp:Label ID="lb_AFTER_ETIME_HOUR_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                                <asp:DropDownList ID="ddl_AFTER_ETIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Enabled="false"></asp:DropDownList>
                                                <asp:Label ID="lb_AFTER_ETIME_MINUTE_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                            </td>
                                            <%-- 勤後時數 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_AFTER_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_AFTER_HOUR%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_AFTER_HOUR" runat="server" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%-- 出差時間起迄 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_TRIP_TIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_TRIP_TIME%>"></asp:Label>:
                                            </th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_TRIP_TIME" runat="server" CssClass="date"  BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_TRIP_STIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Enabled="false"></asp:DropDownList>
                                                <asp:Label ID="lb_TRIP_STIME_H" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                                <asp:DropDownList ID="ddl_TRIP_STIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Enabled="false"></asp:DropDownList>
                                                <asp:Label ID="lb_TRIP_STIME_M" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                            </td>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_TRIP_ETIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Enabled="false"></asp:DropDownList>
                                                <asp:Label ID="lb_TRIP_ETIME_H" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                                <asp:DropDownList ID="ddl_TRIP_ETIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Enabled="false"></asp:DropDownList>
                                                <asp:Label ID="lb_TRIP_ETIME_M" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                            </td>
                                            <%-- 出差時數 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_TRIP_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_TRIP_HOUR%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_TRIP_HOUR" runat="server" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%-- 申請總時數 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_APPLY_OVERTIME_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_HOUR%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_APPLY_OVERTIME_HOUR" runat="server" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                            </td>
                                            <%-- 核准總時數 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_APPROVE_OVERTIME_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPROVE_OVERTIME_HOUR%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_APPROVE_OVERTIME_HOUR" runat="server" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                            </td>
                                            <%-- 計算總時數 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_OVERTIME_PAY_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_OVERTIME_PAY_HOUR2%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_OVERTIME_PAY_HOUR" runat="server" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%-- 三高累計時數 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_HYPER_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HYPER_HOUR%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_HYPER_HOUR" runat="server" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                            </td>
                                            <%-- 一般累計時數 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_NORMAL_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_NORMAL_HOUR%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_NORMAL_HOUR" runat="server" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                            </td>
                                            <%-- 可換休時數 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_EXCHANGE_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EXCHANGE_HOUR%>"></asp:Label>:</th>
                                            <td align="left" class="Body_Label">
                                                <asp:TextBox ID="txt_EXCHANGE_HOUR" runat="server" BorderWidth="0" Width="54px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td align="right">
                            <div id="Div2" style="display: inline; overflow: auto; width: 974px;">
                                <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                                    <colgroup>
                                        <col width="11%" />
                                        <col width="15%" />
                                        <col width="11%" />
                                        <col width="15%" />
                                        <col width="11%" />
                                        <col width="15%" />
                                        <col width="11%" />
                                        <col width="11%" />
                                    </colgroup>
                                    <tbody>
                                        <tr>
                                            <%-- 核准日期 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_IFLOW_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IFLOW_APPROVE_DT_2%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_IFLOW_APPROVE_DT" runat="server" Width="100px" ClientIDMode="Static" CssClass="date" BorderWidth="0" ></asp:TextBox>
                                            </td>
                                            <%-- 表單狀態 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_FORM_STATUS" runat="server" Text="<%$Resources:Resource,wfb2di_lb_FORM_STATUS%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_FORM_STATUS" runat="server" BorderWidth="0" Width="50px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <%-- 是否刷卡比對 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_IS_DUTY_CHECK" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IS_DUTY_CHECK%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_IS_DUTY_CHECK" runat="server" BorderWidth="0" Width="50px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%-- 刷卡比對狀態 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_CHECK_STATUS" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CHECK_STATUS%>"></asp:Label>:
                                            </th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_CHECK_STATUS" runat="server" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <%-- 刷卡上班時間 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_CLOCK_IN_TIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CLOCK_IN_TIME%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_CLOCK_IN_TIME" runat="server" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <%-- 刷卡下班時間 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_CLOCK_OUT_TIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CLOCK_OUT_TIME%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_CLOCK_OUT_TIME" runat="server" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%-- 申請單號 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IFLOW_NO%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_IFLOW_NO" runat="server" BorderWidth="0" Width="130px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <%-- 發薪日期 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_PAY_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_SALARY_GIVE_DT%>"></asp:Label>:
                                            </th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_PAY_DT" runat="server" Width="100px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                            </td>
                                            <%-- 計薪狀態 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_SALARY_SETTLE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SALARY_SETTLE_STATUS%>"></asp:Label>:
                                            </th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_SALARY_SETTLE_STATUS" runat="server" BorderWidth="0" Width="81px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%-- 備註 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2di_lb_REMARK%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label" colspan="7">
                                                <asp:TextBox ID="txt_REMARK" TextMode="MultiLine" runat="server" BorderWidth="0" Width="610px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <!--======================================================================================-->
                </table>
            </td>
        </tr>
        <!-- START: Show only Save and Cancel button instead for Add and Edit operation -->
        <tr>
            <td class="Body_PageControl" style="text-align: right">
                <asp:Button ID="btn_Cancel" runat="server" Text="<%$Resources:Resource,wfb2di_btn_back%>" OnClick="btn_Cancel_Click" />
            </td>
        </tr>
        <!-- END: Show only Save and Cancel button instead for Add and Edit operation -->
    </table>

</asp:Content>
