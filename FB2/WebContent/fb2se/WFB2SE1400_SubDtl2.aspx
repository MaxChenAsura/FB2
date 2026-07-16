<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2se/WFB2SE1400_SubDtl2.aspx.cs" Inherits="WebContent_fb2se_WFB2SE1400_SubDtl2" Culture="auto" UICulture="auto"%>
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
                width: "1000",
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
        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData2"
            SelectCountMethod="getCount2" TypeName="CFB2SE1400DAO" EnablePaging="True"
            SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
            OnSelected="ods1_Selected">
            <SelectParameters>
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:QueryStringParameter QueryStringField="qdatakey" ConvertEmptyStringToNull="false" Name="qdatakey" Type="String" DefaultValue="" />

            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
            OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="980px"
            OnPageIndexChanging="gv_result_PageIndexChanging" >
            <Columns>

                <asp:CheckBoxField DataField="IS_APPROVE_MARK" HeaderText="<%$Resources:Resource,wfb2se_APPROVE_MARK%>" SortExpression="" HeaderStyle-Width="20px"/>
                <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" HeaderStyle-Width="10px"/>
                <asp:BoundField DataField="LEVEL_CD" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="GRADE_CD" HeaderText="<%$Resources:Resource,wfb2se_GRADE_CD%>" SortExpression="GRADE_CD" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="EXAMINE_A" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_A%>" SortExpression="EXAMINE_A" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_B" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_B%>" SortExpression="EXAMINE_B" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_C" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_C%>" SortExpression="EXAMINE_C" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_D" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_D%>" SortExpression="EXAMINE_D" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_E" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_E%>" SortExpression="EXAMINE_E" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="ABILITY_ADJ" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_ABILITY_ADJ%>" SortExpression="ABILITY_ADJ" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="LEVEL_ADJ" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_ADJ%>" SortExpression="LEVEL_ADJ" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="LEVEL_PAY_LOW" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_LOW%>" SortExpression="LEVEL_PAY_LOW" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="LEVEL_PAY_AVG" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_AVG%>" SortExpression="LEVEL_PAY_AVG" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="LEVEL_PAY_UP" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_PAY_UP%>" SortExpression="LEVEL_PAY_UP" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Right"/>

            </Columns>
            <EmptyDataTemplate>
            <table><tr>
                <td>
                    <asp:Literal ID="lit_noda" runat="server" Text="<%$Resources:Resource,wfd2se_nodata%>"></asp:Literal>
                
                </td>
                   </tr>
            </table>
            </EmptyDataTemplate>
            <PagerStyle CssClass="GridviewScrollPager" />
            <FooterStyle CssClass="GridviewScrollPager" />
        </asp:GridView>
        <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

    </form>
</body>
</html>
