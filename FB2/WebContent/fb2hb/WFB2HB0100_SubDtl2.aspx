<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0100_SubDtl2.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0100_SubDtl2" Culture="auto" UICulture="auto"%>
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
                width: "1020",
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
        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getEduData"
            SelectCountMethod="getEduCount" TypeName="CFB2HB0100DAO" EnablePaging="True"
            SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
            OnSelected="ods1_Selected">
            <SelectParameters>
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:QueryStringParameter QueryStringField="emp_id" ConvertEmptyStringToNull="false" Name="emp_id" Type="String" DefaultValue="" />

            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
            OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1000px"
            OnPageIndexChanging="gv_result_PageIndexChanging">
            <Columns>

                <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="20px" HtmlEncode="false" />
                <asp:BoundField DataField="SCHOOL_NATION_DESC" HeaderText="<%$Resources:Resource,wfb2hb_SCHOOL_NATION_CD%>" SortExpression="SCHOOL_NATION_CD" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" />
                <asp:BoundField DataField="EDUCATION_DESC" HeaderText="<%$Resources:Resource,wfb2hb_EDUCATION_CD%>" SortExpression="EDUCATION_CD" HeaderStyle-Width="40px" HtmlEncode="false" />
                <asp:BoundField DataField="SCHOOL_NAME" HeaderText="<%$Resources:Resource,wfb2hb_SCHOOL_NAME%>" SortExpression="SCHOOL_NAME" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" HtmlEncode="false" />
                <asp:BoundField DataField="DEPARTMENT_NAME" HeaderText="<%$Resources:Resource,wfb2hb_lb_DEPARTMENT_NAME%>" SortExpression="DEPARTMENT_NAME" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" HtmlEncode="false" />
                <asp:BoundField DataField="GRADUATION_YEAR" HeaderText="<%$Resources:Resource,wfb2hb_lb_GRADUATION_YEAR%>" SortExpression="GRADUATION_YEAR" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" />
                <asp:TemplateField HeaderStyle-Width="40px" HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_SALARY_SCHOOL%>">
                    <ItemTemplate>
                        <asp:CheckBox ID="cb_IS_SALARY_SCHOOL" runat="server" />
                        <asp:HiddenField ID="hid_IS_SALARY_SCHOOL" runat="server" Value='<%#Bind("IS_SALARY_SCHOOL")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-Width="40px" HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_VIRTUAL_SCHOOL%>">
                    <ItemTemplate>
                        <asp:CheckBox ID="cb_IS_VIRTUAL_SCHOOL" runat="server" />
                        <asp:HiddenField ID="hid_IS_VIRTUAL_SCHOOL" runat="server" Value='<%#Bind("IS_VIRTUAL_SCHOOL")%>' />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
            <PagerStyle CssClass="GridviewScrollPager" />
            <FooterStyle CssClass="GridviewScrollPager" />
        </asp:GridView>
        <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
    </form>
</body>
</html>
