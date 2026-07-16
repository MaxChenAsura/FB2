<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0400_Qry_Leave1.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0400_Qry_Leave1" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
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
                barcolor: "#7F7F7F",
                freezesize: 2

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");

        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

    </script>

    <style type="text/css">
        .auto-style1
        {
            width: 268435136px;
        }
    </style>
        
</asp:Content>

<asp:Content ID ="Content2" ContentPlaceHolderID ="ContentPlaceHolder1" runat ="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">

        <colgroup>
            <col width="12%" />
            <col width="15%" />
            <col width="12%" />
            <col width="15%" />
            <col width="12%" />
            <col width="34%" />
        </colgroup>
        <tbody>
            <tr>
                <th align="left" class="Body_TableHeader">
                   <asp:Label ID="lb_HAPPEN_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_HAPPEN_DT%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label">
                    <asp:Label ID="txt_APPLY_LEAVE_SDT" runat="server" MaxLength="10" Width="64px"  ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label">
                    <asp:Label ID="txt_EMP_ID" runat="server" MaxLength="10" Width="64px"  ClientIDMode="Static"></asp:Label>
                </td>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label">
                    <asp:Label ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="64px"  ClientIDMode="Static"></asp:Label>
                </td>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_DEPT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DEPT%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label">
                    <asp:Label ID="txt_DEPT_NAME" runat="server" MaxLength="10" Width="150px"  ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="10">
                    <div id="init_grid" style="display: inline">
                        <%--<input id="WFB2DH0400Back" runat="server" type="button"  value="<%$Resources:Resource,wfb2dh_WFB2DH0400Back%>" onclick="history.back(-2)" />--%>
                    </div>
                </td>
            </tr>
        </tbody>
    </table >


    <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getL1gvData"
                SelectCountMethod="getL1gvCount" TypeName="CFB2DH0400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_APPLY_LEAVE_SDT" Name="apply_leave_sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <%--<asp:ControlParameter ControlID="txt_EMP_NAME" Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />--%>
                    <%--<asp:ControlParameter ControlID="txt_DEPT_NO" Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />--%>
                    
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1120px"
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
                <Columns>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2dh_RowNumber%>" SortExpression="RowNumber"  />
                    <asp:BoundField DataField="MAIN_LEAVE_DESC" HeaderText="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>" SortExpression="MAIN_LEAVE_CD"  />
                    <asp:BoundField DataField="SUB_LEAVE_DESC" HeaderText="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD%>" SortExpression="SUB_LEAVE_CD"  />
                    <asp:BoundField DataField="FACT_HAPPEN_DT" HeaderText="<%$Resources:Resource,wfb2dh_lb_FACT_HAPPEN_DT%>" SortExpression="FACT_HAPPEN_DT"  />
                    <asp:BoundField DataField="APPLY_LEAVE_SDT" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_SDT%>" SortExpression="APPLY_LEAVE_SDT"  />
                    <asp:BoundField DataField="APPLY_LEAVE_STIME" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_STIME%>" SortExpression="APPLY_LEAVE_STIME"  />
                    <asp:BoundField DataField="APPLY_LEAVE_EDT" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_EDT%>" SortExpression="APPLY_LEAVE_EDT"  />
                    <asp:BoundField DataField="APPLY_LEAVE_ETIME" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_ETIME%>" SortExpression="APPLY_LEAVE_ETIME"  />
                    <asp:BoundField DataField="TOTAL_TIME_APPROVE" HeaderText="<%$Resources:Resource,wfb2dh_lb_TOTAL_TIME_APPROVE%>" SortExpression="TOTAL_TIME_APPROVE"  />
                    <asp:BoundField DataField="IFLOW_NO" HeaderText="<%$Resources:Resource,wfb2dh_lb_IFLOW_NO%>" SortExpression="IFLOW_NO"  />
                    <asp:BoundField DataField="IFLOW_APPROVE_DT" HeaderText="<%$Resources:Resource,wfb2dh_lb_IFLOW_APPROVE_DT%>" SortExpression="IFLOW_APPROVE_DT"  />
                    <asp:BoundField DataField="CHECK_STATUS" HeaderText="<%$Resources:Resource,wfb2dh_lb_CHECK_STATUS%>" SortExpression="CHECK_STATUS "  />
                    <asp:BoundField DataField="SALARY_SETTLE_STATUS" HeaderText="<%$Resources:Resource,wfb2dh_lb_SALARY_SETTLE_STATUS%>" SortExpression="SALARY_SETTLE_STATUS"  />
                    <asp:BoundField DataField="FORM_STATUS" HeaderText="<%$Resources:Resource,wfb2dh_lb_FORM_STATUS%>" SortExpression="FORM_STATUS"  />

                </Columns>


            </asp:GridView>
                
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr valign="top">

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
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
           
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            



        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
