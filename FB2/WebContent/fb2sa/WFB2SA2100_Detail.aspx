<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sa/action/WFB2SA2100_Detail.aspx.cs" Inherits="WebContent_WFB2SA_WFB2SA2100_Detail" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            gridviewScroll();
            $.unblockUI();
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
                freezesize: 3

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        //清空畫面
        function ClearAll() {
            $("#ddl_SALARY_ID").val("-1");
            $("#ddl_PROCESS_STATUS").val("-1");
            $("#txt_START_DT").val("");
            $("#txt_END_DT").val("");
        }

        function ValidateSearch() {
            if (Page_ClientValidate("GroupA") == false) {
                return false;
            }
            else {
                BlockUI();
            }
        }

        //刪除,檢查是否有勾選
        function doDelete() {
            BlockUI();
            var cnt = checkboxsSelected();
            if (cnt == 0 || cnt > 1) {
                alert("請選取一筆資料!");
                $.unblockUI();
                return false;
            } else {

                var grid = document.getElementById("<%=gv_result.ClientID %>");
                var ItemCheckBoxs = $("[type=checkbox]");
                var cellchg_status;

                if (grid.rows.length > 0) {
                    for (i = 1; i <= grid.rows.length - 1; i++) {
                        if (ItemCheckBoxs[i + 2].checked) {
                            cellchg_status = grid.rows[i].cells[14].all[grid.rows[i].cells[14].all.length - 1];
                            if (cellchg_status.value != "") {
                                if (confirm("確定要刪除?"))
                                    return true;
                                else {
                                    $.unblockUI();
                                    return false;
                                }
                            }
                        }
                    }
                }

                return true;
            }
        }

        //修改檢查是否有勾選
        function doUpdate() {
            BlockUI();
            var cnt = checkboxsSelected();
            if (cnt == 0 || cnt > 1) {
                alert("請選取一筆資料!");
                $.unblockUI();
                return false;
            }
            else
                return true;
        }

        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            var grid = document.getElementById("<%=gv_result.ClientID %>");
            var HaveCheck = 0;
            var ItemCheckBoxs = $("[type=checkbox]");
            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    if (ItemCheckBoxs[i + 2].checked)
                        HaveCheck++;
                }
            }
            return HaveCheck;
        }

        //清空畫面
        function ClearAll() {
            $("#ddl_SALARY_ID").val("-1");
            $("#ddl_PROCESS_STATUS").val("-1");
            $("#txt_START_DT").val("");
            $("#txt_END_DT").val("");
        }

    </script>
    <style type="text/css">
        .txtReadOnly[readonly] {
            background: #fff;
            color: #000;
            border: 0px solid #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="15%" />
                                <col width="10%" />
                                <col width="15%" />
                                <col width="10%" />
                                <col width="15%" />
                                <col width="10%" />
                                <col width="15%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_ID%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true"></asp:TextBox>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_NAME%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="113px" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_COMPANY_CD%>"></asp:Label>:</th>
                                    <td>
                                        <asp:TextBox ID="txt_COMPANY_SNAME" runat="server" Width="113px" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">

                                        <asp:TextBox ID="txt_EMP_CD_DESC" runat="server" Width="113px" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_JOIN_DT%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_JOIN_DT" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true"></asp:TextBox>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_STATUS%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_STATUS_DESC" runat="server" Width="113px" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_DT" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_LEAVE_DT%>"></asp:Label>:</th>
                                    <td>
                                        <asp:TextBox ID="txt_LEAVE_DT" runat="server" Width="113px" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_LEVEL_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">

                                        <asp:TextBox ID="txt_LEVEL_CD" runat="server" MaxLength="3" Width="113px" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GRADE_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_GRADE_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_GRADE_CD" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true"></asp:TextBox>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_PJOB_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PJOB_CD_DESC" runat="server" Width="113px" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th></th>
                                    <td></td>
                                    <th></th>
                                    <td></td>
                                </tr>
                            </tbody>
                        </table>
                        <hr />
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="12%" />
                                <col width="30%" />
                                <col width="12%" />
                                <col width="46%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_SALARY_ID%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">

                                        <asp:DropDownList ID="ddl_SALARY_ID" runat="server" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_PROCESS_STATUS%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">

                                        <asp:DropDownList ID="ddl_PROCESS_STATUS" runat="server" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EFFECT_DT_Range" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EFFECT_DT_Range%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_START_DT" runat="server" Width="113px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        ~
                                         <asp:TextBox ID="txt_END_DT" runat="server" Width="113px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDATE%>" ControlToValidate="txt_START_DT" ForeColor="Red"
                                            ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ControlToValidate="txt_END_DT" ForeColor="Red"
                                            ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <!--起日不可大於迄日 -->	
							        	<asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_START_DT"
                                       ControlToValidate="txt_END_DT" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                       Display="None" ValidationGroup="GroupA"></asp:CompareValidator>

                                    </td>
                                    <td colspan="2">
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th align="right">
                                        <aces:Btn ID="WFB2SA2100Search2" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Search%>" OnClick="WFB2SA2100Search2_Click" OnClientClick="return ValidateSearch();" />
                                        <%--<asp:Button ID="WFB2SA2100Search2" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Search%>" OnClick="WFB2SA2100Search2_Click" OnClientClick="return ValidateSearch();" />--%>
                                         <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,btn_clear%>" onclick="ClearAll();"   />
                                        <asp:Button ID="btn_backpage" runat="server" Text="<%$Resources:Resource,wfb2sa_cancel%>" OnClick="btn_backpage_Click"/>
                                    </th>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFB2SA2100Add" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Add%>" OnClick="WFB2SA2100Add_Click" OnClientClick="BlockUI();" />
                            <aces:Btn ID="WFB2SA2100Delete" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Delete%>" OnClick="WFB2SA2100Delete_Click" Visible="false" OnClientClick="return doDelete();" />
                            <aces:Btn ID="WFB2SA2100Edit" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Edit%>" OnClick="WFB2SA2100Edit_Click" Visible="false" OnClientClick="return doUpdate();" />
                            <%-- <asp:Button ID="WFB2SA2100Add" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Add%>" OnClick="WFB2SA2100Add_Click" OnClientClick="BlockUI();" />
                            <asp:Button ID="WFB2SA2100Delete" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Delete%>" OnClick="WFB2SA2100Delete_Click" Visible="false" OnClientClick="return doDelete();" />
                            <asp:Button ID="WFB2SA2100Edit" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Edit%>" OnClick="WFB2SA2100Edit_Click" Visible="false" OnClientClick="return doUpdate();" />--%>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDetailData"
                SelectCountMethod="getDetailCount" TypeName="CFB2SA2100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_SALARY_ID" DefaultValue=""
                        Name="salary_id" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_PROCESS_STATUS" DefaultValue=""
                        Name="process_status" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="hid_EMP_STATUS" DefaultValue=""
                        Name="emp_status_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_START_DT" DefaultValue=""
                        Name="start_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_END_DT" DefaultValue=""
                        Name="end_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1560px" OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2sa_RowNumber%>" HeaderStyle-Width="40px" />
                    <%-- 薪資項目 --%>       
                    <asp:BoundField DataField="SALARY_NAME" HeaderText="<%$Resources:Resource,wfb2sa_lb_SALARY_ID%>" SortExpression="SALARY_ID" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 敘薪金額 --%>
                    <asp:BoundField DataField="CHG_AMT_A" HeaderText="<%$Resources:Resource,wfb2sa_grd_AMOUNT%>" SortExpression="CHG_AMT_A" DataFormatString="{0:N0}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                    <%-- 生效日期起 --%>
                    <asp:BoundField DataField="EFFECT_SDT_A" HeaderText="<%$Resources:Resource,wfb2sa_lb_START_DT%>" SortExpression="EFFECT_SDT_A" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 生效日期迄 --%>
                    <asp:BoundField DataField="EFFECT_EDT_A" HeaderText="<%$Resources:Resource,wfb2sa_lb_END_DT%>" SortExpression="EFFECT_EDT_A" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 處理狀態 --%>
                    <asp:BoundField DataField="DESC1" HeaderText="<%$Resources:Resource,wfb2sa_lb_PROCESS_STATUS%>" SortExpression="PROCESS_STATUS" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 異動狀態 --%>
                    <asp:BoundField DataField="DESC2" HeaderText="<%$Resources:Resource,wfb2sa_lb_CHG_STATUS%>" SortExpression="CHG_STATUS" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 異動前金額 --%>     
                    <asp:BoundField DataField="CHG_AMT_B" HeaderText="<%$Resources:Resource,wfb2sa_grd_CHG_AMT_B%>" SortExpression="CHG_AMT_B" DataFormatString="{0:N0}" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                    <%-- 異動前生效日期起 --%> 
                    <asp:BoundField DataField="EFFECT_SDT_B" HeaderText="<%$Resources:Resource,wfb2sa_lb_EFFECT_SDT_B%>" SortExpression="EFFECT_SDT_B" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 異動前生效日期迄 --%>
                    <asp:BoundField DataField="EFFECT_EDT_B" HeaderText="<%$Resources:Resource,wfb2sa_lb_EFFECT_EDT_B%>" SortExpression="EFFECT_EDT_B" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 核定人員 --%>
                    <asp:BoundField DataField="APPROVE_BY" HeaderText="<%$Resources:Resource,wfb2sa_lb_APPROVE_BY%>" SortExpression="APPROVE_BY" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 核定日期 --%>
                    <asp:BoundField DataField="APPROVE_DT" HeaderText="<%$Resources:Resource,wfb2sa_lb_APPROVE_DT%>" SortExpression="APPROVE_DT" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 備註說明 --%>
                    <asp:BoundField DataField="REMARK" HeaderText="<%$Resources:Resource,wfb2sa_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 主管核定備註 --%>
                    <%-- 隱藏項目 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_APP_REMARK%>" SortExpression="APP_REMARK" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_APP_REMARK" runat="server" Text='<%#Bind("APP_REMARK")%>' Width="100%"></asp:Label>
                            <asp:HiddenField ID="hid_SEQ_NO" runat="server" Value='<%#Bind("SEQ_NO")%>' />
                            <asp:HiddenField ID="hid_SEQ_NO_B" runat="server" Value='<%#Bind("SEQ_NO_B")%>' />
                            <asp:HiddenField ID="hid_SALARY_ID" runat="server" Value='<%#Bind("SALARY_ID")%>' />
                            <asp:HiddenField ID="hid_CHG_STATUS" runat="server" Value='<%#Bind("CHG_STATUS")%>' />
                            <asp:HiddenField ID="hid_PROCESS_STATUS" runat="server" Value='<%#Bind("PROCESS_STATUS")%>' />
                            <asp:HiddenField ID="hid_EFFECT_SDT_A" runat="server" Value='<%#Bind("EFFECT_SDT_A", "{0:yyyy/MM/dd}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_EMP_STATUS" runat="server" ClientIDMode="Static" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
