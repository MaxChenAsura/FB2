<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2dl/WFB2DL0300_Qry.aspx.cs" Inherits="WebContent_fb2dl_WFB2DL0300_Qry" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ym').mask('9999/99/99');
            $('#txt_LEAVE_PLAN_YEAR_search').mask('9999');
            $('#txt_DEPT_NAME').attr("readonly", true);
            $('#txt_qry_EMP_NAME').attr("readonly", true);
            gridviewScroll();
            $.unblockUI();

            //工號取得姓名的ajax
            $("#txt_EMP_ID_search").change(function () {
                if ($("#txt_EMP_ID_search").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID_search').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_qry_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_qry_EMP_NAME').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_qry_EMP_NAME').val("");
                }
            });

            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NO").change(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_NAME').val("");
                }
            });
        }

        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            $("#HID_PageRow2").val($("#ddlPerPageRow2").val());
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }

        //清空
        function ClearAll() {
            var day = new Date();
            var year = day.getFullYear();
            $("#txt_LEAVE_PLAN_YEAR_search").val(year);
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#lb_company_target_txt").text("");
            $("#lb_ACHIEVEMENT_RATE_txt").text("");
            $("#txt_EMP_ID_search").val($("#hid_defalut_EMP_ID").val());
            $("#txt_qry_EMP_NAME").val($("#hid_defalut_EMP_NAME").val());
            return false;
        }
        function Check(source, arguments) {
            if ($("#txt_EMP_ID_search").val() == "" && $("#txt_DEPT_NO").val() == "") {
                arguments.IsValid = false;
            }
            else
                arguments.IsValid = true;
        }
        function CheckLEAVE_PLAN_YEAR(source, arguments) {
            var today = new Date();
            var re = /^\d\d\d\d$/;
            var currentYear = today.getFullYear();
            if (re.test($("#txt_LEAVE_PLAN_YEAR_search").val())) {
                if ($("#txt_LEAVE_PLAN_YEAR_search").val() >= 1911) {
                    arguments.IsValid = true;
                }
                else {
                    arguments.IsValid = false;
                }
            }
            else {
                arguments.IsValid = false;
            }
        }
        function delayedShow() {
            if (Page_IsValid != null && Page_IsValid == true) {
                BlockUI();
            }
        }
        function SelectSingleRadiobutton(rdbtnid) {
            var rdBtn = document.getElementById(rdbtnid);
            var rdBtnList = $("[type=radio]");
            for (i = 0; i < rdBtnList.length; i++) {
                if (rdBtnList[i].id != rdBtn.id) {
                    rdBtnList[i].checked = false;
                }
                else {
                    $('#hid_selectedIndex').val(i);
                }
            }
            $('#WFB2DL0300Check').click();
        }


        function doUnBlock() {
            $.unblockUI();
        }
        function MycheckDowning(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return;
            BlockUI();
            processed = confirm($("#hidconfirm_Execute").val() + msg + "?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--頁面table-->
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="14%" />
                                <col width="15%" />
                                <col width="10%" />
                                <col width="22%" />
                                <col width="10%" />
                                <col width="29%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--排休年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_PLAN_YEAR_search" runat="server" Text="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_YEAR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LEAVE_PLAN_YEAR_search" runat="server" MaxLength="5" Width="64px" class="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_YEAR_isNull%>"
                                            ControlToValidate="txt_LEAVE_PLAN_YEAR_search" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_YEAR_isError%>"
                                            ClientValidationFunction="CheckLEAVE_PLAN_YEAR" ForeColor="Red"
                                            ControlToValidate="txt_LEAVE_PLAN_YEAR_search" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                    <%--工號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID_search" runat="server" Text="<%$Resources:Resource,wfb2dl_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID_search" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_search', 'txt_qry_EMP_NAME', 'N');" />
                                        <asp:TextBox ID="txt_qry_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_EMP_ID_choose_one%>"
                                            ClientValidationFunction="Check" ForeColor="Red"
                                            ControlToValidate="txt_EMP_ID_search" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                    <%--部門代號.部門名稱--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO_search" runat="server" Text="<%$Resources:Resource,wfb2dl_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" runat="server" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" MaxLength="60" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--公司目標數(日)--%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_company_target" runat="server" Text="<%$Resources:Resource,wfb2dl_lb_COMPANY_PLAN_TARGET%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:Label ID="lb_company_target_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                </td>
                                <%--達成率--%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_ACHIEVEMENT_RATE" runat="server" Text="<%$Resources:Resource,wfb2dl_lb_ACHIEVEMENT_RATE%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label" colspan="3">
                                    <asp:Label ID="lb_ACHIEVEMENT_RATE_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                </td>
                                <tr>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <%--<asp:Button ID="WFB2DL0300Search" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Search%>" OnClick="WFB2DL0300Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <aces:Btn ID="WFB2DL0300Search" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Search%>" OnClick="WFB2DL0300Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2dl_btn_clear%>" OnClientClick="return ClearAll();" />
                                            <%--<asp:Button ID="WFB2DL0300ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2dl_excelImport%>" OnClick="WFB2DL0300ExcelDown_Click" OnClientClick="return MycheckDowning(this.value);" ValidationGroup="GroupA" />--%>
                                            <aces:Btn ID="WFB2DL0300ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2dl_excelImport%>" OnClick="WFB2DL0300ExcelDown_Click" OnClientClick="return MycheckDowning(this.value);" ValidationGroup="GroupA" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>

            <!--grid1-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DL0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_LEAVE_PLAN_YEAR_search" DefaultValue=""
                        Name="leave_plan_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID_search" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO" DefaultValue=""
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_is_super"
                        Name="is_super" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_is_dept"
                        Name="is_dept" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_departments"
                        Name="departments" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderText="" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:RadioButton ID="WFB2DL0300Radio" runat="server" OnClick="javascript:SelectSingleRadiobutton(this.id)" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部門--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_DEPT%>" SortExpression="ORI_DEPT_NO" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                        <ItemTemplate>
                            <div style="text-align: left;">
                                <asp:Label ID="lb_DEPT" runat="server" Text='<%#Bind("DEPT")%>' Style="word-wrap: break-word;"></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_ORI_LEVEL_CD%>" SortExpression="ORI_LEVEL_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lb_ORI_LEVEL_CD" runat="server" Text='<%#Bind("ORI_LEVEL_CD")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--年休可用數(時)--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_lb_AVAILABLE_VALUE%>" SortExpression="AVAILABLE_VALUE" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <div style="text-align: right;">
                                <asp:Label ID="lb_AVAILABLE_VALUE" runat="server" Text='<%#Bind("AVAILABLE_VALUE")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--年休目標數(時)--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_lb_EMP_LEAVE_TARGET2%>" SortExpression="EMP_LEAVE_TARGET" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <div style="text-align: right;">
                                <asp:Label ID="lb_EMP_LEAVE_TARGET" runat="server" Text='<%#Bind("EMP_LEAVE_TARGET")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--休假不足數(時)--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_lb_TARGET_MINUS%>" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <%--已休假時數--%>
                            <asp:HiddenField ID="hid_TOTAL_TIME_APPROVE" runat="server" Value='<%#Bind("TOTAL_TIME_APPROVE")%>'></asp:HiddenField>
                            <div style="text-align: right;">
                                <asp:Label ID="lb_TARGET_MINUS" runat="server" Text='<%#Bind("TARGET_MINUS")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height: 30px;"></td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                     <!--grid2-->
                    <asp:GridView ID="gv_result2" runat="server" AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False"
                        OnRowDataBound="gv_result_RowDataBound2" OnRowCreated="gv_result_RowCreated" Width="1020px" OnDataBound="gv_result_DataBound">
                        <Columns>
                            <asp:BoundField DataField="TITLE" HeaderText="" HeaderStyle-Width="72px" />
                            <asp:BoundField DataField="JAN" HeaderText="<%$Resources:Resource,wfb2dl_JAN%>" HeaderStyle-Width="72px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="FEB" HeaderText="<%$Resources:Resource,wfb2dl_FEB%>" HeaderStyle-Width="72px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="MAR" HeaderText="<%$Resources:Resource,wfb2dl_MAR%>" HeaderStyle-Width="72px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="APR" HeaderText="<%$Resources:Resource,wfb2dl_APR%>" HeaderStyle-Width="72px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="MAY" HeaderText="<%$Resources:Resource,wfb2dl_MAY%>" HeaderStyle-Width="72px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="JUN" HeaderText="<%$Resources:Resource,wfb2dl_JUN%>" HeaderStyle-Width="72px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="JUL" HeaderText="<%$Resources:Resource,wfb2dl_JUL%>" HeaderStyle-Width="72px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="AUG" HeaderText="<%$Resources:Resource,wfb2dl_AUG%>" HeaderStyle-Width="72px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="SEP" HeaderText="<%$Resources:Resource,wfb2dl_SEP%>" HeaderStyle-Width="72px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="OCT" HeaderText="<%$Resources:Resource,wfb2dl_OCT%>" HeaderStyle-Width="72px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="NOV" HeaderText="<%$Resources:Resource,wfb2dl_NOV%>" HeaderStyle-Width="72px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="DEC" HeaderText="<%$Resources:Resource,wfb2dl_DEC%>" HeaderStyle-Width="72px" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="TOTAL" HeaderText="<%$Resources:Resource,wfb2dl_TOTAL%>" HeaderStyle-Width="72px" ItemStyle-HorizontalAlign="Right" />
                        </Columns>

                        <PagerStyle CssClass="GridviewScrollPager" />
                        <FooterStyle CssClass="GridviewScrollPager" />
                    </asp:GridView>
                </td>
              </tr>
            </table>


            <asp:Button ID="WFB2DL0300Check" runat="server" OnClick="WFB2DL0300Check_Click" ClientIDMode="Static" Style="display: none" />
            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_selectedIndex" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_current_month" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_current_year" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_leave_plan_year" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_emp_id" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_is_super" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_is_dept" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_departments" runat="server" ClientIDMode="Static" />
             <!--預設的工號,姓名-->
            <asp:HiddenField ID="hid_defalut_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_defalut_EMP_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidconfirm_Execute" Value="<%$Resources:Resource,confirm_Execute%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2ia_condition_NottypeMessage" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DL0300ExcelDown" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
