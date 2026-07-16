<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0500_Dtl.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0500_Dtl" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date1").datepicker({ dateFormat: 'yy/mm' })
            $(".PROFESSION_ALLOWANCE").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });
            $(".MANAGEMENT_ALLOWANCE").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });

            $(".number2").mask("999");

            $.unblockUI();
        }//月曆

        function ShowRecord(obj) {

            $("#HID_DEPT_NO").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
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
                            <asp:Label ID="txt_EMP_ID" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static" AutoPostBack="true"></asp:Label>

                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DEPT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_DEPT_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:Label>
                        </td>

                    </tr>

                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label1" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static">請假實績:</asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_MAIN_LEAVE_CD" runat="server" MaxLength="10" Width="200px" ClientIDMode="Static" AutoPostBack="true" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SUB_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <%--<asp:Textbox ID="ddl_SUB_LEAVE_CD" runat="server" ClientIDMode="Static" BorderWidth="0" ReadOnly="true" BackColor="#ffffff"  ForeColor="#000000" Font-Bold="True"></asp:Textbox>--%>
                            <asp:Label ID="ddl_SUB_LEAVE_CD" runat="server" ClientIDMode="Static"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEAVE_TIME_UNIT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_TIME_UNIT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lb_LEAVE_MIN_VALUE" Text="" runat="server" Width="30px" MaxLength="10" ClientIDMode="Static"></asp:Label>
                            <asp:Label ID="txt_LEAVE_TIME_UNIT" Text="" runat="server" Width="80px" MaxLength="10" ClientIDMode="Static"></asp:Label>
                            <asp:HiddenField ID="HID_LEAVE_MIN_VALUE" runat="server" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_SDT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_SDT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <%--<asp:Textbox ID="txt_APPLY_LEAVE_SDT"  runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" BorderWidth="0" ReadOnly="true" BackColor="#ffffff"  ForeColor="#000000" Font-Bold="True"></asp:Textbox>--%>
                            <asp:Label ID="txt_APPLY_LEAVE_SDT" runat="server" Width="81px" ClientIDMode="Static"></asp:Label>
                        </td>
                        
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_EDT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_EDT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="3">
                            <%--<asp:Textbox ID="txt_APPLY_LEAVE_EDT" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" BorderWidth="0" ReadOnly="true" BackColor="#ffffff"  ForeColor="#000000" Font-Bold="True"></asp:Textbox>--%>
                            <asp:Label ID="txt_APPLY_LEAVE_EDT" runat="server" Width="81px" ClientIDMode="Static"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                       <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_STIME" runat="server" Text="<%$Resources:Resource,wfb2dh_APPLY_LEAVE_STIME1%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <%--                    <asp:Textbox ID="ddl_APPLY_LEAVE_STIME_H" runat="server" Width="20px" ClientIDMode="Static" BorderWidth="0" ReadOnly="true" BackColor="#ffffff"  ForeColor="#000000" Font-Bold="True"></asp:Textbox>時
                            <asp:Textbox ID="ddl_APPLY_LEAVE_STIME_M" runat="server" Width="20px" ClientIDMode="Static"  BorderWidth="0" ReadOnly="true" BackColor="#ffffff"  ForeColor="#000000" Font-Bold="True"></asp:Textbox>分--%>
                            <asp:Label ID="ddl_APPLY_LEAVE_STIME_H" runat="server" ClientIDMode="Static"></asp:Label>時
                    <asp:Label ID="ddl_APPLY_LEAVE_STIME_M" runat="server" ClientIDMode="Static"></asp:Label>分
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_ETIME" runat="server" Text="<%$Resources:Resource,wfb2dh_APPLY_LEAVE_ETIME1%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <%--                    <asp:Textbox ID="ddl_APPLY_LEAVE_ETIME_H" runat="server" ClientIDMode="Static" Width="20px" BorderWidth="0" ReadOnly="true" BackColor="#ffffff"  ForeColor="#000000" Font-Bold="True"></asp:Textbox>時
                            <asp:Textbox ID="ddl_APPLY_LEAVE_ETIME_M" runat="server" ClientIDMode="Static"  Width="20px" BorderWidth="0" ReadOnly="true" BackColor="#ffffff"  ForeColor="#000000" Font-Bold="True"></asp:Textbox>分--%>
                            <asp:Label ID="ddl_APPLY_LEAVE_ETIME_H" runat="server" ClientIDMode="Static"></asp:Label>時
                    <asp:Label ID="ddl_APPLY_LEAVE_ETIME_M" runat="server" ClientIDMode="Static"></asp:Label>分
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_TOTAL_TIME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_TOTAL_TIME%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_DATE" runat="server" MaxLength="2" Width="30px" ClientIDMode="Static"></asp:Label>日
                            <asp:Label ID="txt_HOUR" runat="server" MaxLength="2" Width="30px" ClientIDMode="Static" Text=""></asp:Label>時
                            <asp:Label ID="txt_MINUTE" runat="server" MaxLength="2" Width="30px" ClientIDMode="Static"></asp:Label>分
                    <asp:HiddenField ID="hid_nn_total" runat="server" ClientIDMode="Static" />
                        </td>

                    </tr>
                     <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_FACT_HAPPEN_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_FACT_HAPPEN_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_FACT_HAPPEN_DT" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_OVERTIME_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="3">
                            <asp:Label ID="txt_APPLY_OVERTIME_DT" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" Enabled="false"></asp:Label>
                        </td>

                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEAVE_REASON" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_REASON%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:TextBox ID="txt_LEAVE_REASON" runat="server" Width="580px" TextMode="MultiLine" ClientIDMode="Static" BorderWidth="0" ReadOnly="true" BackColor="#ffffff" ForeColor="#000000" Font-Bold="True" Style="overflow: auto"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_APPROVE_DT1" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IFLOW_APPROVE_DT1%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_IFLOW_APPROVE_DT1" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PAY_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_PAY_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_PAY_DT" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IFLOW_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_IFLOW_NO" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" ></asp:Label>
                        </td>

                    </tr>

                    <tr>
                        <%-- 刷卡比對狀態 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CHECK_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_CHECK_STATUS%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_CHECK_STATUS" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static"></asp:Label>
                        </td>
                        <%-- 計薪狀態 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SALARY_SETTLE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SALARY_SETTLE_STATUS%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_SALARY_SETTLE_STATUS" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" Height="16px"></asp:Label>
                        </td>
                        <%-- 表單狀態 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_FORM_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_FORM_STATUS%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_FORM_STATUS" runat="server" MaxLength="8" Width="81px" ClientIDMode="Static" ></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <%-- 備註 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_REMARK%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:TextBox ID="txt_REMARK" runat="server" Width="580px" TextMode="MultiLine" ClientIDMode="Static" BorderWidth="0" ReadOnly="true" BackColor="#ffffff" ForeColor="#000000" Font-Bold="True" Style="overflow: auto"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td align="right" class="Body_label" colspan="6"></td>
                        <td>
                            <asp:Button ID="WFB2DH0500Cancel" runat="server" Text="<%$Resources:Resource,btn_back%>" OnClick="WFB2DH0500Cancel_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:HiddenField ID="HID_DEPT_NO" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
