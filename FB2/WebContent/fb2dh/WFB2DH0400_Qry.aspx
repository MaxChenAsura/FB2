<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0400_Qry.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0400_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask("9999/99/99");
            $(".ym").datepicker({ dateFormat: 'yy/mm' });
            $(".ym").mask("9999/99");
            $(".number2").mask("999");

            $("#txt_MAIN_LEAVE_CD").change(function () {
                $("#txt_MAIN_LEAVE_CD").val($("#txt_MAIN_LEAVE_CD").val().toUpperCase());
            });

            $('#txt_MAIN_LEAVE_DESC').attr("readonly", true);
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
                    $('#txt_DEPT_NAME').val("");
                }
            });
            gridviewScroll();
            $.unblockUI();

        }

        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 6

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");

        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function getEmp() {
            var json = OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');
            if (json != undefined) {
                $("#txt_DEPT_NAME").val(json.DEPT_NAME)
                $("#txt_DEPT_NO").val(json.DEPT_NO)
            }
            //alert("所屬部門代號:" + json.DEPT_NO + "\n" + "所屬部門名稱:" + json.DEPT_NAME + "\n" +
            //    "員工區分:" + json.EMP_CD + "\n" + "資格代號:" + json.LEVEL_CD + "\n" +
            //    "級數代號:" + json.GRADE_CD + "\n" + "職務代號:" + json.PJOB_CD + "\n" +
            //    "員工區分:" + json.JOIN_DT + "\n" + "資格代號:" + json.BE_EMP_DT + "\n" +
            //    "職種:" + json.WS_CD + "\n" + "在職狀態:" + json.EMP_STATUS + "\n");
        }
        //清空畫面
        function ClearAll() {
            $("#txt_APPLY_LEAVE_SDT").val("");
            $("#txt_APPLY_LEAVE_EDT").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#txt_MAIN_LEAVE_CD").val("");
            $("#txt_MAIN_LEAVE_DESC").val("");
            $("#ddl_SUB_LEAVE_CD").val("-1");
            $("#txt_IFLOW_NO").val("");
            $("#txt_IFLOW_APPROVE_DT").val("");
            $("#ddl_CHECK_STATUS").val("-1");
            $("#ddl_FORM_STATUS").val("-1");
            __doPostBack('<%#txt_MAIN_LEAVE_CD.ClientID %>', 'OnTextChanged');
        }

        function getMAIN_LEAVE_CD() {
            var json = OpenSearch('LeaveType_Search.aspx', 'txt_MAIN_LEAVE_CD', 'txt_MAIN_LEAVE_DESC', '', 'Y');
            __doPostBack('<%#txt_MAIN_LEAVE_CD.ClientID %>', 'OnTextChanged');
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb299_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }


        //主假別用視窗挑選回傳的資料
        function LeaveType_Search(id, name, json) {
            var returnValue = json;
            if (json == undefined) {
                returnValue = 'undefined';
            }
            //alert(returnValue);
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#txt_MAIN_LEAVE_CD").val(obj.CD);
                $("#txt_MAIN_LEAVE_DESC").val(obj.DESC);
            } else {

            }
            __doPostBack('leaveType', '');
        }
    </script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <%--查詢條件欄位寬度--%>
                <colgroup>
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="28%" />
                    <col width="10%" />
                    <col width="22%" />
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<% $Resources:Resource,wfb2dh_Required_APPLY_LEAVE_DT %> "
                                ControlToValidate="txt_APPLY_LEAVE_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<% $Resources:Resource,wfb2dh_Required_APPLY_LEAVE_DT2 %>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_LEAVE_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            ~
                    <asp:TextBox ID="txt_APPLY_LEAVE_EDT" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="81px" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<% $Resources:Resource,wfb2dh_Required_APPLY_LEAVE_DT %> "
                                ControlToValidate="txt_APPLY_LEAVE_EDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="qrytest2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<% $Resources:Resource,wfb2dh_Required_APPLY_LEAVE_DT2 %>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_LEAVE_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_APPLY_LEAVE_SDT"
                                ControlToValidate="txt_APPLY_LEAVE_EDT" ErrorMessage="<%$Resources:Resource,wfb2dh_CP_APPLY_LEAVE_DT%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </td>
                    </tr>
                    <%--工號(...)、姓名、部門(...)--%>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">

                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_HEAD_EMP_ID" type="button" value="..." onclick="getEmp();" />
                            <asp:TextBox ID="txt_EMP_NAME" Width="80px" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DEPT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                            <asp:TextBox ID="txt_DEPT_NAME" Width="150px" runat="server" MaxLength="6" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>

                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_FORM_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_FORM_STATUS%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_FORM_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <%--主假別(...)、子假別(ddl)、申請單號--%>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_MAIN_LEAVE_CD" runat="server" MaxLength="6" Width="40px" ClientIDMode="Static" OnTextChanged="txt_MAIN_LEAVE_CD_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <input id="Button1" type="button" value="..." onclick="getMAIN_LEAVE_CD();" />
                            <asp:TextBox ID="txt_MAIN_LEAVE_DESC" runat="server" MaxLength="10" Width="100px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SUB_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_SUB_LEAVE_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IFLOW_NO%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_IFLOW_NO" runat="server" MaxLength="18" Width="169px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <%--核准年月、刷卡比對狀態(ddl)--%>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IFLOW_APPROVE_YM%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_IFLOW_APPROVE_DT" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static" CssClass="ym"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CHECK_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_CHECK_STATUS%>"></asp:Label>:
                        </th>
                        <td align="left" class="auto-style1">
                            <asp:DropDownList ID="ddl_CHECK_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <%--查詢清除Button--%>
                    <tr>
                        <td align="right" colspan="10">
                            <div id="init_grid" style="display: inline">

                                <aces:Btn ID="WFB2DH0400Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Search%>" OnClick="WFB2DH0400Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                <aces:Btn ID="WFB2DH0400AddBatch" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400AddBatch%>" OnClick="WFB2DH0400AddBatch_Click" />
                                <%--
                                <asp:Button ID="WFB2DH0400Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Search%>" OnClick="WFB2DH0400Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                <asp:Button ID="WFB2DH0400AddBatch" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400AddBatch%>" OnClick="WFB2DH0400AddBatch_Click" />
                                --%>
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

                            <aces:Btn ID="WFB2DH0400Add" runat="server" Text="<%$resources:resource,wfb2dh_WFB2DH0400Add%>" OnClick="WFB2DH0400Add_Click" />
                            <aces:Btn ID="WFB2DH0400Edit" runat="server" Text="<%$resources:resource,wfb2dh_WFB2DH0400Edit%>" OnClick="WFB2DH0400Edit_Click" Visible="false" />
                            <aces:Btn ID="WFB2DH0400Delete" runat="server" Text="<%$resources:resource,wfb2dh_WFB2DH0400Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DH0400Delete_Click" Visible="false" Style="margin-top: 0px;" />
                            <aces:Btn ID="WFB2DH0400Detail" runat="server" Text="<%$resources:resource,wfb2dh_WFB2DH0400Detail%>" Visible="false" OnClick="WFB2DH0400Detail_Click" />
                        </td>
                    </tr>


                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DH0400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_APPLY_LEAVE_SDT" Name="apply_leave_sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_APPLY_LEAVE_EDT" Name="apply_leave_edt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO" Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME" Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_MAIN_LEAVE_CD" Name="main_leave_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_SUB_LEAVE_CD" DefaultValue="" Name="sub_leave_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_IFLOW_APPROVE_DT" Name="iflow_approve_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_CHECK_STATUS" DefaultValue="" Name="check_status" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_IFLOW_NO" Name="iflow_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_FORM_STATUS" DefaultValue=""
                        Name="form_status" PropertyName="SelectedValue" Type="String" />

                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1495px"
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="MAIN_LEAVE_DESC" HeaderText="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>" SortExpression="MAIN_LEAVE_DESC" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <%--子假別 --%>
                    <asp:BoundField DataField="SUB_LEAVE_DESC" HeaderText="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD%>" SortExpression="SUB_LEAVE_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <%--表單狀態 --%>
                    <asp:BoundField DataField="FORM_STATUS" HeaderText="<%$Resources:Resource,wfb2dh_lb_FORM_STATUS%>" SortExpression="FORM_STATUS" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <%--事實發生日 --%>
                    <asp:BoundField DataField="FACT_HAPPEN_DT" HeaderText="<%$Resources:Resource,wfb2dh_lb_FACT_HAPPEN_DT%>" SortExpression="FACT_HAPPEN_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%--請假勤務日-起 --%>
                    <asp:BoundField DataField="APPLY_LEAVE_SDT" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_SDT%>" SortExpression="APPLY_LEAVE_SDT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%--開始時間 --%>
                    <asp:BoundField DataField="APPLY_LEAVE_STIME" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_STIME%>" SortExpression="APPLY_LEAVE_STIME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <%--請假勤務日-迄 --%>
                    <asp:BoundField DataField="APPLY_LEAVE_EDT" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_EDT%>" SortExpression="APPLY_LEAVE_EDT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%--結束時間 --%>
                    <asp:BoundField DataField="APPLY_LEAVE_ETIME" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_ETIME%>" SortExpression="APPLY_LEAVE_ETIME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <%--請假合計 --%>
                    <asp:BoundField DataField="TOTAL_TIME_APPROVE" HeaderText="<%$Resources:Resource,wfb2dh_lb_TOTAL_TIME_APPROVE%>" SortExpression="TOTAL_TIME_APPROVE" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <%--申請單號 --%>
                    <asp:BoundField DataField="IFLOW_NO" HeaderText="<%$Resources:Resource,wfb2dh_lb_IFLOW_NO%>" SortExpression="IFLOW_NO" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left" />
                    <%--核淮年月 --%>
                    <asp:BoundField DataField="IFLOW_APPROVE_DT" HeaderText="<%$Resources:Resource,wfb2dh_lb_IFLOW_APPROVE_YM%>" SortExpression="IFLOW_APPROVE_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%--刷卡比對狀態 --%>
                    <asp:BoundField DataField="CHECK_STATUS" HeaderText="<%$Resources:Resource,wfb2dh_lb_CHECK_STATUS%>" SortExpression="CHECK_STATUS" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    
                    <%--部門 --%>
                    <asp:BoundField DataField="DEPT_NO" HeaderText="<%$Resources:Resource,wfb2dh_lb_DEPT%>" SortExpression="DEPT_NO" HeaderStyle-Width="185px" ItemStyle-Width="185px" ItemStyle-HorizontalAlign="Left" />
                               
                </Columns>

                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
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
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
