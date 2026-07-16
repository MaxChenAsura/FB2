<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2se/WFB2SE1300_SubDtl1.aspx.cs" Inherits="WebContent_fb2se_WFB2SE1300_SubDtl1" Culture="auto" UICulture="auto" %>

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
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {


            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 6

            });


        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData1"
            SelectCountMethod="getCount1" TypeName="CFB2SE1300DAO" EnablePaging="True"
            SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
            OnSelected="ods1_Selected">
            <SelectParameters>
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:QueryStringParameter QueryStringField="qdatakey" ConvertEmptyStringToNull="false" Name="qdatakey" Type="String" DefaultValue="" />

            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
            OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1475px"
            OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
            <Columns>
                <asp:CheckBoxField DataField="IS_APPROVE_MARK" HeaderText="<%$Resources:Resource,wfb2se_APPROVE_MARK%>" SortExpression="IS_APPROVE_MARK" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                <asp:BoundField DataField="CHG_STATUS" HeaderText="<%$Resources:Resource,wfb2se_CHG_STATUS%>" SortExpression="CHG_STATUS" HeaderStyle-Width="70px" ItemStyle-Width="70px" />
                <asp:BoundField DataField="EFFECT_YM" HeaderText="<%$Resources:Resource,wfb2se_EFFECT_YM%>" SortExpression="EFFECT_YM" HeaderStyle-Width="65px" ItemStyle-Width="65px" ItemStyle-HorizontalAlign="left" />
                <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2se_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2se_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="LEVEL_CD" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="30px" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="GRADE_CD" HeaderText="<%$Resources:Resource,wfb2se_GRADE_CD%>" SortExpression="GRADE_CD" HeaderStyle-Width="30px" ItemStyle-Width="30px" />
                <asp:BoundField DataField="DEPT_NAME_20" HeaderText="<%$Resources:Resource,wfb2se_DEPT_NAME_20%>" SortExpression="DEPT_NAME_20" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="DEPT_NAME_30" HeaderText="<%$Resources:Resource,wfb2se_DEPT_NAME_30%>" SortExpression="DEPT_NAME_30" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="DEPT_NAME_40" HeaderText="<%$Resources:Resource,wfb2se_DEPT_NAME_40%>" SortExpression="DEPT_NAME_40" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="THIS_YEAR_GRADE" HeaderText="<%$Resources:Resource,wfb2se_THIS_YEAR_GRADE%>" SortExpression="THIS_YEAR_GRADE" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="LEVEL_PAY_OLD" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_OLD%>" SortExpression="LEVEL_PAY_OLD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="EXAMINE_ADJ" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_ADJ%>" SortExpression="EXAMINE_ADJ" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="LEVEL_ADJ" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_ADJ%>" SortExpression="LEVEL_ADJ" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="LEVEL_PAY_NEW" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_NEW%>" SortExpression="LEVEL_PAY_NEW" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="ABILITY_PAY_OLD" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_ABILITY_PAY_OLD%>" SortExpression="ABILITY_PAY_OLD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="ABILITY_ADJ" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_ABILITY_ADJ%>" SortExpression="ABILITY_ADJ" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="ABILITY_PAY_NEW" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_ABILITY_PAY_NEW%>" SortExpression="ABILITY_PAY_NEW" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="LEVEL_PAY_DIFF" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_DIFF%>" SortExpression="LEVEL_PAY_DIFF" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                <asp:CheckBoxField DataField="IS_NOPAYDIFF_YN" HeaderText="<%$Resources:Resource,wfb2se_NOPAYDIFF_YN%>" SortExpression="IS_NOPAYDIFF_YN" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
            </Columns>
            <PagerStyle CssClass="GridviewScrollPager" />
            <FooterStyle CssClass="GridviewScrollPager" />
            <EmptyDataTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Literal ID="lit_noda" runat="server" Text="<%$Resources:Resource,wfd2se_nodata%>"></asp:Literal>

                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:GridView>
        <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
            <tr height="100%" valign="top">
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
        <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

    </form>
</body>
</html>
