<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0700_Qry.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0700_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".number").mask('999');

            $("#txt_EMP_NAME").attr("readonly", true);
            $("#txt_DEPT_NAME").attr("readonly", true);
            $("#txt_MAIN_LEAVE_DESC").attr("readonly", true);

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

            //Grid部門代號取得部門名稱的ajax
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
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F",
                freezesize: 4
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
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

        function getMainLeaveCD() {
            var json = OpenSearch('LeaveType_Search.aspx', 'txt_MAIN_LEAVE_CD', 'txt_MAIN_LEAVE_DESC', '', 'Y');
            if (json != undefined) {
                $("#txt_MAIN_LEAVE_CD").val(json.CD);
                $("#txt_MAIN_LEAVE_DESC").val(json.DESC);

                $("#hid_MAIN_LEAVE_CD").val(json.CD);
                $("#hid_MAIN_LEAVE_DESC").val(json.DESC);
            } else {
                $("#hid_MAIN_LEAVE_CD").val("");
                $("#hid_MAIN_LEAVE_DESC").val("");
            }
        }

        //清空畫面
        function ClearAll() {
            $("#txt_APPLY_LEAVE_SDT").val($("#hid_APPLY_LEAVE_SDT").val());
            $("#txt_APPLY_LEAVE_EDT").val($("#hid_APPLY_LEAVE_EDT").val());
            $("#txt_EMP_ID").val($("#hid_EMP_ID").val());
            $("#txt_EMP_NAME").val($("#hid_EMP_NAME").val());
            $("#txt_DEPT_NO").val($("#hid_DEPT_NO").val());
            $("#txt_DEPT_NAME").val($("#hid_DEPT_NAME").val());
            $("#txt_MAIN_LEAVE_CD").val("");
            $("#txt_MAIN_LEAVE_DESC").val("");
            $("#txt_LEVEL_CD_S").val("");
            $("#txt_LEVEL_CD_E").val("");
            $("#ddl_SUB_LEAVE_CD").empty(); //移除下拉選單
            $("#hid_IS_Clear").val("Y");
        }

        function getEmpName(EMP_id) {
            var txt = document.getElementById(EMP_id);
            if (txt.value != "") {
                document.getElementById('hid_getEmpName').click();
                return false;
            } else {
                $("#txt_EMP_NAME").val("");
                $("#txt_DEPT_NO").val("");
                $("#txt_DEPT_NAME").val("");
            }
        }

        function getDept() {
            var is_super = $("#hid_is_super").val();
            var json = OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', is_super);
            //if (json != undefined)
            //alert("上層部門代號:" + json.UP_DEPT_NO + "\n" + "上層部門名稱:" + json.UP_DEPT_NAME + "\n" + "部門主管工號:" + json.HEAD_EMP_ID + "\n" + "部門主管名稱:" + json.HEAD_EMP_NAME);
        }

        function getMAIN_LEAVE_DESC(MAIN_LEAVE_id) {
            var txt = document.getElementById(MAIN_LEAVE_id);
            document.getElementById('hid_getMAIN_LEAVE_DESC').click();
            return false;
        }

        function getLEVEL_CD_ToUpper(LEVEL_CD) {
            var txt = document.getElementById(LEVEL_CD);
            txt.value = txt.value.toUpperCase();  //轉大寫
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
            $('#hid_getMAIN_LEAVE_DESC').click();
            return false;
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
                                    <%-- 請假日期 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPLY_LEAVE_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_APPLY_LEAVE_SDT" runat="server" ClientIDMode="Static" CssClass="MandatoryField date" Width="80px"></asp:TextBox>~
                                        <asp:TextBox ID="txt_APPLY_LEAVE_EDT" runat="server" ClientIDMode="Static" CssClass="MandatoryField date" Width="80px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_APPLY_LEAVE_SDT2%>"
                                            ControlToValidate="txt_APPLY_LEAVE_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_APPLY_LEAVE_EDT2%>"
                                            ControlToValidate="txt_APPLY_LEAVE_EDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                                        <asp:CustomValidator ID="CustomValidatorAPPLY_LEAVE_SDT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_APPLY_LEAVE_SDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_APPLY_LEAVE_SDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidatorAPPLY_LEAVE_EDT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_APPLY_LEAVE_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_APPLY_LEAVE_EDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_APPLY_LEAVE_SDT"
                                            ControlToValidate="txt_APPLY_LEAVE_EDT" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_APPLY_LEAVE_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%-- 工號 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="80px" ClientIDMode="Static"></asp:TextBox>
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
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                        <input id="btn_DEPT_NO" type="button" value="..." onclick="getDept();" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%-- 主假別 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_MAIN_LEAVE_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_MAIN_LEAVE_CD" runat="server" MaxLength="2" Width="80px" ClientIDMode="Static" onblur="javascript:getMAIN_LEAVE_DESC(this.id)"></asp:TextBox>
                                        <asp:Button ID="btn_MAIN_LEAVE_CD" runat="server" Text="..." OnClientClick="getMainLeaveCD();" />
                                        <asp:TextBox ID="txt_MAIN_LEAVE_DESC" runat="server" BorderWidth="0" Width="80px" ClientIDMode="Static"></asp:TextBox>
                                    </td>

                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SUB_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_SUB_LEAVE_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SUB_LEAVE_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEVEL_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LEVEL_CD_S" runat="server" ClientIDMode="Static" Width="64px" MaxLength="3" onblur="javascript:getLEVEL_CD_ToUpper(this.id)"></asp:TextBox>~
                                        <asp:TextBox ID="txt_LEVEL_CD_E" runat="server" ClientIDMode="Static" Width="64px" MaxLength="3" onblur="javascript:getLEVEL_CD_ToUpper(this.id)"></asp:TextBox>
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_LEVEL_CD_S"
                                            ControlToValidate="txt_LEVEL_CD_E" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEVEL_CD_RANGE2%>" Type="String" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>

                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DH0700Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0700Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DH0700Search_Click" />
                                             <aces:Btn ID="WFB2SC4102LeaveSearch" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0700Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DH0700Search_Click" />
                                             <aces:Btn ID="WFB2DH0601Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0700Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DH0700Search_Click" />
                                             <aces:Btn ID="WFB2DH0407Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0700Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DH0700Search_Click" />
                                             <aces:Btn ID="WFB2DH0507Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0700Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DH0700Search_Click" />
                                            <%--<asp:Button ID="WFB2DH0700Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0700Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DH0700Search_Click" />--%>
                                            
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dh_btn_clear%>" onclick="ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td align="right" class="Body_label">
                        <hr>
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DH0700DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_APPLY_LEAVE_SDT"
                        Name="apply_leave_sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_APPLY_LEAVE_EDT"
                        Name="apply_leave_edt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_MAIN_LEAVE_CD"
                        Name="main_leave_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_SUB_LEAVE_CD" DefaultValue=""
                        Name="sub_leave_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_LEVEL_CD_S"
                        Name="level_cd_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_LEVEL_CD_E"
                        Name="level_cd_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
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
                OnRowDataBound="gv_result_RowDataBound" Width="1800px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 部門 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_DEPT_NO2%>" SortExpression="DEPT_NAME" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_MAIN_LEAVE_CD%>" SortExpression="MAIN_LEAVE_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text='<%#Bind("MAIN_LEAVE_CD")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%-- 子假別 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_SUB_LEAVE_CD%>" SortExpression="SUB_LEAVE_CD" HeaderStyle-Width="160px" ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_SUB_LEAVE_CD" runat="server" Text='<%#Bind("SUB_LEAVE_CD")%>' Width="160px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 事實發生日 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_FACT_HAPPEN_DT%>" SortExpression="APPLY_LEAVE_SDT" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_FACT_HAPPEN_DT" runat="server" Text='<%#Bind("FACT_HAPPEN_DT", "{0:yyyy/MM/dd}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 請假起日 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_APPLY_LEAVE_SDT%>" SortExpression="APPLY_LEAVE_SDT" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPLY_LEAVE_SDT" runat="server" Text='<%#Bind("APPLY_LEAVE_SDT")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_APPLY_LEAVE_STIME%>" SortExpression="APPLY_LEAVE_STIME" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPLY_LEAVE_STIME" runat="server" Text='<%#Bind("APPLY_LEAVE_STIME")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_APPLY_LEAVE_EDT%>" SortExpression="APPLY_LEAVE_EDT" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPLY_LEAVE_EDT" runat="server" Text='<%#Bind("APPLY_LEAVE_EDT")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_APPLY_LEAVE_ETIME1%>" SortExpression="APPLY_LEAVE_ETIME" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPLY_LEAVE_ETIME" runat="server" Text='<%#Bind("APPLY_LEAVE_ETIME")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_TOTAL_TIME_APPROVE%>" SortExpression="TOTAL_TIME_APPROVE" HeaderStyle-Width="140px">
                        <ItemTemplate>
                            <asp:Label ID="lb_TOTAL_TIME_APPROVE" runat="server" Text='<%#Bind("TOTAL_TIME_APPROVE")%>' Width="140px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_IFLOW_APPROVE_DT%>" SortExpression="IFLOW_APPROVE_DT" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_IFLOW_APPROVE_DT" runat="server" Text='<%#Bind("IFLOW_APPROVE_DT", "{0:yyyy/MM/dd}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_CHECK_STATUS%>" SortExpression="CHECK_STATUS" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CHECK_STATUS" runat="server" Text='<%#Bind("CHECK_STATUS")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_SALARY_SETTLE_STATUS%>" SortExpression="SALARY_SETTLE_STATUS" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_SETTLE_STATUS" runat="server" Text='<%#Bind("SALARY_SETTLE_STATUS")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_PAY_DT%>" SortExpression="PAY_DT" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_PAY_DT" runat="server" Text='<%#Bind("PAY_DT", "{0:yyyy/MM/dd}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_FORM_STATUS%>" SortExpression="FORM_STATUS" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_FORM_STATUS" runat="server" Text='<%#Bind("FORM_STATUS")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_IFLOW_NO%>" SortExpression="IFLOW_NO" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_IFLOW_NO" runat="server" Text='<%#Bind("IFLOW_NO")%>' Width="150px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dh_RowNumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DEPT_NO2%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2dh_MAIN_LEAVE_CD%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dh_SUB_LEAVE_CD%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2dh_APPLY_LEAVE_SDT%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2dh_APPLY_LEAVE_STIME%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2dh_APPLY_LEAVE_EDT%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2dh_APPLY_LEAVE_ETIME1%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2dh_TOTAL_TIME_APPROVE%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2dh_IFLOW_APPROVE_DT%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2dh_CHECK_STATUS%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="<%$Resources:Resource,wfb2dh_SALARY_SETTLE_STATUS%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2dh_PAY_DT%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="<%$Resources:Resource,wfb2dh_FORM_STATUS%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2dh_IFLOW_NO%>" Width="150px"></asp:Label>
                            </td>
                        </tr>
                    </table>

                </EmptyDataTemplate>
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
            <asp:HiddenField ID="hid_MAIN_LEAVE_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_MAIN_LEAVE_DESC" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_is_super" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_is_dept" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_departments" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_APPLY_LEAVE_SDT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_APPLY_LEAVE_EDT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_EMP_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_DEPT_NO" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_DEPT_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_IS_Clear" runat="server" ClientIDMode="Static" />
            <asp:Button ID="hid_getEmpName" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getEmpName_Click" />
            <asp:Button ID="hid_getMAIN_LEAVE_DESC" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getMAIN_LEAVE_DESC_Click" />

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

