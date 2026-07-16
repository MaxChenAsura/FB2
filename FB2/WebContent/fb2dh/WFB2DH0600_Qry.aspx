<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0600_Qry.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0600_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.date').mask('9999/99');
            $(".number").mask('999.99');

            $("#txt_EMP_NAME").attr("readonly", true);
            $("#txt_DEPT_NO").attr("readonly", true);
            $("#txt_DEPT_NAME").attr("readonly", true);

            gridviewScroll();
            $.unblockUI();

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
                                $('#txt_DEPT_NO').val("");
                                $('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));
                                $('#txt_DEPT_NO').val(JData.DEPT_NO);
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME').val("");
                    $('#txt_DEPT_NO').val("");
                    $('#txt_DEPT_NAME').val("");
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
                height: "500",
                barcolor: "#7F7F7F"
            });
        }

        function getEmp() {
            var is_super = $("#hid_is_super").val();
            var json = OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', is_super);
            if (json != undefined) {
                //alert("所屬部門代號:" + json.DEPT_NO + "\n" + "所屬部門名稱:" + json.DEPT_NAME + "\n" +
                //    "員工區分:" + json.EMP_CD + "\n" + "資格代號:" + json.LEVEL_CD + "\n" +
                //    "級數代號:" + json.GRADE_CD + "\n" + "職務代號:" + json.PJOB_CD + "\n" +
                //    "員工區分:" + json.JOIN_DT + "\n" + "資格代號:" + json.BE_EMP_DT + "\n" +
                //    "職種:" + json.WS_CD + "\n" + "在職狀態:" + json.EMP_STATUS + "\n");

                $("#txt_DEPT_NO").val(json.DEPT_NO);
                $("#txt_DEPT_NAME").val(json.DEPT_NAME);
            }
        }

        //清空畫面
        function ClearAll() {
            var newdate = getThisMonth();
            $("#txt_APPLY_LEAVE_YM").val(newdate);
            $("#txt_EMP_ID").val($("#hid_EMP_ID").val());
            $("#txt_EMP_NAME").val($("#hid_EMP_NAME").val());
            $("#txt_DEPT_NO").val($("#hid_DEPT_NO").val());
            $("#txt_DEPT_NAME").val($("#hid_DEPT_NAME").val());
        }

        function openDH0700() {
            if (Page_ClientValidate("GroupA")) {
                var apply_leave_dt = $("#txt_APPLY_LEAVE_YM").val();
                var emp_id = $("#txt_EMP_ID").val();
                var emp_name = $("#txt_EMP_NAME").val();
                var dept_no = $("#txt_DEPT_NO").val();
                var dept_name = $("#txt_DEPT_NAME").val();
                if (apply_leave_dt != "" && emp_id != "") {
                    window.open("WFB2DH0700_Qry.aspx?fn=FB2DH060&apply_leave_dt=" + apply_leave_dt +
                        "&emp_id=" + emp_id + "&emp_name=" + emp_name + "&dept_no=" + dept_no + "&dept_name=" + dept_name + "&parentFuncId=" + parentFuncID);
                }
            }
        }

        function getEmpName(EMP_id) {
            var txt = document.getElementById(EMP_id);
            if (txt.value != "") {
                document.getElementById('hid_getEmpName').click();
                return false;
            }
            else {
                $("#txt_EMP_NAME").val("");
                $("#txt_DEPT_NO").val("");
                $("#txt_DEPT_NAME").val("");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="25%" />
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="25%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPLY_LEAVE_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_DT2%>"></asp:Label>:
                                    </th>
                                    <%-- 年月 --%>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_APPLY_LEAVE_YM" runat="server" Width="80px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_APPLY_LEAVE_DT%>"
                                            ControlToValidate="txt_APPLY_LEAVE_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_APPLY_LEAVE_DATE%>" ControlToValidate="txt_APPLY_LEAVE_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <%-- 工號 --%>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="80px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_EMP_ID%>"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2dh_format_emp_id%>" ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression=".{5,5}" Display="None"></asp:RegularExpressionValidator>
                                        <input id="bt_EMP_ID" type="button" value="..." onclick="getEmp();" />
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="80px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DEPT_NO2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="Body_label" colspan="6">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DH0600Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0600Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DH0600Search_Click" />
                                            <aces:Btn ID="WFB2DH0406Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0600Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DH0600Search_Click" />
                                            <aces:Btn ID="WFB2DH0506Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0600Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DH0600Search_Click" />
                                            <%--<asp:Button ID="WFB2DH0600Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0600Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DH0600Search_Click"/>--%>

                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dh_btn_clear%>" onclick="ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="right" class="Body_label" colspan="5">
                                        <div id="init_grid">
                                            <aces:Btn ID="WFB2DH0600QryDetail" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0600QryDetail%>" OnClientClick="openDH0700();" />
                                            <aces:Btn ID="WFB2DH0406QryDetail" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0600QryDetail%>" OnClientClick="openDH0700();" />
                                            <%-- FOR FB2DH040--%>
                                            <aces:Btn ID="WFB2DH0506QryDetail" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0600QryDetail%>" OnClientClick="openDH0700();" />
                                            <%-- FOR FB2DH050--%>
                                            <%--<asp:Button ID="WFB2DH0600QryDetail" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0600QryDetail%>" OnClientClick="openDH0700();" />--%>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

                <tr id="tr_overtime" runat="server" visible ="false">
                    <td align="left" class="Body_label" colspan="5" >

                        <fieldset style="padding:5px">
                            <%-- 剩餘時數(不含在途) --%>
                            <legend class="Body_label">
                                <asp:Label ID="lb_lack_time" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_lack_time%>"></asp:Label>

                            </legend>
                            <table align="left" width="100%">
                                <tr>
                                    <%-- 平日換休(月度) --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME2" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_OVERTIME2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME2" runat="server" Width="80px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%-- 假日換休 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME3" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_OVERTIME3%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME3" runat="server" Width="80px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                     <%-- 特休 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME4" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_OVERTIME4%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME4" runat="server" Width="80px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                      <%-- 榮譽假 --%>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME5" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_OVERTIME5%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME5" runat="server" Width="80px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <br/><br/>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DH0600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_APPLY_LEAVE_YM"
                        Name="apply_leave_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_is_super"
                        Name="is_super" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_is_dept"
                        Name="is_dept" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_departments"
                        Name="departments" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1020px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" >
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'  width="40px" ItemStyle-HorizontalAlign="Center"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 主假別 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_MAIN_LEAVE_CD%>" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="MAIN_LEAVE_CD_DESC">
                        <ItemTemplate>
                            <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text='<%#Bind("MAIN_LEAVE_CD_DESC")%>' width="100px"  ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 子假別 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_SUB_LEAVE_CD%>" HeaderStyle-Width="180px" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left" >
                        <ItemTemplate>
                            <asp:Label ID="lb_SUB_LEAVE_CD" runat="server" Text='<%#Bind("SUB_LEAVE_CD_DESC")%>' width="180px" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 時間單位 
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_TIME_UNIT%>" HeaderStyle-Width="80px" SortExpression="LEAVE_TIME_UNIT">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_TIME_UNIT" runat="server" Text='<%#Bind("LEAVE_TIME_UNIT_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    --%>
                     <%-- 查詢月度合計時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_TOTAL_TIME_APPROVE_M_DH060%>" HeaderStyle-Width="160px"  ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Right" SortExpression="TOTAL_TIME_APPROVE_M" >
                        <ItemTemplate>
                            <asp:Label ID="lb_TOTAL_TIME_APPROVE_M" runat="server" Text='<%#Bind("TOTAL_TIME_APPROVE_M")%>' width="160px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 累計至查詢月度--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_TOTAL_TIME_APPROVE_C%>" HeaderStyle-Width="90px"  ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right" SortExpression="TOTAL_TIME_APPROVE_C">
                        <ItemTemplate>
                            <asp:Label ID="lb_TOTAL_TIME_APPROVE_C" runat="server" Text='<%#Bind("TOTAL_TIME_APPROVE_C")%>' width="90px" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 年度累計時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_TOTAL_TIME_APPROVE_Y_DH060%>" HeaderStyle-Width="120px"  ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right"  SortExpression="TOTAL_TIME_APPROVE_Y">
                        <ItemTemplate>
                            <asp:Label ID="lb_TOTAL_TIME_APPROVE_Y" runat="server" Text='<%#Bind("TOTAL_TIME_APPROVE_Y")%>' width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 在途申請時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_TOTAL_TIME_IFLOW%>" HeaderStyle-Width="120px"  ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right" SortExpression="TOTAL_TIME_IFLOW">
                        <ItemTemplate>
                            <asp:Label ID="lb_TOTAL_TIME_IFLOW" runat="server" Text='<%#Bind("TOTAL_TIME_IFLOW")%>' width="120px"></asp:Label>
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
            </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:HiddenField ID="hid_is_super" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_is_dept" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_departments" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_EMP_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_DEPT_NO" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_DEPT_NAME" runat="server" ClientIDMode="Static" />
            <asp:Button ID="hid_getEmpName" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getEmpName_Click" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

