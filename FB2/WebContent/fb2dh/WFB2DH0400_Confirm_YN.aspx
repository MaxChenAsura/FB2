<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0400_Confirm_YN.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0400_Confirm_YN" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });

            $(".number2").mask("999");
            $.unblockUI();
            gridviewScroll();
        }

        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 5

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        //清空畫面
        function ClearAll() {
            $("#txt_APPLY_LEAVE_SDT").val("");
            $("#txt_APPLY_LEAVE_EDT").val("");
        }
    </script>

    <style type="text/css">
        .auto-style1
        {
            width: 268435136px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
        <%--查詢條件欄位寬度--%>
        <colgroup>
            <col width="15%" />
            <col width="30%" />
            <col width="15%" />
            <col width="40%" />
            
        </colgroup>
        <tbody>
            <%--查詢條件欄位--%>
                <%--請假日期--%>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_APPLY_LEAVE" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label">
                    <asp:TextBox ID="txt_APPLY_LEAVE_SDT" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="81px" ClientIDMode="Static"></asp:TextBox>
                    <asp:RequiredFieldValidator ID ="RequiredFieldValidator1" runat="server" ErrorMessage="<% $Resources:Resource,wfb2dh_Required_APPLY_LEAVE_DT %> "
                                ControlToValidate="txt_APPLY_LEAVE_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID ="RegularExpressionValidator2" runat="server"
                                ErrorMessage="<% $Resources:Resource,wfb2dh_Required_APPLY_LEAVE_DT2 %>" ControlToValidate="txt_APPLY_LEAVE_SDT" ForeColor="Red"
                                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])[/ /.](0[1-9]|[12][0-9]|3[01])" Display="None" ValidationGroup="GroupA">
                    </asp:RegularExpressionValidator>
                            ~
                    <asp:TextBox ID="txt_APPLY_LEAVE_EDT" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="81px" ClientIDMode="Static"></asp:TextBox>
                    <asp:RequiredFieldValidator ID ="RequiredFieldValidator11" runat="server" ErrorMessage="<% $Resources:Resource,wfb2dh_Required_APPLY_LEAVE_DT %> "
                                ControlToValidate="txt_APPLY_LEAVE_EDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID ="RegularExpressionValidator1" runat="server"
                                ErrorMessage="<% $Resources:Resource,wfb2dh_Required_APPLY_LEAVE_DT2 %>" ControlToValidate="txt_APPLY_LEAVE_EDT" ForeColor="Red"
                                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])[/ /.](0[1-9]|[12][0-9]|3[01])" Display="None" ValidationGroup="GroupA">
                    </asp:RegularExpressionValidator>
                </td>
                <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IS_CONFIRM_CHECK" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IS_CONFIRM_CHECK%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label" colspan=4>		
                        <asp:RadioButtonList ID="rbl_IS_CONFIRM_CHECK" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static">
                         <asp:ListItem Text="<%$Resources:Resource,wfb2dh_Radiobutton3%>" Value="Y" Selected="True"></asp:ListItem>
                         <asp:ListItem Text="<%$Resources:Resource,wfb2dh_Radiobutton4%>" Value="N"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:HiddenField ID="hid_IS_CONFIRM_CHECK" runat="server" />
            </tr>
                
            <%--查詢清除Button--%>
            <tr>
                <td align="right" colspan="10">
                    <div id="init_grid" style="display: inline">
                        <aces:Btn ID="WFB2DH0402Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Search%>" OnClick="WFB2DH0400Search_Click" ValidationGroup="GroupA"/>

                        <%--<asp:Button ID="WFB2DH0402Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Search%>" OnClick="WFB2DH0400Search_Click" ValidationGroup="GroupA"/>--%>
                        <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dh_btn_clear%>" onclick="ClearAll();" />
                    </div>
                </td>
            </tr>
            <%--線--%>
            <tr>
                <td align="center" height="1" colspan="10">
                    <hr />
                </td>
            </tr>
            <%--新增刪除修改確認取消Button--%>
            <tr>
                <td align="right" colspan="10">
                    <aces:Btn ID="WFB2DH0400Confirm" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Confirm%>" OnClick="WFB2DH0400Confirm_Click" />

                    <%--<asp:Button ID="WFB2DH0400Confirm" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Confirm%>" OnClick="WFB2DH0400Confirm_Click" />--%>
                    <asp:Button ID="WFB2DH0400Cancel" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Cancel%>" OnClick="WFB2DH0400Cancel_Click" />
                </td>
            </tr>
            
                
        </tbody>
    </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getConfirmYNData"
                SelectCountMethod="getConfirmYNCount" TypeName="CFB2DH0400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_APPLY_LEAVE_SDT" Name="apply_leave_sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_APPLY_LEAVE_EDT" Name="apply_leave_edt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="rbl_IS_CONFIRM_CHECK" Name="is_confirm_check" PropertyName="SelectedValue" Type="String" />

                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1120px"
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID"  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" SortExpression="RowNumber"  />
                    <asp:BoundField DataField="DEPT_NO" HeaderText="<%$Resources:Resource,wfb2dh_lb_DEPT_NO%>" SortExpression="DEPT_NO"  HeaderStyle-Width="185px"/>
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>" SortExpression="EMP_ID"  />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="50px"/>
                    <asp:BoundField DataField="MAIN_LEAVE_CD" HeaderText="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>" SortExpression="MAIN_LEAVE_CD"  />
                    <asp:BoundField DataField="SUB_LEAVE_CD" HeaderText="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD%>" SortExpression="SUB_LEAVE_CD"  />
                    <%--<asp:BoundField DataField="FACT_HAPPEN_DT" HeaderText="<%$Resources:Resource,wfb2dh_lb_FACT_HAPPEN_DT%>" SortExpression="FACT_HAPPEN_DT"  />--%>
                    <asp:BoundField DataField="APPLY_LEAVE_SDT" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_SDT%>" SortExpression="APPLY_LEAVE_SDT"  />
                    <asp:BoundField DataField="APPLY_LEAVE_STIME" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_STIME%>" SortExpression="APPLY_LEAVE_STIME"  />
                    <asp:BoundField DataField="APPLY_LEAVE_EDT" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_EDT%>" SortExpression="APPLY_LEAVE_EDT"  />
                    <asp:BoundField DataField="APPLY_LEAVE_ETIME" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_ETIME%>" SortExpression="APPLY_LEAVE_ETIME"  />
                    <asp:BoundField DataField="TOTAL_TIME_APPROVE" HeaderText="<%$Resources:Resource,wfb2dh_lb_TOTAL_TIME_APPROVE%>" SortExpression="TOTAL_TIME_APPROVE"  />
                    <asp:BoundField DataField="IFLOW_NO" HeaderText="<%$Resources:Resource,wfb2dh_lb_IFLOW_NO%>" SortExpression="IFLOW_NO"  />
                    <asp:BoundField DataField="IFLOW_APPROVE_DT" HeaderText="<%$Resources:Resource,wfb2dh_lb_IFLOW_APPROVE_DT%>" SortExpression="IFLOW_APPROVE_DT"  />
                    <asp:BoundField DataField="IS_CONFIRM_CHECK" HeaderText="<%$Resources:Resource,wfb2dh_lb_IS_CONFIRM_CHECK%>" SortExpression="IS_CONFIRM_CHECK"  />
                    <asp:BoundField DataField="CHECK_STATUS" HeaderText="<%$Resources:Resource,wfb2dh_lb_CHECK_STATUS%>" SortExpression="CHECK_STATUS"  />
                    <%--<asp:BoundField DataField="FORM_STATUS" HeaderText="<%$Resources:Resource,wfb2dh_lb_FORM_STATUS%>" SortExpression="FORM_STATUS"  />--%>


                </Columns>


            </asp:GridView>
            
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
           
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>