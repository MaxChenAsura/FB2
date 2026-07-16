<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2se/WFB2SE1300_SubDtl3.aspx.cs" Inherits="WebContent_fb2se_WFB2SE1300_SubDtl3" Culture="auto" UICulture="auto"%>
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
                barcolor: "#7F7F7F"

            });


        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData3"
            SelectCountMethod="getCount3" TypeName="CFB2SE1300DAO" EnablePaging="True"
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
            OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1000px"
            OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound" >
            <Columns>
                <asp:CheckBoxField DataField="IS_APPROVE_MARK" HeaderText="<%$Resources:Resource,wfb2se_APPROVE_MARK%>" SortExpression="IS_APPROVE_MARK" HeaderStyle-Width="70px"/>
                <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>"  HeaderStyle-Width="40px"/>
                <asp:BoundField DataField="LEVEL_CD" HeaderText="<%$Resources:Resource,wfb2se_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="PJOB_NAME" HeaderText="<%$Resources:Resource,wfb2se_PJOB_TYPE%>" SortExpression="PJOB_TYPE" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="EXAMINE_S" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_S%>" SortExpression="EXAMINE_S" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_A" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_A%>" SortExpression="EXAMINE_A" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_B" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_B%>" SortExpression="EXAMINE_B" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_C" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_C%>" SortExpression="EXAMINE_C" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_D" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_D%>" SortExpression="EXAMINE_D" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_E" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_E%>" SortExpression="EXAMINE_E" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_F" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_F%>" SortExpression="EXAMINE_F" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_G" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_G%>" SortExpression="EXAMINE_G" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_H" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_H%>" SortExpression="EXAMINE_H" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_I" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_I%>" SortExpression="EXAMINE_I" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="EXAMINE_J" DataFormatString="{0:N0}" HeaderText="<%$Resources:Resource,wfb2se_EXAMINE_J%>" SortExpression="EXAMINE_J" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"/>
            </Columns>
            <PagerStyle CssClass="GridviewScrollPager" />
            <FooterStyle CssClass="GridviewScrollPager" />
            <EmptyDataTemplate>
            <table><tr>
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
