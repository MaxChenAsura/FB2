<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0500_Qry.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0500_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date1").datepicker({ dateFormat: 'yy/mm' });
            $('#txt_MAIN_LEAVE_DESC').attr("readonly", true);

            $(".date1").mask("9999/99");
            $(".date").mask("9999/99/99");
            $(".number2").mask("999");
            $('#txt_EMP_NAME').attr("readonly", true);
            $("#txt_MAIN_LEAVE_CD").change(function () {
                $("#txt_MAIN_LEAVE_CD").val($("#txt_MAIN_LEAVE_CD").val().toUpperCase());
            });

            //工號取得姓名的ajax
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        cache: false,
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
            $('#txt_tree_DEPT_NAME').attr("readonly", true);
            //部門代號取得部門名稱的ajax
            $("#txt_tree_DEPT_NO").change(function () {
                if ($("#txt_tree_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_tree_DEPT_NO').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_tree_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_tree_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_tree_DEPT_NAME').val("");
                }
            });
            gridviewScroll();
            $.unblockUI();
        }//月曆
        //清空畫面
        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 2

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
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_tree_DEPT_NO").val("");
            $("#txt_tree_DEPT_NAME").val("");
            $("#txt_MAIN_LEAVE_CD").val("");
            $("#txt_MAIN_LEAVE_DESC").val("");
            //$("#ddl_SUB_LEAVE_CD").val("-1");
            $("#ddl_SUB_LEAVE_CD").empty();
            $("#txt_IFLOW_NO").val("");
            $("#txt_IFLOW_APPROVE_DT").val("");
            $("#ddl_CHECK_STATUS").val("-1");
            $("#txt_PAY_DT").val("");
            $("#ddl_FORM_STATUS").val("-1");
            $("#ddl_SALARY_SETTLE_STATUS").val("-1");
            //__doPostBack('<%#txt_MAIN_LEAVE_CD.ClientID %>', 'OnTextChanged');
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
        function getDept() {
            var json = OpenDeptSearch('txt_tree_DEPT_NO', 'txt_tree_DEPT_NAME', 'N');
        }

        function openallUpdate2() {
            if (LookUpCheckboxs() > 0) {
                $("#allUpdate2").show();
            }
            else {
                alert("請選擇資料");
            }
        }

        function hideAllUpdate2() {
            $("#allUpdate2").hide();
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

        function getMAIN_LEAVE_CD() {
            var json = OpenSearch('LeaveType_Search.aspx', 'txt_MAIN_LEAVE_CD', 'txt_MAIN_LEAVE_DESC', '','Y');
            //__doPostBack('<%#txt_MAIN_LEAVE_CD.ClientID %>', 'OnTextChanged');
        }

        function checkBatchconfirm(checkconfirmmessage) {
            var ret = confirm(checkconfirmmessage);
            if (ret) {
                BlockUI();
                __doPostBack('batch', '');

            }
        }

        function checkDeleteconfirm(checkconfirmmessage) {
            var ret = confirm(checkconfirmmessage);
            if (ret) {
                BlockUI();
                __doPostBack('delete', '');

            }
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm("確定要註銷?");
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckBatchAction() {
            if (LookUpCheckboxs() > 0)
                return confirm("確定要更新?");
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
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
        //檔案匯入檢驗
        function checkUpload(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                //BlockUI();
            }
            else {
                processed = false;
                return false;
            }

            BlockUI();
            //processed = confirm("確定要進行?");
            //if (!processed) {
            //    $.unblockUI();
            //}
            return processed;
        }


        function doUnBlock() {
            $.unblockUI();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">

                <colgroup>
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="29%" />
                    <col width="10%" />
                    <col width="19%" />
                </colgroup>
                <tbody>
                    <tr>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_APPLY_LEAVE_SDT"
                            ControlToValidate="txt_APPLY_LEAVE_EDT" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_CALENDAR_DT%>" Type="Date" Operator="GreaterThanEqual"
                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:TextBox ID="txt_APPLY_LEAVE_SDT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_APPLY_LEAVE_SDT%>"
                                ControlToValidate="txt_APPLY_LEAVE_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_APPLY_LEAVE_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_LEAVE_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            ~
                                <asp:TextBox ID="txt_APPLY_LEAVE_EDT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_APPLY_LEAVE_EDT%>"
                                ControlToValidate="txt_APPLY_LEAVE_EDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_APPLY_LEAVE_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_LEAVE_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                        </td>

                    </tr>
                    <tr>
                        <%-- 工號  --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="90px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                        </td>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DEPT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_tree_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="getDept();" />
                            <asp:TextBox ID="txt_tree_DEPT_NAME" runat="server" Width="140px" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_FORM_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_FORM_STATUS%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_FORM_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>

                    <tr>
                        <%-- 主假別  --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_MAIN_LEAVE_CD" runat="server" MaxLength="2" Width="64px" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="txt_MAIN_LEAVE_CD_TextChanged"></asp:TextBox>
                            <input id="Button1" type="button" value="..." onclick="getMAIN_LEAVE_CD();" />
                            <asp:TextBox ID="txt_MAIN_LEAVE_DESC" runat="server" MaxLength="10" Width="50px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="SUB_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD%>"></asp:Label>:</th>

                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_SUB_LEAVE_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IFLOW_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_IFLOW_NO" runat="server" Width="150px" ClientIDMode="Static" MaxLength="18"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IFLOW_APPROVE_YM%>"></asp:Label>
                            :</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_IFLOW_APPROVE_DT" runat="server" ClientIDMode="Static" CssClass="date1" MaxLength="10" Width="81px"> </asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="SALARY_SETTLE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SALARY_SETTLE_STATUS%>"></asp:Label>
                            :</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_SALARY_SETTLE_STATUS" runat="server" ClientIDMode="Static">
                            </asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PAY_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_PAY_DT%>"></asp:Label>
                            :</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_PAY_DT" runat="server" ClientIDMode="Static" CssClass="date" Width="113px"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_PAY_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_PAY_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>
                    </tr>

                </tbody>
            </table>

            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="12%" />
                    <col width="10%" />
                    <col width="10%" />
                    <col width="11%" />
                    <col width="50%" />
                </colgroup>

                <tbody>

                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <aces:Btn ID="WFB2DH0500Search" runat="server" ValidationGroup="GroupA" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500Search%>" OnClick="WFB2DH0500Search_Click" OnClientClick="CheckValid();" />

                                <%--<asp:Button ID="WFB2DH0500Search" runat="server" ValidationGroup="GroupA" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500Search%>" OnClick="WFB2DH0500Search_Click" OnClientClick="CheckValid();" />--%>
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dh_btn_clear%>" onclick="ClearAll();" />
                            </div>
                        </td>
                    </tr>


                    <!-- end: Create MODULE ID -->
                    <!-- START: Create a line to separate Search field with body field -->
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="10" >
                            <div id="init_grid">
                                <aces:Btn ID="WFB2DH0500Add" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500Add%>" OnClick="WFB2DH0500Add_Click" />
                                <aces:Btn ID="WFB2DH0500Cancel" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500Cancel%>" OnClick="WFB2DH0500Cancel_Click" Visible="False" OnClientClick="return CheckDelAction();" />
                                <aces:Btn ID="WFB2DH0500Edit" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500Edit%>" OnClick="WFB2DH0500Edit_Click" Visible="False" />
                                <aces:Btn ID="WFB2DH0500Detail" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500Detail%>" OnClick="WFB2DH0500Detail_Click" Visible="False" />
                                <aces:Btn ID="WFB2DH0500ExportXLS" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500ExportXLS%>" ValidationGroup="GroupA" Visible="False" OnClick="WFB2DH0500ExportXLS_Click" OnClientClick="return checkUpload();" />
                                <aces:Btn id="WFB2DH0500BatchEdit" runat="server"  Text="<%$Resources:Resource,wfb2dh_WFB2DH0500BatchEdit%>" Onclick="WFB2DH0500BatchEdit_Click1" visible="false" />   

                                <%--
                                 <input id="WFB2DH0500BatchEdit" runat="server" type="button" value="<%$Resources:Resource,wfb2dh_WFB2DH0500BatchEdit%>" onclick="openallUpdate2();" visible="false" />   

                                    <asp:Button ID="WFB2DH0500Add" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500Add%>" OnClick="WFB2DH0500Add_Click" />
                                <asp:Button ID="WFB2DH0500Cancel" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500Cancel%>" OnClick="WFB2DH0500Cancel_Click" Visible="False" OnClientClick="return CheckDelAction();" />
                                <asp:Button ID="WFB2DH0500Edit" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500Edit%>" OnClick="WFB2DH0500Edit_Click" Visible="False" />
                                <asp:Button ID="WFB2DH0500Detail" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500Detail%>" OnClick="WFB2DH0500Detail_Click" Visible="False" />
                                <asp:Button ID="WFB2DH0500ExportXLS" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500ExportXLS%>" ValidationGroup="GroupA" Visible="False" OnClick="WFB2DH0500ExportXLS_Click" OnClientClick="return checkUpload();" />--%>
                                <%--<asp:Button ID="WFB2DH0500BatchEdit" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500BatchEdit%>" OnClientClick="openallUpdate2(); return confirm('確定要註銷?');"  Visible="False"  />--%>
                                
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table id="allUpdate2" cellspacing="1" cellpadding="1" width="400" border="0" class="Body_Label" style="display: none">
                                <colgroup>
                                    <col width="4%" />
                                    <col width="12%" />

                                </colgroup>
                                <tbody>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_paydt" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_paydt%>" Width="80px"></asp:Label></th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_paydt" runat="server" ClientIDMode="Static" CssClass="date" Width="80px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_REMARK2%>" Width="80px"></asp:Label></th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_REMARK" runat="server" ClientIDMode="Static" MaxLength="200"></asp:TextBox>
                                            <aces:Btn ID="WFB2DH0500Save" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500Save%>" OnClick="WFB2DH0500BatchEdit_Click" OnClientClick="return CheckBatchAction();" />
                                            <asp:Button ID="WFB2DH0501Cancel" runat="server" Text="取消"  OnClick="WFB2DH0501Cancel_Click" />
                                            <%--<asp:Button ID="WFB2DH0500Save" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0500Save%>" OnClick="WFB2DH0500BatchEdit_Click" OnClientClick="return CheckBatchAction();" />--%>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>

                    </tr>

                    <!-- END: Create a line -->
                </tbody>
            </table>



            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DH0500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_APPLY_LEAVE_SDT"
                        Name="apply_leave_sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_APPLY_LEAVE_EDT"
                        Name="apply_leave_edt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="emp_name" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_tree_DEPT_NO" DefaultValue=""
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_MAIN_LEAVE_CD" DefaultValue=""
                        Name="main_leave_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_SUB_LEAVE_CD" DefaultValue=""
                        Name="sub_leave_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_IFLOW_NO" DefaultValue=""
                        Name="iflow_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_IFLOW_APPROVE_DT" DefaultValue=""
                        Name="iflow_approve_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_SALARY_SETTLE_STATUS" DefaultValue=""
                        Name="salary_settle_status" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_PAY_DT"
                        Name="pay_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_FORM_STATUS" DefaultValue=""
                        Name="form_status" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1590px"
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2dh_RowNumber%>" HeaderStyle-Width="40px" />
                    
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="MAIN_LEAVE_CD" HeaderText="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>" SortExpression="MAIN_LEAVE_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <%--子假別 --%>
                    <asp:BoundField DataField="SUB_LEAVE_CD_DESC" HeaderText="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD%>" SortExpression="SUB_LEAVE_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <%--表單狀態 --%>
                    <asp:BoundField DataField="FORM_STATUS" HeaderText="<%$Resources:Resource,wfb2dh_lb_FORM_STATUS%>" SortExpression="FORM_STATUS" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <%--事實發生日 --%>
                    <asp:BoundField DataField="FACT_HAPPEN_DT" HeaderText="<%$Resources:Resource,wfb2dh_FACT_HAPPEN_DT%>" SortExpression="FACT_HAPPEN_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%--請假勤務日-起 --%>
                    <asp:BoundField DataField="APPLY_LEAVE_SDT" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_SDT%>" SortExpression="APPLY_LEAVE_EDT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%--開始時間 --%>
                    <asp:BoundField DataField="APPLY_LEAVE_STIME" HeaderText="<%$Resources:Resource,wfb2dh_APPLY_LEAVE_STIME%>" SortExpression="APPLY_LEAVE_STIME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <%--請假勤務日-迄 --%>
                    <asp:BoundField DataField="APPLY_LEAVE_EDT" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_EDT%>" SortExpression="APPLY_LEAVE_EDT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%--結束時間 --%>
                    <asp:BoundField DataField="APPLY_LEAVE_ETIME" HeaderText="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_ETIME%>" SortExpression="APPLY_LEAVE_ETIME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <%--請假合計 --%>
                    <asp:BoundField DataField="TOTAL_TIME_APPROVE" HeaderText="<%$Resources:Resource,wfb2dh_lb_TOTAL_TIME_APPROVE%>" SortExpression="TOTAL_TIME_APPROVE" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <%--申請單號 --%>
                    <asp:BoundField DataField="IFLOW_NO" HeaderText="<%$Resources:Resource,wfb2dh_lb_IFLOW_NO%>" SortExpression="IFLOW_NO" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left" />
                    <%--核淮年月 --%>
                    <asp:BoundField DataField="IFLOW_APPROVE_DT" HeaderText="<%$Resources:Resource,wfb2dc_lb_IFLOW_APPROVE_DT%>" SortExpression="IFLOW_APPROVE_DT" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center" />
                    <%--計薪狀態 --%>
                    <asp:BoundField DataField="SALARY_SETTLE_STATUS" HeaderText="<%$Resources:Resource,wfb2dh_lb_SALARY_SETTLE_STATUS%>" SortExpression="SALARY_SETTLE_STATUS" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <%--發薪日期 --%>
                    <asp:BoundField DataField="PAY_DT" HeaderText="<%$Resources:Resource,wfb2dh_lb_PAY_DT%>" SortExpression="PAY_DT" HeaderStyle-Width="80px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center" />
                    
                    <%--部門 --%>
                    <asp:BoundField DataField="DEPT_NO" HeaderText="<%$Resources:Resource,wfb2dh_lb_DEPT%>" SortExpression="DEPT_NO" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>


            <!-- footer -->
            <td>


                <%--                <table width="980" border="0" cellspacing="0" cellpadding="0">
                    <tr class="PageFooter">
                        <td align="center" height="15" bgcolor="#FF0000">&copy  2007-TOYOTA MOTOR ASIA PACIFIC - ENGINEERING & MANUFACTURING CO., LTD. All Rights Reserved 
                        </td>
                    </tr>
                </table>--%>

            </td>
            </tr>
     <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DH0500ExportXLS"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
