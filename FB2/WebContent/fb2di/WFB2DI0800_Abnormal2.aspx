<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0800_Abnormal2.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0800_Abnormal2" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <title></title>
    <script type="text/javascript" src="../../Scripts/Basic.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {

            $("#txt_DEPT_NAME").attr("readonly", true);
            $("#txt_TARGET_TYPE").attr("readonly", true);
            $("#txt_YM").attr("readonly", true);
            gridviewScroll();

        }

        function gridviewScroll() {


            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1040",
                height: "400",
                barcolor: "#7F7F7F"
            });


            $('#<%=gv_result2.ClientID%>').gridviewScroll({
                width: "1040",
                height: "400",
                barcolor: "#7F7F7F"
            });

            $('#<%=gv_result3.ClientID%>').gridviewScroll({
                width: "1040",
                height: "400",
                barcolor: "#7F7F7F"
            });

        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
            <colgroup>
                <col width="10%" />
                <col width="20%" />
                <col width="10%" />
                <col width="20%" />
                <col width="20%" />
                <col width="20%" />

            </colgroup>
            <tbody style="height: 25px;" valign="bottom">
                <tr>
                    <th align="left" class="Body_TableHeader">
                        <asp:Label ID="lb_DEPT_NAME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NAME%>"></asp:Label>:</th>
                    <td align="left" class="Body_label">
                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <th align="left" class="Body_TableHeader">
                        <asp:Label ID="lb_TARGET_TYPE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_TARGET_TYPE%>"></asp:Label>:</th>
                    <td align="left" class="Body_label">
                        <asp:TextBox ID="txt_TARGET_TYPE" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <th align="left" class="Body_TableHeader">
                        <asp:Label ID="lb_YM" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CALENDAR_YM%>"></asp:Label>:</th>
                    <td align="left" class="Body_label">
                        <asp:TextBox ID="txt_YM" runat="server" MaxLength="5" Width="81px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" height="1" colspan="10">
                        <hr>
                    </td>
                </tr>

            </tbody>
        </table>
        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
            <colgroup>
                <col width="100%" />
            </colgroup>
            <tbody>
                <tr>
                    <td align="center" class="Body_label">
                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_overtime%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="Body_label">
                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_weekdays_over4%>"></asp:Label>:
                        <asp:Label ID="lb_gv_result" runat="server"  ForeColor="red"  Text=""></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <%--平日＞4小時   --%>
        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getAb2Data1"
            SelectCountMethod="getAb2Count1" TypeName="CFB2DI0800DAO" EnablePaging="True"
            SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
            OnSelected="ods1_Selected">
            <SelectParameters>
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:QueryStringParameter QueryStringField="dept_no" Name="dept_no" ConvertEmptyStringToNull="false" Type="String" />
                <asp:QueryStringParameter QueryStringField="target_type" Name="target_type" ConvertEmptyStringToNull="false" Type="String" />
                <asp:QueryStringParameter QueryStringField="ym" Name="ym" ConvertEmptyStringToNull="false" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
            OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1015px"
            OnPageIndexChanging="gv_result_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="DEPT_NO" HeaderText="<%$Resources:Resource,wfb2di_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2di_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="180px"  ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2di_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="185px"  ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="APPLY_OVERTIME_DT" HeaderText="<%$Resources:Resource,wfb2di_APPLY_LEAVE_SDATE%>" SortExpression="APPLY_OVERTIME_DT" HeaderStyle-Width="175px"  ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="APPROVE_OVERTIME_HOUR" HeaderText="<%$Resources:Resource,wfb2di_TOTAL_TIME_APPROVE2%>" SortExpression="APPROVE_OVERTIME_HOUR" HeaderStyle-Width="175px"  ItemStyle-HorizontalAlign="Right"/>
                
            </Columns>

            <HeaderStyle CssClass="GridviewScrollHeader" />
            <PagerStyle CssClass="GridviewScrollPager" />
            <FooterStyle HorizontalAlign="Center" />
            <EditRowStyle HorizontalAlign="Center" />
        </asp:GridView>
        <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
            <colgroup>
                <col width="100%" />
            </colgroup>
            <tbody>
                <tr>
                    <td align="left" class="Body_label">
                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_holiday_over12%>"></asp:Label>:
                        <asp:Label ID="lb_gv_result2" runat="server"  ForeColor="red"  Text=""></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <%--假日＞12小時   --%>
        <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="getAb2Data2"
            SelectCountMethod="getAb2Count2" TypeName="CFB2DI0800DAO" EnablePaging="True"
            SortParameterName="sortExpression" OnSelecting="obs2_Selecting"
            OnSelected="ods2_Selected">
            <SelectParameters>
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:QueryStringParameter QueryStringField="dept_no" Name="dept_no" ConvertEmptyStringToNull="false" Type="String" />
                <asp:QueryStringParameter QueryStringField="target_type" Name="target_type" ConvertEmptyStringToNull="false" Type="String" />
                <asp:QueryStringParameter QueryStringField="ym" Name="ym" ConvertEmptyStringToNull="false" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result2_Sorting"
            OnRowDataBound="gv_result2_RowDataBound" OnRowCreated="gv_result2_RowCreated" Width="1015px"
            OnPageIndexChanging="gv_result2_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>" HeaderStyle-Width="50px" />
                <asp:BoundField DataField="DEPT_NO" HeaderText="<%$Resources:Resource,wfb2di_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2di_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2di_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="185px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="APPLY_OVERTIME_DT" HeaderText="<%$Resources:Resource,wfb2di_APPLY_LEAVE_SDATE%>" SortExpression="APPLY_OVERTIME_DT" HeaderStyle-Width="175px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="APPROVE_OVERTIME_HOUR" HeaderText="<%$Resources:Resource,wfb2di_TOTAL_TIME_APPROVE2%>" SortExpression="APPROVE_OVERTIME_HOUR" HeaderStyle-Width="175px" ItemStyle-HorizontalAlign="Right"/>
                
            </Columns>

            <HeaderStyle CssClass="GridviewScrollHeader" />
            <PagerStyle CssClass="GridviewScrollPager" />
            <FooterStyle HorizontalAlign="Center" />
            <EditRowStyle HorizontalAlign="Center" />
        </asp:GridView>
        <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
            <colgroup>
                <col width="100%" />
            </colgroup>
            <tbody>
                <tr>
                    <td align="left" class="Body_label">
                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_exceeding_limit%>"></asp:Label>:
                        <asp:Label ID="lb_gv_result3" runat="server"  ForeColor="red"  Text=""></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <%--超過管制上限時數  --%>
        <asp:ObjectDataSource ID="ods3" runat="server" SelectMethod="getAb2Data3"
            SelectCountMethod="getAb2Count3" TypeName="CFB2DI0800DAO" EnablePaging="True"
            SortParameterName="sortExpression" OnSelecting="obs3_Selecting"
            OnSelected="ods3_Selected">
            <SelectParameters>
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:QueryStringParameter QueryStringField="dept_no" Name="dept_no" ConvertEmptyStringToNull="false" Type="String" />
                <asp:QueryStringParameter QueryStringField="target_type" Name="target_type" ConvertEmptyStringToNull="false" Type="String" />
                <asp:QueryStringParameter QueryStringField="ym" Name="ym" ConvertEmptyStringToNull="false" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:GridView ID="gv_result3" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result3_Sorting"
            OnRowDataBound="gv_result3_RowDataBound" OnRowCreated="gv_result3_RowCreated" Width="1020px"
            OnPageIndexChanging="gv_result3_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>" HeaderStyle-Width="50px" />
                <asp:BoundField DataField="DEPT_NO" HeaderText="<%$Resources:Resource,wfb2di_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2di_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2di_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="185px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="OVERTIME_CTL_CD" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_CTL_CD%>" SortExpression="OVERTIME_CTL_CD" HeaderStyle-Width="175px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="APPROVE_OVERTIME_HOUR" HeaderText="<%$Resources:Resource,wfb2di_TOTAL_TIME_APPROVE2%>" HeaderStyle-Width="175px"  ItemStyle-HorizontalAlign="Right"/>
                
            </Columns>

            <HeaderStyle CssClass="GridviewScrollHeader" />
            <PagerStyle CssClass="GridviewScrollPager" />
            <FooterStyle HorizontalAlign="Center" />
            <EditRowStyle HorizontalAlign="Center" />
        </asp:GridView>
        <asp:HiddenField ID="HID_PageRow3" runat="server" ClientIDMode="Static" />
    </form>
</body>
</html>
