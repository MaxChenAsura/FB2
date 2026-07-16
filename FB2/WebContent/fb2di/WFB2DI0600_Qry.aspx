<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0600_Qry.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0600_Qry" Culture="auto" UICulture="auto" %>
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
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date2").mask('9999/99');

            $("#allUpdate1").hide();
            $("#allUpdate2").hide();

            $("#txt_EMP_NAME").attr("readonly", true);
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
                                //$('#txt_DEPT_NO').val("");
                                //$('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));
                                //$('#txt_DEPT_NO').val(JData.DEPT_NO);
                               // $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME').val("");
                    //$('#txt_DEPT_NO').val("");
                    //$('#txt_DEPT_NAME').val("");
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
                freezesize: 6
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        function doUpdate() {
            if (LookUpCheckboxs() > 0) {
                $("#allUpdate1").toggle();
                $("#allUpdate2").toggle();
            }
            else {
                alert("請選擇資料");
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function getEmp() {
            var json = OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'Y');
            if (json != undefined) {
               
            }
        }

        function getDept() {
            var json = OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'Y');
        }

        //清空畫面
        function ClearAll() {
            $("#txt_APPLY_OVERTIME_SDT").val("");
            $("#txt_APPLY_OVERTIME_EDT").val("");
            $("#txt_CREATED_SDT").val("");
            $("#txt_CREATED_EDT").val("");
            $("#ddl_DT_TYPE").val("-1");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_OVERTIME_CD").val("-1");
            $("#ddl_O_SPECIAL_CD").val("-1");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#txt_IFLOW_NO").val("");
            $("#txt_IFLOW_APPROVE_DT").val("");
            $("#ddl_SALARY_SETTLE_STATUS").val("-1");
            $("#txt_PAY_DT").val("");
            $("#ddl_FORM_STATUS").val("-1");
        }

        function checkConfirm() {
            var processed = true;
            if (Page_ClientValidate("GroupB")) {
                var result = confirm("確定要更新?");
                if (result) {
                    BlockUI();
                    return true;
                }
                else {
                    $("#allUpdate1").hide();
                    $("#allUpdate2").hide();
                    $.unblockUI();
                    return false;
                }
            }
            else
                processed = false;

            if (!processed)
                $.unblockUI();

            return processed;

        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm("確定要註銷?");
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function getEmpName(EMP_id) {
            var txt = document.getElementById(EMP_id);
            if (txt.value != "") {
                document.getElementById('hid_getEmpName').click();
                return false;
            }
        }

        function checkDowning() {
            if (Page_ClientValidate("GroupA")) {

            }
            else {
                return false;
            }
            BlockUI();
            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
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
                                <col width="25%" />
                                <col width="10%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <!-- 加班日期 -->
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_DT2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_APPLY_OVERTIME_SDT" runat="server" ClientIDMode="Static" CssClass="MandatoryField date" Width="80px"></asp:TextBox>~
                                        <asp:TextBox ID="txt_APPLY_OVERTIME_EDT" runat="server" ClientIDMode="Static" CssClass="MandatoryField date" Width="80px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_APPLY_OVERTIME_SDT%>"
                                            ControlToValidate="txt_APPLY_OVERTIME_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_APPLY_OVERTIME_EDT%>"
                                            ControlToValidate="txt_APPLY_OVERTIME_EDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorAPPLY_OVERTIME_SDT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_APPLY_OVERTIME_SDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_APPLY_OVERTIME_SDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidatorAPPLY_OVERTIME_EDT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_APPLY_OVERTIME_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_APPLY_OVERTIME_EDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_APPLY_OVERTIME_SDT"
                                            ControlToValidate="txt_APPLY_OVERTIME_EDT" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_APPLY_OVERTIME_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                     <!-- 建立日期 -->
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_txt_CREATED_DT" runat="server" Text="建立日期"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CREATED_SDT" runat="server" ClientIDMode="Static" CssClass="date" Width="80px"></asp:TextBox>~
                                        <asp:TextBox ID="txt_CREATED_EDT" runat="server" ClientIDMode="Static" CssClass="date" Width="80px"></asp:TextBox>
                                         <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="建立日期(起)格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_CREATED_SDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="建立日期(迄)格式錯誤>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_CREATED_EDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_CREATED_SDT"
                                            ControlToValidate="txt_CREATED_EDT" ErrorMessage="建立日期起不能大於建立日期迄" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                    <!-- 日期類型 -->
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DT_TYPE" runat="server" Text="日期類型"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_DT_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>


                                </tr>
                                <tr>
                                    <!-- 工號 -->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="80px" ClientIDMode="Static" ></asp:TextBox>
                                        <input id="bt_EMP_ID" type="button" value="..." onclick="getEmp();" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="80px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <!-- 部門 -->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                        <input id="btn_DEPT_NO" type="button" value="..." onclick="getDept();" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="120px"></asp:TextBox>
                                    </td>
                                    <!-- 表單狀態 -->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_FORM_STATUS" runat="server" Text="<%$Resources:Resource,wfb2di_lb_FORM_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_FORM_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <!-- 加班類型 -->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_OVERTIME_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <!-- 加班特殊狀況 -->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_O_SPECIAL_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_O_SPECIAL_CD%>" ClientIDMode="Static"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
<%--                                        <asp:DropDownList ID="ddl_OVERTIME_TIME_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>--%>
                                        <asp:DropDownList ID="ddl_O_SPECIAL_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <%--申請單號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IFLOW_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_IFLOW_NO" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--核准年月--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IFLOW_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IFLOW_APPROVE_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_IFLOW_APPROVE_DT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date2"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_IFLOW_APPROVE_DATE%>" ControlToValidate="txt_IFLOW_APPROVE_DT" ForeColor="Red"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <%--計薪狀態--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_SETTLE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2di_lb_SALARY_SETTLE_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_SETTLE_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <%--發薪日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_PAY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PAY_DT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorPAY_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_PAY_DATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_PAY_DT" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
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
                                            <aces:Btn ID="WFB2DI0600Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0600Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DI0600Search_Click" />

                                            <%--<asp:Button ID="WFB2DI0600Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0600Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DI0600Search_Click" />--%>
                                            
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2di_btn_clear%>" onclick="ClearAll();" />
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
                        <div id="init_grid">
                            <aces:Btn ID="WFB2DI0600Add" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0600Add%>" OnClick="WFB2DI0600Add_Click" />
                            <aces:Btn ID="WFB2DI0600Cancel" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0600Cancel%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DI0600Cancel_Click" />
                            <aces:Btn ID="WFB2DI0600Edit" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0600Edit%>" Visible="false" OnClick="WFB2DI0600Edit_Click" />
                            <aces:Btn ID="WFB2DI0600Detail" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0600Detail%>" Visible="false" OnClick="WFB2DI0600Detail_Click" />
                            <aces:Btn ID="WFB2DI0600BatchEdit" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0600BatchEdit%>" visible="false" OnClick="WFB2DI0600BatchEdit_Click"  />
                            <aces:Btn ID="WFB2DI0600ExportXLS" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0600ExportXLS%>" Visible="false" OnClick="WFB2DI0600ExportXLS_Click" OnClientClick="return checkDowning();"/>
                            <asp:Button ID="WFB2DI0600Upload" runat="server" Text="上傳" OnClick="WFB2DI0600Upload_Click" />
                            <asp:Button ID="WFB2DI0600Export" runat="server" Text="假日加班匯出" Visible="false" OnClick="WFB2DI0600Export_Click" OnClientClick="return checkDowning();"/>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="allUpdate1">
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_PAY_DT2" runat="server" Text="<%$Resources:Resource,wfb2di_lb_PAY_DT2%>" Width="95px"></asp:Label>
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_PAY_DT2" runat="server" CssClass="date" Width="80px"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorPAY_DT2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_PAY_DATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_PAY_DT2" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="allUpdate2">
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_REMARK2" runat="server" Text="<%$Resources:Resource,wfb2di_lb_REMARK2%>" Width="95px"></asp:Label>
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_REMARK2" runat="server" MaxLength="200"></asp:TextBox>
                                    <asp:Button ID="bt_BatchEdit" runat="server" Text="<%$Resources:Resource,wfb2di_bt_BatchEdit%>" OnClientClick="return checkConfirm();" OnClick="bt_BatchEdit_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>

                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DI0600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_APPLY_OVERTIME_SDT"
                        Name="apply_overtime_sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_APPLY_OVERTIME_EDT"
                        Name="apply_overtime_edt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_OVERTIME_CD" DefaultValue=""
                        Name="overtime_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_O_SPECIAL_CD" DefaultValue=""
                        Name="o_special_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_IFLOW_NO"
                        Name="iflow_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_IFLOW_APPROVE_DT"
                        Name="iflow_approve_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_SALARY_SETTLE_STATUS" DefaultValue=""
                        Name="salary_settle_status" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_PAY_DT"
                        Name="pay_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_FORM_STATUS" DefaultValue=""
                        Name="form_status" PropertyName="SelectedValue" Type="String" />
                     <asp:ControlParameter ControlID="txt_CREATED_SDT"
                        Name="created_sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_CREATED_EDT"
                        Name="created_edt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_DT_TYPE" DefaultValue=""
                        Name="dt_type" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1800px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--勤務日期 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_DT3%>" SortExpression="APPLY_OVERTIME_DT" HeaderStyle-Width="100px"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text='<%#Bind("APPLY_OVERTIME_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--加班類型 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_OVERTIME_CD%>" SortExpression="OVERTIME_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_OVERTIME_CD" runat="server" Text='<%#Bind("OVERTIME_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--加班出勤別 
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_OVERTIME_DT_TYPE3%>" SortExpression="OVERTIME_DT_TYPE" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_OVERTIME_DT_TYPE" runat="server" Text='<%#Bind("OVERTIME_DT_TYPE")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                        --%>
                    <%--勤務日期類型 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_DT_TYPE2%>" HeaderStyle-Width="120px" SortExpression="DT_TYPE" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                             <asp:Label ID="lb_DT_TYPE" runat="server" Text='<%#Bind("DT_TYPE_DESC")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--班別 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_SHIFT_CD%>" HeaderStyle-Width="200px" SortExpression="SHIFT_CD" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                             <asp:Label ID="lb_SHIFT_CD" runat="server" Text='<%#Bind("SHIFT_CD_DESC")%>' Width="200px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--申請總時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_APPLY_OVERTIME_HOUR%>" SortExpression="APPLY_OVERTIME_HOUR" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPLY_OVERTIME_HOUR" runat="server" Text='<%#Bind("APPLY_OVERTIME_HOUR")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--核准總時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_APPROVE_OVERTIME_HOUR%>" SortExpression="O_APPROVE_OVERTIME_HOUR" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_OVERTIME_HOUR" runat="server" Text='<%#Bind("APPROVE_OVERTIME_HOUR")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--計算總時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_PAY_HOUR2%>" SortExpression="OVERTIME_PAY_HOUR" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_OVERTIME_PAY_HOUR" runat="server" Text='<%#Bind("OVERTIME_PAY_HOUR")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--勤前時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_BEFORE_HOUR%>" SortExpression="BEFORE_HOUR" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_BEFORE_HOUR" runat="server" Text='<%#Bind("BEFORE_HOUR")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--勤前起迄時間 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_BEFORE_TIME%>" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_BEFORE_TIME" runat="server" Width="120px"></asp:Label>
                            <asp:HiddenField ID="hid_BEFORE_STIME" runat="server" Value='<%#Bind("BEFORE_STIME", "{0:HH:mm}")%>' />
                            <asp:HiddenField ID="hid_BEFORE_ETIME" runat="server" Value='<%#Bind("BEFORE_ETIME", "{0:HH:mm}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--勤後時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_AFTER_HOUR%>" SortExpression="AFTER_HOUR" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_AFTER_HOUR" runat="server" Text='<%#Bind("AFTER_HOUR")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--勤後起迄時間 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_AFTER_TIME%>" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_AFTER_TIME" runat="server" Width="120px"></asp:Label>
                            <asp:HiddenField ID="hid_AFTER_STIME" runat="server" Value='<%#Bind("AFTER_STIME", "{0:HH:mm}")%>' />
                            <asp:HiddenField ID="hid_AFTER_ETIME" runat="server" Value='<%#Bind("AFTER_ETIME", "{0:HH:mm}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--核淮年月 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_IFLOW_APPROVE_DT%>" SortExpression="IFLOW_APPROVE_DT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_IFLOW_APPROVE_DT" runat="server" Text='<%#Bind("IFLOW_APPROVE_DT", "{0:yyyy/MM}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--計薪狀態 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_SALARY_SETTLE_STATUS%>" SortExpression="SALARY_SETTLE_STATUS" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_SETTLE_STATUS" runat="server" Text='<%#Bind("SALARY_SETTLE_STATUS")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發薪日期 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_PAY_DT%>" SortExpression="PAY_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_PAY_DT" runat="server" Text='<%#Bind("PAY_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--是否刷卡比對 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_IS_DUTY_CHECK%>" SortExpression="IS_DUTY_CHECK" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_DUTY_CHECK" runat="server" Text='<%#Bind("IS_DUTY_CHECK")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <%--刷卡比對狀態 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_CHECK_STATUS%>" SortExpression="CHECK_STATUS" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CHECK_STATUS" runat="server" Text='<%#Bind("CHECK_STATUS")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <%--表單狀態 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_FORM_STATUS%>" SortExpression="FORM_STATUS" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_FORM_STATUS" runat="server" Text='<%#Bind("FORM_STATUS")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                  
                    <%--申請單號 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_IFLOW_NO%>" SortExpression="IFLOW_NO" HeaderStyle-Width="150px"  ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_IFLOW_NO" runat="server" Text='<%#Bind("IFLOW_NO")%>' Width="150px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工數區分 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_WORK_CD%>" SortExpression="WORK_CD" HeaderStyle-Width="150px"  ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_CD" runat="server" Text='<%#Bind("WORK_CD_DESC")%>' Width="150px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部門 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>" SortExpression="DEPT_NAME" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME")%>' Width="200px"></asp:Label>
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
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <asp:Button ID="hid_getEmpName" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getEmpName_Click" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DI0600ExportXLS" />
            <asp:PostBackTrigger ControlID="WFB2DI0600Export" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>

