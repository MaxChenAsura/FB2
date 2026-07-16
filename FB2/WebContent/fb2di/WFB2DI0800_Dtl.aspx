<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0800_Dtl.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0800_Dtl" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date1").datepicker({ dateFormat: 'yy/mm' });

            $("#txt_EMP_ID").attr("readonly", true);
            $("#txt_EMP_NAME").attr("readonly", true);
            $("#txt_DEPT_NAME").attr("readonly", true);
            $("#txt_YM").attr("readonly", true);

            gridviewScroll();
            $.unblockUI();
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
                ,freezesize: 3
            });
        }


        function getDept() {
            var json = OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
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
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                        </td>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_NAME%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                        </td>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT_NAME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NAME%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <td align="left" class="Body_label"></td>
                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_YM" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CALENDAR_YM%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_YM" runat="server" MaxLength="5" Width="81px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                        </td>
                        <td align="right" class="Body_label" colspan="4">
                            <div id="init">
                                <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2di_btn_back%>" OnClick="btn_back_Click" />
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td align="center" height="1" colspan="10"></td>
                    </tr>
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDtlData"
                SelectCountMethod="getDtlCount" TypeName="CFB2DI0800DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:QueryStringParameter QueryStringField="emp_id" Name="emp_id" ConvertEmptyStringToNull="false" Type="String" />
                    <asp:QueryStringParameter QueryStringField="ym" Name="ym" ConvertEmptyStringToNull="false" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>

                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>"   HeaderStyle-Width ="40px"  ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="APPLY_OVERTIME_DT" HeaderText="<%$Resources:Resource,wfb2di_APPLY_OVERTIME_DT%>" SortExpression="APPLY_OVERTIME_DT" HeaderStyle-Width="100px"   ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center" />
                    <%--加班類型 --%>
                    <asp:BoundField DataField="OVERTIME_DESC" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_CD%>" SortExpression="OVERTIME_CD" HeaderStyle-Width="160px"  ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                    <%--加班時段別 --%>
                    <asp:BoundField DataField="OVERTIME_TIME_DESC" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_DT_TYPE%>" SortExpression="OVERTIME_DT_TYPE"  HeaderStyle-Width="100px"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left"   />
                    <asp:BoundField DataField="APPROVE_OVERTIME_HOUR" HeaderText="<%$Resources:Resource,wfb2di_APPROVE_OVERTIME_HOUR%>" HtmlEncode="false" SortExpression="APPLY_OVERTIME_HOUR" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center"  />
                    <asp:BoundField DataField="BEFORE_HOUR" HeaderText="<%$Resources:Resource,wfb2di_BEFORE_HOUR2%>" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center"  />
                    <asp:BoundField DataField="BEFORE_SETIME" HeaderText="<%$Resources:Resource,wfb2di_BEFORE_SETIME%>" HeaderStyle-Width="120px"  ItemStyle-Width="120px"  HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="AFTER_HOUR" HeaderText="<%$Resources:Resource,wfb2di_AFTER_HOUR2%>"  HeaderStyle-Width="70px"  ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center"  />
                    <asp:BoundField DataField="AFTER_SETIME" HeaderText="<%$Resources:Resource,wfb2di_AFTER_SETIME%>" HeaderStyle-Width="120px"  ItemStyle-Width="120px" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="IFLOW_APPROVE_DT" HeaderText="<%$Resources:Resource,wfb2di_IFLOW_APPROVE_DT%>" HeaderStyle-Width="80px"  ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="CHECK_STATUS" HeaderText="<%$Resources:Resource,wfb2di_CHECK_STATUS%>" HeaderStyle-Width="100px"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%--表單狀態 --%>
                    <asp:BoundField DataField="FORM_STATUS" HeaderText="<%$Resources:Resource,wfb2di_lb_FORM_STATUS%>" SortExpression="FORM_STATUS" HeaderStyle-Width="80px" ItemStyle-Width="80px" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%--計薪狀態 --%>
                    <asp:BoundField DataField="SALARY_SETTLE_STATUS" HeaderText="<%$Resources:Resource,wfb2di_SALARY_SETTLE_STATUS%>" HeaderStyle-Width="80px"  ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="PAY_DT" HeaderText="<%$Resources:Resource,wfb2di_PAY_DT%>" HeaderStyle-Width="100px"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField DataField="IFLOW_NO" HeaderText="<%$Resources:Resource,wfb2di_IFLOW_NO%>" HeaderStyle-Width="160px"  ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Center" />
                </Columns>

                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
            </asp:GridView>

            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
