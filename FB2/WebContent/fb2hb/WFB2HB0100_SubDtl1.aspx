<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0100_SubDtl1.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0100_SubDtl1" Culture="auto" UICulture="auto"%>
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
        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getFamData"
            SelectCountMethod="getFamCount" TypeName="CFB2HB0100DAO" EnablePaging="True"
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
            OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1200px"
            OnPageIndexChanging="gv_result_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" SortExpression="RowNumber"  HtmlEncode="false" />
                <asp:BoundField DataField="FAMILY_NATION_DESC" HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_NATION_CD%>" SortExpression="FAMILY_NATION_CD"  HtmlEncode="false" />
                <asp:BoundField DataField="FAMILY_SEX_DESC" HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_SEX_CD%>" SortExpression="FAMILY_SEX_CD"  HtmlEncode="false" />
                <asp:BoundField DataField="FAMILY_LICENSE_ID" HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_LICENSE_ID%>" SortExpression="FAMILY_LICENSE_ID" HtmlEncode="false" />
                <asp:BoundField DataField="FAMILY_PASSPORT_ID" HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_PASSPORT_ID%>" SortExpression="FAMILY_PASSPORT_ID" HtmlEncode="false" />
                <asp:BoundField DataField="FAMILY_NAME" HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_NAME%>" SortExpression="FAMILY_NAME"  HtmlEncode="false" />
                <asp:BoundField DataField="FAMILY_RELATION_DESC" HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_RELATION%>" SortExpression="FAMILY_RELATION" HtmlEncode="false" />
                <asp:BoundField DataField="FAMILY_BIRTH_DT" HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_BIRTH_DT%>" SortExpression="FAMILY_BIRTH_DT" HtmlEncode="false" />
                <asp:BoundField DataField="FAMILY_WORK_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_FAMILY_WORK_DESC%>" SortExpression="FAMILY_WORK_DESC"  ItemStyle-HorizontalAlign="Left" HtmlEncode="false" />
                <asp:TemplateField  HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_ALLOWANCE%>">
                    <ItemTemplate>
                        <asp:CheckBox ID="cb_IS_ALLOWANCE" runat="server" />
                        <asp:HiddenField ID="hid_IS_ALLOWANCE" runat="server" Value='<%#Bind("IS_ALLOWANCE")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField  HeaderText="<%$Resources:Resource,wfb2hb_lb_BENEFICIARY%>">
                    <ItemTemplate>
                        <asp:CheckBox ID="cb_BENEFICIARY" runat="server" />
                        <asp:HiddenField ID="hid_BENEFICIARY" runat="server" Value='<%#Bind("BENEFICIARY")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="VENDOR_ID" HeaderText="<%$Resources:Resource,wfb2hb_lb_VENDOR_ID%>" SortExpression="VENDOR_ID" ItemStyle-HorizontalAlign="Left" HtmlEncode="false" />
                <asp:TemplateField  HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_VALID%>">
                    <ItemTemplate>
                        <asp:CheckBox ID="cb_IS_VALID" runat="server" />
                        <asp:HiddenField ID="hid_IS_VALID" runat="server" Value='<%#Bind("IS_VALID")%>' />
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
