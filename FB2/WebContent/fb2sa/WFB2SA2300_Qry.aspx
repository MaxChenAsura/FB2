<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sa/action/WFB2SA2300_Qry.aspx.cs" Inherits="WebContent_WFB2SA_WFB2SA2300_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".number").mask('9999');
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".empid").mask('99999')
            gridviewScroll();
            $.unblockUI();

            //寫在這，按查詢才不會消失
            $('#txt_HR_CHG_DESC').attr("readonly", true);
            $('#txt_HR_CHG_CD').change(function () {

                if ($('#txt_HR_CHG_CD').val().length <= 3) {
                    $.ajax({
                        url: "../commgeo/WFB2ChangeCode_Search.ashx",
                        data: {
                            HR_CHG_CD: $('#txt_HR_CHG_CD').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_HR_CHG_DESC').val("");
                            }
                            else {
                                $('#txt_HR_CHG_DESC').val(JData.HR_CHG_DESC);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_HR_CHG_DESC').val("");
                }
            });

            $('#txt_EMP_NAME').attr("readonly", true);
            //工號取得姓名的ajax
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME').val("");
                }
            });
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
                freezesize: 7

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        //清空畫面
        function ClearAll() {
            $("#ddl_PROCESS_STATUS").val("-1");
            $("#ddl_SALARY_PROC_TYPE").val("-1");
            $("#ddl_EMP_CD").val("-1");
            $("#txt_HR_CHG_CD").val("");
            $("#txt_HR_CHG_DESC").val("");
            $("#txt_EFFECTIVE_SDT").val("");
            $("#txt_EFFECTIVE_EDT").val("");
            $("#txt_HR_CHG_NO").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
        }

        function CheckSave() {
            BlockUI();
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                processed = true;

            } else {
                processed = false;
                $.unblockUI();
            }

            return processed;
        }
    </script>
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
                                <col width="15%" />
                                <col width="30%" />
                                <col width="15%" />
                                <col width="40%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_PROCESS_STATUS%>"></asp:Label>:</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_PROCESS_STATUS" runat="server" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_PROC_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_SALARY_PROC_TYPE%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_PROC_TYPE" runat="server" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HR_CHG_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_HR_CHG_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_HR_CHG_CD" runat="server" MaxLength="3" Width="40px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="btn_HR_CHG_CD" type="button" value="..." onclick="OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_DESC', '', '');" />
                                        <asp:TextBox ID="txt_HR_CHG_DESC" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EFFECTIVE_DT_RANGE" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EFFECTIVE_DT_RANGE%>"></asp:Label>:</th>
                                    <td>
                                        <asp:TextBox ID="txt_EFFECTIVE_SDT" runat="server" Width="100px" CssClass="date" ClientIDMode="Static"></asp:TextBox>
                                        ~
                                <asp:TextBox ID="txt_EFFECTIVE_EDT" runat="server" Width="100px" CssClass="date" ClientIDMode="Static"></asp:TextBox>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_EFFECTIVE_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_stime%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_EFFECTIVE_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_EFFECTIVE_SDT"
                                            ControlToValidate="txt_EFFECTIVE_EDT" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HR_CHG_NO" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_HR_CHG_NO%>"></asp:Label>:</th>
                                    <td>
                                        <asp:TextBox ID="txt_HR_CHG_NO" runat="server" MaxLength="12" Width="120px" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2sa_lb_HR_CHG_NO_isError%>" ControlToValidate="txt_HR_CHG_NO" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="^[\d]+$" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_EMP_CD" runat="server" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_ID%>"></asp:Label>:</th>
                                    <td>
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5"> </asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_NAME%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="30" Width="120px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th align="right">
                                        <aces:Btn ID="WFB2SA2300Search" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Search%>" ValidationGroup="GroupA" OnClick="WFB2SA2300Search_Click" CausesValidation="true" OnClientClick="return CheckSave();" />
                                        <%--<asp:Button ID="WFB2SA2300Search" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Search%>" ValidationGroup="GroupA" OnClick="WFB2SA2300Search_Click" CausesValidation="true" OnClientClick="return CheckSave();" />--%>
                                        <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Clear%>" OnClientClick="ClearAll();" CausesValidation="false" />
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
                            <aces:Btn ID="WFB2SA2300Execute" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2300Execute%>" OnClick="WFB2SA2300Execute_Click" OnClientClick="BlockUI();" Visible="False" />
                            <%--<asp:Button ID="WFB2SA2300Execute" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2300Execute%>" OnClick="WFB2SA2300Execute_Click" OnClientClick="BlockUI();" Visible="False" />--%>
                        </div>

                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SA2300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_PROCESS_STATUS" DefaultValue=""
                        Name="process_status" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_SALARY_PROC_TYPE" DefaultValue=""
                        Name="salary_proc_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_HR_CHG_CD"
                        Name="hr_chg_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EFFECTIVE_SDT"
                        Name="effective_sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EFFECTIVE_EDT"
                        Name="effective_edt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_HR_CHG_NO"
                        Name="hr_chg_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_EMP_CD" DefaultValue=""
                        Name="emp_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME"
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="2340px" OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2sa_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <asp:BoundField DataField="SALARY_PROC_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2sa_lb_SALARY_PROC_TYPE%>" SortExpression="SALARY_PROC_TYPE" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <asp:TemplateField  HeaderText="<%$Resources:Resource,wfb2sa_lb_HR_CHG_CD%>" SortExpression="HR_CHG_CD" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lb_HR_CHG_DESC" runat="server" Text='<%#Bind("HR_CHG_DESC")%>' ClientIDMode="Static" />
                            <asp:HiddenField ID="hid_HR_CHG_CD" runat="server" Value='<%#Bind("HR_CHG_CD")%>' ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2sa_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2sa_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_DESC" HeaderText="<%$Resources:Resource,wfb2sa_lb_EMP_CD%>" SortExpression="EMP_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_STATUS_DESC" HeaderText="<%$Resources:Resource,wfb2sa_lb_EMP_STATUS_CD%>" SortExpression="EMP_STATUS" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="WS_DESC" HeaderText="<%$Resources:Resource,wfb2sa_lb_WS_CD%>" SortExpression="WS_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="LEVEL_CD" HeaderText="<%$Resources:Resource,wfb2sa_lb_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="GRADE_CD" HeaderText="<%$Resources:Resource,wfb2sa_lb_GRADE_CD%>" SortExpression="GRADE_CD" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="EDUCATION_DESC" HeaderText="<%$Resources:Resource,wfb2sa_lb_EDUCATION_CD%>" SortExpression="EDUCATION_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="GRADE_YEAR" HeaderText="<%$Resources:Resource,wfb2sa_lb_GRADE_YEAR%>" SortExpression="GRADE_YEAR" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="SEX_DESC" HeaderText="<%$Resources:Resource,wfb2sa_lb_SEX_CD%>" SortExpression="SEX_CD" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ARMY_DESC" HeaderText="<%$Resources:Resource,wfb2sa_lb_ARMY_CD%>" SortExpression="ARMY_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="APPROVE_WORK_YEARS" HeaderText="<%$Resources:Resource,wfb2sa_lb_APPROVE_WORK_YEARS%>" SortExpression="APPROVE_WORK_YEARS" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="COMPANY_SNAME" HeaderText="<%$Resources:Resource,wfb2sa_lb_COMPANY_CD%>" SortExpression="COMPANY_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="IS_STUDENT" HeaderText="<%$Resources:Resource,wfb2sa_lb_IS_STUDENT%>" SortExpression="IS_STUDENT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DEPT_NO" HeaderText="<%$Resources:Resource,wfb2sa_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="JPN_DESC" HeaderText="<%$Resources:Resource,wfb2sa_lb_JPN_CD%>" SortExpression="JPN_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="HR_CHG_NO" HeaderText="<%$Resources:Resource,wfb2sa_lb_HR_CHG_NO%>" SortExpression="HR_CHG_NO" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="HR_PROC_DT" HeaderText="<%$Resources:Resource,wfb2sa_lb_HR_PROC_DT%>" DataFormatString="{0:yyyy/MM/dd}" SortExpression="HR_PROC_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EFFECTIVE_DT" HeaderText="<%$Resources:Resource,wfb2sa_lb_EFFECTIVE_DT%>" DataFormatString="{0:yyyy/MM/dd}" SortExpression="EFFECTIVE_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_PROCESS_STATUS%>" SortExpression="PROCESS_STATUS" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_PROCESS_STATUS_DESC" runat="server" Text='<%#Bind("PROCESS_STATUS_DESC")%>' ClientIDMode="Static" />
                            <asp:HiddenField ID="hid_PROCESS_STATUS" runat="server" Value='<%#Bind("PROCESS_STATUS")%>' ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PROCESS_DT" HeaderText="<%$Resources:Resource,wfb2sa_lb_PROCESS_DT%>" DataFormatString="{0:yyyy/MM/dd}" SortExpression="PROCESS_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="OP_MSG" HeaderText="<%$Resources:Resource,wfb2sa_lb_OP_MSG%>" SortExpression="OP_MSG" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
