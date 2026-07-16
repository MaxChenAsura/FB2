<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0100_OtherPjob.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0100_OtherPjob" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../../Scripts/Basic.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {

            gridviewScroll();


        }

        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "400",
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
        <br />

        <table width="600px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">
            <tr>
                <td align="left">
                    【兼任】
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getOtherJobData"
                        SelectCountMethod="getOtherJobTrainCount" TypeName="CFB2HB0100DAO" EnablePaging="True"
                        SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                        OnSelected="ods1_Selected">
                        <SelectParameters>
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:QueryStringParameter QueryStringField="emp_id" ConvertEmptyStringToNull="false" Name="emp_id" Type="String" DefaultValue="" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="false" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="400px"
                        OnPageIndexChanging="gv_result_PageIndexChanging">
                        <Columns>

                            <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="40px" HtmlEncode="false" />
                            <asp:BoundField DataField="ORI_DIV_DEPT_FULL_NAME" HeaderText="<%$Resources:Resource,wfb2hb_ORI_DEPT_NO%>" SortExpression="ORI_DEPT_NO" HeaderStyle-Width="320px" ItemStyle-HorizontalAlign="Left" HtmlEncode="false" />
                            <asp:BoundField DataField="OTHER_PJOB_DESC" HeaderText="<%$Resources:Resource,wfb2hb_OTHER_PJOB_CD2%>" SortExpression="OTHER_PJOB_CD" HeaderStyle-Width="100px" HtmlEncode="false" ItemStyle-HorizontalAlign="Left"/>

                        </Columns>
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <FooterStyle CssClass="GridviewScrollPager" />
                    </asp:GridView>
                    <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
