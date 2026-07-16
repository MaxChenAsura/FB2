<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0800_Abnormal1.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0800_Abnormal1" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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


        <span>
            <br>
        </span>
        <!----------------------------------------------------------------------------------------------------------------------------->

        <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
            <colgroup>
                <col width="100%" />
            </colgroup>
            <tbody>
                <tr>
                    <td align="center" class="Body_label">
                        <asp:Label ID="lb_GridTitle" runat="server" Text="<%$Resources:Resource,wfb2di_lb_GridTitle%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="Body_label">
                         <asp:Label ID="lb_gv_result" runat="server" ForeColor="red" Text=""></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getAb1Data"
            SelectCountMethod="getAb1Count" TypeName="CFB2DI0800DAO" EnablePaging="True"
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
            OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
            OnPageIndexChanging="gv_result_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>" HeaderStyle-Width="40px" />
                <asp:BoundField DataField="DEPT_NO" HeaderText="<%$Resources:Resource,wfb2di_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="240px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2di_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2di_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="CALENDAR_DT" HeaderText="<%$Resources:Resource,wfb2di_APPLY_LEAVE_SDATE%>" SortExpression="CALENDAR_DT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="SHIFT_DESC" HeaderText="<%$Resources:Resource,wfb2di_SHIFT_CD%>" SortExpression="SHIFT_CD" HeaderStyle-Width="160px" ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="CLOCK_IN_OUT_DT" HeaderText="<%$Resources:Resource,wfb2di_CLOCK_IN_OUT_DT%>" SortExpression="CLOCK_IN_OUT_DT" HeaderStyle-Width="180px"  ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="VIOLATE_BEFORE_HOUR" HeaderText="<%$Resources:Resource,wfb2di_VIOLATE_BEFORE_HOUR%>" SortExpression="VIOLATE_BEFORE_HOUR" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="VIOLATE_AFTER_HOUR" HeaderText="<%$Resources:Resource,wfb2di_VIOLATE_AFTER_HOUR%>" SortExpression="VIOLATE_AFTER_HOUR" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" />

            </Columns>

            <HeaderStyle CssClass="GridviewScrollHeader" />
            <PagerStyle CssClass="GridviewScrollPager" />
            <FooterStyle HorizontalAlign="Center" />
            <EditRowStyle HorizontalAlign="Center" />
        </asp:GridView>

        <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    </form>
</body>
</html>
