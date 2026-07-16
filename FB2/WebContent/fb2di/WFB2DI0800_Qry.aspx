<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0800_Qry.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0800_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {

            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $(".date").mask('9999/99');

            $("#txt_DEPT_NAME").attr("readonly", true);
            $("#txt_OVERTIME_TARGET_VALUE").attr("readonly", true);
            $("#txt_OVERTIME1").attr("readonly", true);
            $("#txt_OVERTIME2").attr("readonly", true);
            $("#txt_DEPT_EMPS").attr("readonly", true);
            $("#txt_OVERTIME4").attr("readonly", true);
            $("#txt_OVERTIME5").attr("readonly", true);
            $("#txt_OVERTIME_AVG_HOUR").attr("readonly", true);
            $("#txt_OVERTIME7").attr("readonly", true);
            $("#txt_OVERTIME8").attr("readonly", true);
            //寫在這，按查詢才不會消失
            $('#txt_DEPT_NAME').attr("readonly", true);
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
                    $('#txt_qry_DEPT_NAME').val("");
                    $('#txt_DEPT_NAME').val("");
                }
            });
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
                ,freezesize: 5
                ,headerrowcount: 2
            });
        }

        //清空畫面
        function ClearAll() {
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#ddl_TARGET_TYPE").val("-1");
            $("#txt_YM").val("");

        }

        //function getDept() {
        //    var json = OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');
        //    //if (json != undefined)
        //    //alert("上層部門代號:" + json.UP_DEPT_NO + "\n" + "上層部門名稱:" + json.UP_DEPT_NAME + "\n" + "部門主管工號:" + json.HEAD_EMP_ID + "\n" + "部門主管名稱:" + json.HEAD_EMP_NAME);
        //}

        function getDept() {
            var dept_no = $("#txt_DEPT_NO").val();
            var dept_no_list = $("#hid_dept_no_list").val();
            OpenSearch('DeptGrid_Search.aspx', 'txt_DEPT_NO', 'txt_DEPT_NAME', 'DEPT_NO=' + dept_no + '&DEPT_NO_LIST=' + dept_no_list);
        }

        function openAbnormal1(dept_no, dept_name, targettype, targettypeName, ym) {
            window.open("WFB2DI0800_Abnormal1.aspx?dept_no=" + dept_no + "&dept_name=" + dept_name +
                "&target_type=" + targettype + "&target_typeName=" + targettypeName + "&ym=" + ym + "&parentFuncId=" + parentFuncID);
            //window.showModalDialog("WFB2DI0800_Abnormal1.aspx?dept_no=" + dept_no + "&dept_name=" + dept_name +
            //    "&target_type=" + targettype + "&target_typeName=" + targettypeName + "&ym=" + ym + "&parentFuncId=" + parentFuncID, self, 'dialogWidth=800px;dialogHeight=600px;scroll=no');
        }


        function openAbnormal2(dept_no, dept_name, targettype, targettypeName, ym) {
            window.open("WFB2DI0800_Abnormal2.aspx?dept_no=" + dept_no + "&dept_name=" + dept_name +
                "&target_type=" + targettype + "&target_typeName=" + targettypeName + "&ym=" + ym + "&parentFuncId=" + parentFuncID);
            //window.showModalDialog("WFB2DI0800_Abnormal2.aspx?dept_no=" + dept_no + "&dept_name=" + dept_name +
            //    "&target_type=" + targettype + "&target_typeName=" + targettypeName + "&ym=" + ym + "&parentFuncId=" + parentFuncID, self, 'dialogWidth=800px;dialogHeight=700px;scroll=no');
        }

        function getDEPT_NAME(EMP_id) {
            var txt = document.getElementById(EMP_id);
            if (txt.value != "") {
                document.getElementById('hid_getDEPT_NAME').click();
                return false;
            }
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table cellspacing="1" cellpadding="1" width="1020px">
                <tr>
                    <td>

                        <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="30%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%-- 部門 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NAME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NAME%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <%-- 
                                            <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="getDept();" />
                                        --%>
                                        <input id="Button1" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />

                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2dj_format_dept_no%>" ControlToValidate="txt_DEPT_NO" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression=".{7,7}" Display="None"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_required_DEPT_NO_not_exist%>"
                                            ControlToValidate="txt_DEPT_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="MainValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_required_DEPT_NO%>"
                                            ControlToValidate="txt_DEPT_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>


                                    </td>
                                    <%-- 管理類別 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_TARGET_TYPE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_TARGET_TYPE%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="4">
                                        <asp:DropDownList ID="ddl_TARGET_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_required_TARGET_TYPE%>"
                                            ControlToValidate="ddl_TARGET_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>

                                <tr>
                                    <%-- 勤務年月 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_YM" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CALENDAR_YM%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_YM" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_YM%>" ControlToValidate="txt_YM" ForeColor="Red"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_required_YM%>"
                                            ControlToValidate="txt_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right" class="Body_label" colspan="10">
                                        <div id="init">
                                            <%--
                                             <aces:Btn ID="WFB2DI0800Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0800Search%>" OnClick="WFB2DI0800Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            --%>
                                            <asp:Button ID="WFB2DI0800Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0800Search%>" OnClick="WFB2DI0800Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2di_btn_clear%>" onclick="ClearAll();" />
                                        </div>
                                    </td>

                                </tr>

                                <tr>
                                    <td align="center" height="1" colspan="10">
                                        <hr>
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                        <div id="init_grid">
                            <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">

                                <colgroup>
                                    <col width="15%" />
                                    <col width="8%" />
                                    <col width="15%" />
                                    <col width="8%" />
                                    <col width="15%" />
                                    <col width="15%" />
                                    <col width="10%" />
                                    <col width="8%" />
                                    <col width="10%" />
                                </colgroup>

                                <tbody style="height: 25px;">
                                    <tr>
                                        <%--加班管理目標值 --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME_TARGET_VALUE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_TARGET_VALUE%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_OVERTIME_TARGET_VALUE" runat="server" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>

                                        </td>
                                        <%-- 部門加班實績(H) --%>
                                        <td align="right" class="Body_label">
                                            <asp:Label ID="lb_OVERTIME_TITLE1" runat="server" Text="<%$Resources:Resource,wfb2di080_lb_OVERTIME_TITLE1%>"></asp:Label></td>
                                        <%-- 部門加班實績(H)-平日 --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME1" runat="server" Text="<%$Resources:Resource,wfb2di080_lb_OVERTIME1%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_OVERTIME1" runat="server" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <%-- 平日換休 --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME2" runat="server" Text="<%$Resources:Resource,wfb2di080_lb_OVERTIME2%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_OVERTIME2" runat="server" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <%-- 換休率% --%>
                                        <td align="right" class="Body_label">
                                            <asp:Label ID="Label1" runat="server" Text="換休率%"></asp:Label></td>
                                        <%-- 平日 --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME9" runat="server" Text="<%$Resources:Resource,wfb2di080_lb_OVERTIME1%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_OVERTIME9" runat="server" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <%-- 部門勤務人數 --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_DEPT_EMPS" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_EMPS%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_DEPT_EMPS" runat="server" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <td align="right" class="Body_label"></td>
                                        <%-- 部門加班實績(H)-假日 --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME4" runat="server" Text="<%$Resources:Resource,wfb2di080_lb_OVERTIME4%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_OVERTIME4" runat="server" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <%-- 假日申告 --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME5" runat="server" Text="<%$Resources:Resource,wfb2di080_lb_OVERTIME5%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_OVERTIME5" runat="server" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <td align="right" class="Body_label"></td>
                                        <%-- 假日 --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME10" runat="server" Text="<%$Resources:Resource,wfb2di080_lb_OVERTIME4%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_OVERTIME10" runat="server" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <%-- 加班管理平均時數 --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME_AVG_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_AVG_HOUR%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_OVERTIME_AVG_HOUR" runat="server" Width="81px" BorderWidth="0" ClientIDMode="Static" Height="16px"></asp:TextBox>
                                        </td>
                                        <%-- 加班申請中(H) --%>
                                        <td align="right" class="Body_label">
                                            <asp:Label ID="lb_APPLY_OVERTIME_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di080_lb_APPLY_OVERTIME_HOUR%>"></asp:Label></td>
                                        <%-- 加班申請中(H) -平日 --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME7" runat="server" Text="<%$Resources:Resource,wfb2di080_lb_OVERTIME1%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_OVERTIME7" runat="server" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <%-- 申告未換休(年) --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME11" runat="server" Text="<%$Resources:Resource,wfb2di080_lb_OVERTIME11%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_OVERTIME11" runat="server" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <td align="right" class="Body_label"></td>
                                        <%-- 總計 --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME12" runat="server" Text="<%$Resources:Resource,wfb2di080_lb_OVERTIME12%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_OVERTIME12" runat="server" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right" class="Body_label"></td>
                                        <td align="right" class="Body_label"></td>
                                        <td align="right" class="Body_label"></td>
                                        <%-- 假日 --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME8" runat="server" Text="<%$Resources:Resource,wfb2di080_lb_OVERTIME4%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_OVERTIME8" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <td align="right" class="Body_label"></td>
                                        <td style="text-align: right" colspan="4">
                                            <input id="btn_Abnormal" runat="server" type="button" value="<%$Resources:Resource,wfb2di_lb_Abnormal%>" />
                                            <input id="btn_Abnormal2" runat="server" type="button" value="<%$Resources:Resource,wfb2di_lb_Abnormal2%>" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td colspan="10">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="10">
                                            <aces:Btn ID="WFB2DI0800Dtl" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0800Dtl%>" OnClick="WFB2DI0800Dtl_Click" ValidationGroup="GroupA" Visible="false" />

                                            <%--<asp:Button ID="WFB2DI0800Dtl" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0800Dtl%>" OnClick="WFB2DI0800Dtl_Click" ValidationGroup="GroupA" Visible="false" />--%>
                                        </td>
                                    </tr>



                                </tbody>

                            </table>
                        </div>


                    </td>
                </tr>


            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DI0800DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="dept_no" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="ddl_TARGET_TYPE" DefaultValue=""
                        Name="target_type" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_YM" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="ym" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1060px"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" Width="40px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="40px"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>"  HeaderStyle-Width="70px"  ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="DEPT_NO" HeaderText="<%$Resources:Resource,wfb2di_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="80px"  ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2di_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="70px"  ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2di_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="OVERTIME_CTL_DESC" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_CTL_CD%>" SortExpression="OVERTIME_CTL_CD" HeaderStyle-Width="240px" ItemStyle-Width="240px" HtmlEncode="false" ItemStyle-HorizontalAlign="Left" />
                    
                    <%-- 三高累計時數--%>
                    <asp:BoundField DataField="HYPER_HOUR" HeaderText="<%$Resources:Resource,wfb2di_HYPER_HOUR%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <%-- 一般累計時數--%>
                    <asp:BoundField DataField="NORMAL_HOUR" HeaderText="<%$Resources:Resource,wfb2di_NORMAL_HOUR%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    
                    <%--加班實績(月)-平日  --%>
                    <asp:BoundField DataField="A_APPROVE_OVERTIME_HOUR" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_TOTAL1%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <%--加班實績(月)-假日  --%>
                    <asp:BoundField DataField="B_APPROVE_OVERTIME_HOUR" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_TOTAL2%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <%--換休實績(月)-平日  --%>
                    <asp:BoundField DataField="Z_TOTAL_TIME_APPROVE" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_TOTAL3%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <%--換休實績(月)-假日  --%>
                    <asp:BoundField DataField="X_TOTAL_TIME_APPROVE" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_TOTAL4%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                     <%--未換休累計-平日  --%>
                    <asp:BoundField DataField="AZ_TOTAL_TIME_APPROVE" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_TOTAL5%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                     <%--未換休累計-假日(年)  --%>
                    <asp:BoundField DataField="BX_TOTAL_TIME_APPROVE" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_TOTAL6%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <%--假日加班(月)-已申告G --%>
                    <asp:BoundField DataField="X0_IS_APPLY_M" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_TOTAL7%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <%--假日加班(月)-未申告H --%>
                    <asp:BoundField DataField="BX_NOT_APPLY_M" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_TOTAL8%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />

                    <%--換休率% 平日 --%>
                    <asp:BoundField DataField="A_EXCHANG_RATE" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_TOTAL9%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <%--換休率% 假日 --%>
                    <asp:BoundField DataField="B_EXCHANG_RATE" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_TOTAL10%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <%--換休率%  合計 --%>
                    <asp:BoundField DataField="AB_EXCHANG_RATE" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_TOTAL11%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                </Columns>

                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
            </asp:GridView>

            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:Button ID="hid_getDEPT_NAME" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getDEPT_NAME_Click" />
            <asp:HiddenField ID="hid_dept_no_list" runat="server" ClientIDMode="Static" />
        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>
