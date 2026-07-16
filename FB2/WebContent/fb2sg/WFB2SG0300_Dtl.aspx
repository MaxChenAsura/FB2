<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sg/WFB2SG0300_Dtl.aspx.cs" Inherits="WebContent_WFB2SG0300_Dtl" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        $(function () {

            iniForm();
        });
        function iniForm() {
            //日期格式心須
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $(".fAMT").mask('9999999');
            $(".txtDisabled").css("background-color", "white").css("color", "black").css("border-width","0");
            //$(".envUnit").mask('9.9');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");


            //GridView必須
            gridviewScroll();
            $.unblockUI();

            //Grid依工號取得員工資料的ajax
            $("#txt_NEW_MP_ID").change(function () {
                doEmpAjax();
            });
            //$("#txt_NEW_MP_ID").blur(function () {
            //    doEmpAjax();
            // });

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
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));
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

        function doEmpAjax() {
            if ($("#txt_NEW_MP_ID").val().length == 5) {
                $.ajax({
                    url: "WFB2SG0300_GetEmpData.ashx",
                    data: {
                        EMP_ID: $('#txt_NEW_MP_ID').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "") {
                            $('#txt_NEW_EMP_NAME').val("");
                            $('#txt_NEW_EMP_CD_DESC').val("");
                            $('#hid_NEW_EMP_CD').val("");
                            $('#txt_NEW_LEVEL_CD').val("");
                            $('#txt_NEW_JOIN_DT').val("");
                            $('#txt_NEW_WORK_DAYS').val("");
                            $('#txt_NEW_EMP_CHG_CD_DESC').val("");
                            $('#hid_NEW_EMP_CHG_CD').val("");
                            $('#txt_NEW_PJOB_CD').val("");
                            alert(JData.errMsg);
                        }
                        else {
                            $('#txt_NEW_EMP_NAME').val(JData.EMP_NAME);
                            $('#txt_NEW_EMP_CD_DESC').val(JData.EMP_CD_DESC);
                            $('#hid_NEW_EMP_CD').val(JData.EMP_CD);
                            $('#txt_NEW_LEVEL_CD').val(JData.LEVEL_CD);
                            $('#txt_NEW_JOIN_DT').val(JData.JOIN_DT);
                            $('#txt_NEW_WORK_DAYS').val(JData.WORK_DAYS);
                            $('#txt_NEW_EMP_CHG_CD_DESC').val(JData.EMP_CHG_CD_DESC);
                            $('#hid_NEW_EMP_CHG_CD').val(JData.EMP_CHG_CD);
                            $('#txt_NEW_PJOB_CD').val(JData.PJOB_CD);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            } else {
                $('#txt_NEW_EMP_NAME').val("");
                $('#txt_NEW_EMP_CD_DESC').val("");
                $('#hid_NEW_EMP_CD').val("");
                $('#txt_NEW_LEVEL_CD').val("");
                $('#txt_NEW_JOIN_DT').val("");
                $('#txt_NEW_WORK_DAYS').val("");
                $('#txt_NEW_EMP_CHG_CD_DESC').val("");
                $('#hid_NEW_EMP_CHG_CD').val("");
                $('#txt_NEW_PJOB_CD').val("");
            }
        }

        //function OpenEmpSearchSG030(emp_id) {
        //    var supervisor = $('#HID_IS_SUPERVISOR').val("");
        //    var returnValue = window.showModalDialog("../comm/Dept_Search.aspx?mode=all&super=" + supervisor + "&parentFuncId=" + parentFuncID, self, 'dialogWidth=900px;dialogHeight=400px;scroll=no;addressbar:No;');
        //    if (returnValue == undefined) {
        //        returnValue = window.returnValue;
        //    }
        //    if (!(typeof returnValue === 'undefined')) {

        //        var obj = jQuery.parseJSON(returnValue);
        //        $("#" + emp_id).val(obj.EMP_ID);
        //        //$("#" + emp_name).val(obj.EMP_NAME);
        //        doEmpAjax();
        //        return obj;

        //    }
        //}
        function returnEMPValueToPage(emp_id, emp_name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#" + emp_id).val(obj.EMP_ID.trim());
                doEmpAjax();
                return obj;
            }
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }

        //凍結視窗用
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 6

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }
            else {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F"

                });
            }
        }



        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                    if (choietr == null) {
                        choietr = i;
                    }
                }
            }
            return HaveCheck;
        }

        //查詢前檢核
        function CheckSearch() {
            //其它需要檢核的

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed) {
                $.unblockUI();
                return;
            }

            return processed;
        }


        //刪除,檢查是否有勾選
        function doDelete() {
            var processed = true;
            BlockUI();
            if (checkboxsSelected() > 0) {
                processed = confirm("確定要刪除?");
            } else {
                alert("請選取資料!");
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
            }
            return processed;

        }

        //檢核 支付狀態一括更新
        function CheckUpdateAction() {
            var processed = true;
            BlockUI();
            var processed = true;
            if (checkboxsSelected() > 0) {
                processed = confirm("確定更新支付狀態？");
            } else {
                alert("請選擇資料");
                choietr = null;
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
        //下載
        function checkDown(msg) {
            var processed = true;
            BlockUI();
            processed = confirm("確定要下載核可資料??");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_EMP_CD").val("-1");
            $("#txt_LEVEL_CD").val("");
            $("#ddl_qry_PAY_TYPE").val("-1");
            $("#ddl_EMP_CHG_CD").val("-1");
            $("#txt_PJOB_CD").val("");
        }

    </script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--節金類別--%>
                            <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_type%>"></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_FESTIVAL_TYPE_DESC" runat="server" CssClass="txtDisabled"  Enabled="false"  BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--節日日期--%>
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_dt%>"></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_FESTIVAL_DT" runat="server" CssClass="txtDisabled"  Enabled="false"  BorderWidth="0"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--節金發放日期--%>
                            <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_pay_dt%>"></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_FESTIVAL_PAY_DT" runat="server" CssClass="txtDisabled"  Enabled="false"  BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_total_amt%>"></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_FESTIVAL_TOTAL_AMT" runat="server" CssClass="txtDisabled"  Enabled="false"  BorderWidth="0"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_total_num%>"></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_FESTIVAL_TOTAL_NUM" runat="server" CssClass="txtDisabled"  Enabled="false"  BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_salary_trans_dt%>"></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_SALARY_TRANS_DT" runat="server" CssClass="txtDisabled"  Enabled="false"  BorderWidth="0"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_approve_status%>"></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="txt_APPROVE_STATUS" runat="server" CssClass="txtDisabled"  Enabled="false"  BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    </tr>
                    <tr>
                        <%--備註說明--%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_remark%>"></asp:Label>
                        </th>
                        <td colspan="5">
                            <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_REMARK" runat="server" Width="100%" Enabled="false"  BorderWidth="1" Style="overflow: auto"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_Excel_Download" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_exceldown%>"></asp:Label>
                        </th>
                        <td colspan="3">
                            <aces:Btn ID="WFB2SG0301ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_maintaindown%>" OnClick="WFB2SG0301ExcelDown_Click" OnClientClick="return checkDown(this.value);" />

                            <%--<asp:Button ID="WFB2SG0301ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_maintaindown%>" OnClick="WFB2SG0301ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />--%>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                            <asp:Button ID="WFB2SG0300Back" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_back%>" OnClientClick="BlockUI();" OnClick="WFB2SG0300Back_Click" />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                                <colgroup>
                                    <col width="10%" />
                                    <col width="20%" />
                                    <col width="10%" />
                                    <col width="20%" />
                                    <col width="10%" />
                                    <col width="20%" />
                                    <col width="10%" />
                                </colgroup>
                                <tbody>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <%--工號--%>
                                            <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_id%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5"></asp:TextBox>
                                            <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                        </td>
                                        <th align="left" class="Body_TableHeader">
                                            <%--姓名--%>
                                            <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_name%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <th align="left" class="Body_TableHeader">
                                            <%--員工區分--%>
                                            <asp:Label ID="Label14" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_prid_cd%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_EMP_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <%--資格代號--%>
                                            <asp:Label ID="Label19" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_level_cd%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_LEVEL_CD" runat="server" Width="50px" ClientIDMode="Static" MaxLength="3"></asp:TextBox>
                                        </td>
                                        <th align="left" class="Body_TableHeader">
                                            <%--支付狀態--%>
                                            <asp:Label ID="Label21" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_pay_type%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_qry_PAY_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <%--在職區分--%>
                                            <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_chg_cd%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_EMP_CHG_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        </td>
                                        <th align="left" class="Body_TableHeader">
                                            <%--職務代號--%>
                                            <asp:Label ID="Label16" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_pjob_cd%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_PJOB_CD" runat="server" Width="100px" ClientIDMode="Static"   MaxLength="4"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <td align="right" colspan="6">
                                            <aces:Btn ID="WFB2SG0301Search" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_search%>" OnClick="WFB2SG0301Search_Click" OnClientClick="BlockUI();" />

                                            <%--<asp:Button ID="WFB2SG0301Search" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_search%>" OnClick="WFB2SG0301Search_Click" OnClientClick="BlockUI();" />--%>
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sg_btn_clear%>" onclick="ClearAll();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="3">
                                            <%--支付狀態--%>
                                            <div id="payTypeUpdate">
                                                <asp:Label ID="Label18" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_pay_type%>"></asp:Label>:
                                                 <asp:DropDownList ID="ddl_upd_PAY_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                <aces:Btn ID="WFB2SG0301Update" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_update%>" OnClick="WFB2SG0301Update_Click" OnClientClick="return CheckUpdateAction();" />

                                                <%--<asp:Button ID="WFB2SG0301Update" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_update%>" OnClick="WFB2SG0301Update_Click" OnClientClick="return CheckUpdateAction();" />--%>
                                            </div>

                                        </td>
                                        <td align="right" class="Body_label" colspan="4">
                                            <div id="init_grid">
                                                <aces:Btn ID="WFB2SG0301Add" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_add%>" Visible="true" OnClick="WFB2SG0301Add_Click" />
                                                <aces:Btn ID="WFB2SG0301Delete" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_delete%>" Visible="false" OnClick="WFB2SG0301Delete_Click" OnClientClick="return doDelete();" />
                                                <aces:Btn ID="WFB2SG0301Edit" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_edit%>" Visible="false" OnClick="WFB2SG0301Edit_Click" OnClientClick="BlockUI();" />
                                                <aces:Btn ID="WFB2SG0301OK" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_ok%>" Visible="false" OnClick="WFB2SG0300OK_Click" OnClientClick="return saveCheck()" />

                                                <%--<asp:Button ID="WFB2SG0301Add" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_add%>" Visible="true" OnClick="WFB2SG0301Add_Click" />
                                                <asp:Button ID="WFB2SG0301Delete" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_delete%>" Visible="false" OnClick="WFB2SG0301Delete_Click" OnClientClick="return doDelete();" />
                                                <asp:Button ID="WFB2SG0301Edit" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_edit%>" Visible="false" OnClick="WFB2SG0301Edit_Click" OnClientClick="BlockUI();" />
                                                <asp:Button ID="WFB2SG0301OK" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_ok%>" Visible="false" OnClick="WFB2SG0300OK_Click" OnClientClick="return saveCheck()" />--%>
                                                <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDataDtl"
                SelectCountMethod="getCountDtl" TypeName="CFB2SG0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="HID_FESTIVAL_TYPE"
                        Name="festival_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_FESTIVAL_DT"
                        Name="festival_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_FESTIVAL_PAY_DT"
                        Name="festival_pay_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="HID_TARGET_GEN_DT"
                        Name="target_gen_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />

                    <asp:ControlParameter ControlID="hid_qry_EMP_ID"
                        Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_EMP_NAME"
                        Name="emp_name" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_LEVEL_CD"
                        Name="level_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_PJOB_CD"
                        Name="pjob_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_EMP_CD"
                        Name="emp_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_PAY_TYPE"
                        Name="pay_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_EMP_CHG_CD"
                        Name="emp_chg_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <%-- 
					//txt
					 <asp:ControlParameter ControlID="txt_APPLY_DT_S"
                        Name="appleDT_S" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
					//下拉式                   
					<asp:ControlParameter ControlID="ddl_ENV_ALLOWANCE_TYPE"
                        Name="env_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
					//radio
                    <asp:ControlParameter ControlID="rbl_USE_STATUS" DefaultValue=""
                        Name="is_valid" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    --%>
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" Width="20px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_rownumber%>" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                           <asp:Label ID="lb_RowNumber" runat="server" Text='' Width="80px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--異常註記--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_approve_mark%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="APPROVE_MARK">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_MARK" runat="server" Text='<%#Bind("APPROVE_MARK")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_APPROVE_MARK" runat="server" Text='<%#Bind("APPROVE_MARK")%>' Width="80px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--異動狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_chg_status%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="CHG_STATUS">
                        <ItemTemplate>
                            <asp:Label ID="lb_CHG_STATUS" runat="server" Text='<%#Bind("CHG_STATUS_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_CHG_STATUS" runat="server" Text='<%#Bind("CHG_STATUS_DESC")%>' Width="80px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--核可--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_approve_flag%>" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" SortExpression="APPROVE_FLAG">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_FLAG" runat="server" Text='<%#Bind("APPROVE_FLAG")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_APPROVE_FLAG" runat="server" Text='<%#Bind("APPROVE_FLAG")%>' Width="40px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_emp_id%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_MP_ID" runat="server" MaxLength="5" Width="60px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_NEW_MP_ID', '', 'N', 'Y');" />
                            <asp:RegularExpressionValidator ID="Regular_NEW_MP_ID" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_emp_id%>" ControlToValidate="txt_NEW_MP_ID" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression=".{5,5}" Display="None"></asp:RegularExpressionValidator>

                        </FooterTemplate>

                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_emp_name%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_EMP_NAME" runat="server" Width="100px" CssClass="txtDisabled" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_EMP_NAME" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_emp_id%>"
                                ControlToValidate="txt_NEW_EMP_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--員工區分--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_emp_cd%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="EMP_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_CD" runat="server" Text='<%#Bind("EMP_CD_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_CD" runat="server" Text='<%#Bind("EMP_CD_DESC")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_EMP_CD_DESC" runat="server" Width="80px" CssClass="txtDisabled" ClientIDMode="Static"></asp:TextBox>
                            <asp:HiddenField ID="hid_NEW_EMP_CD" runat="server" ClientIDMode="Static" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--資格代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_level_cd%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center" SortExpression="LEVEL_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="80px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_LEVEL_CD" runat="server" Width="80px" CssClass="txtDisabled" ClientIDMode="Static"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--入社日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_join_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="JOIN_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_JOIN_DT" runat="server" Text='<%#Bind("JOIN_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_JOIN_DT" runat="server" Text='<%#Bind("JOIN_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_JOIN_DT" runat="server" Width="100px" CssClass="txtDisabled " ClientIDMode="Static"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--在職年資--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_work_days%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" SortExpression="WORK_DAYS">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_DAYS" runat="server" Text='<%#Bind("WORK_DAYS")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_WORK_DAYS" runat="server" Text='<%#Bind("WORK_DAYS")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_WORK_DAYS" runat="server" Width="100px" CssClass="txtDisabled" ClientIDMode="Static"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--節金金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_amt%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="FESTIVAL_AMT">
                        <ItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_AMT" runat="server" Text='<%#Bind("FESTIVAL_AMT","{0:n0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_FESTIVAL_AMT" runat="server" Text='<%#Bind("FESTIVAL_AMT")%>' ClientIDMode="Static" Width="100px" CssClass="MandatoryField  decimal fAMT"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="EDIT_FESTIVAL_AMT" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_festival_amt%>"
                                ControlToValidate="txt_EDIT_FESTIVAL_AMT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="Regular_EDIT_FESTIVAL_AMT" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_festival_amt%>" ControlToValidate="txt_EDIT_FESTIVAL_AMT" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression=".{1,9}" Display="None"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_FESTIVAL_AMT" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField  decimal fAMT"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_FESTIVAL_AMT" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_festival_amt%>"
                                ControlToValidate="txt_NEW_FESTIVAL_AMT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="Regular_NEW_FESTIVAL_AMT" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_festival_amt%>" ControlToValidate="txt_NEW_FESTIVAL_AMT" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression=".{1,9}" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--支付狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_pay_type%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center" SortExpression="PAY_TYPE">
                        <ItemTemplate>
                            <asp:Label ID="PAY_TYPE" runat="server" Text='<%#Bind("PAY_TYPE_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_EDIT_PAY_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:HiddenField ID="hid_EDIT_PAY_TYPE" runat="server" Value='<%#Bind("PAY_TYPE")%> ' ClientIDMode="Static" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_PAY_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--在職區分--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_emp_chg_cd%>" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="left" SortExpression="EMP_CHG_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_CHG_CD" runat="server" Text='<%#Bind("EMP_CHG_CD_DESC")%>' Width="160px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_CHG_CD" runat="server" Text='<%#Bind("EMP_CHG_CD_DESC")%>' Width="160px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_EMP_CHG_CD_DESC" runat="server" Width="100px" CssClass="txtDisabled" ClientIDMode="Static"></asp:TextBox>
                            <asp:HiddenField ID="hid_NEW_EMP_CHG_CD" runat="server" ClientIDMode="Static" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--職務代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_pjob_cd%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center" SortExpression="PJOB_CD">
                        <ItemTemplate>
                            <asp:Label ID="PJOB_CD" runat="server" Text='<%#Bind("PJOB_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="PJOB_CD" runat="server" Text='<%#Bind("PJOB_CD")%>' Width="80px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_PJOB_CD" runat="server" Width="100px" CssClass="txtDisabled" ClientIDMode="Static"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <%--當DB無資料時，就會使用此table --%>
                <EmptyDataTemplate>
                    <div style="width: 1020px;  overflow-x:auto;overflow-y:hidden; height:100px">
                        <table class="grid-view" width="1020px">
                            <tr class="header">
                                <td>
                                    <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_rownumber%>" Width="40px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_approve_mark%>" Width="80px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_chg_status%>" Width="80px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_approve_flag%>" Width="40px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_id%>" Width="100px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_name%>" Width="100px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label20" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_cd%>" Width="80px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label22" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_level_cd%>" Width="80px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label23" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_join_dt%>" Width="100px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label24" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_work_days%>" Width="100px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label25" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_amt%>" Width="80px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label26" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_pay_type%>" Width="80px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label27" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_chg_cd%>" Width="80px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label28" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_pjob_cd%>" Width="80px"></asp:Label>
                                </td>
                            </tr>
                            <tr class="normal">
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_MP_ID" runat="server" MaxLength="5" Width="60px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                    <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_NEW_MP_ID', '', 'N', 'Y');" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_EMP_NAME" runat="server" Width="100px" CssClass="txtDisabled" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="NEW_EMP_NAME" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_emp_id%>"
                                        ControlToValidate="txt_NEW_EMP_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_EMP_CD_DESC" runat="server" Width="80px" CssClass="txtDisabled" ClientIDMode="Static"></asp:TextBox>
                                    <asp:HiddenField ID="hid_NEW_EMP_CD" runat="server" ClientIDMode="Static" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_LEVEL_CD" runat="server" Width="80px" CssClass="txtDisabled" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_JOIN_DT" runat="server" Width="100px" CssClass="txtDisabled" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_WORK_DAYS" runat="server" Width="100px" CssClass="txtDisabled" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_FESTIVAL_AMT" runat="server" Text='<%#Bind("FESTIVAL_AMT")%>' MaxLength="7" ClientIDMode="Static" Width="100px" CssClass="MandatoryField  decimal"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="NEW_FESTIVAL_AMT" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_festival_amt%>"
                                        ControlToValidate="txt_NEW_FESTIVAL_AMT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="Regular_NEW_FESTIVAL_AMT" runat="server"
                                        ErrorMessage="<%$Resources:Resource,wfb2sg_format_festival_amt%>" ControlToValidate="txt_NEW_FESTIVAL_AMT" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression=".{1,9}" Display="None"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_NEW_PAY_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_EMP_CHG_CD_DESC" runat="server" Width="100px" CssClass="txtDisabled" ClientIDMode="Static"></asp:TextBox>
                                    <asp:HiddenField ID="hid_NEW_EMP_CHG_CD" runat="server" ClientIDMode="Static" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_NEW_PJOB_CD" runat="server" Width="100px" CssClass="txtDisabled" ClientIDMode="Static"></asp:TextBox>
                                </td>

                            </tr>
                        </table>
                    </div>
                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />

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

            <%-- SG030_Qry的查詢條件 --%>
            <asp:HiddenField ID="HID_Qry_FESTIVAL_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Qry_FESTIVAL_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Qry_FESTIVAL_PAY_DT" runat="server" ClientIDMode="Static" />

            <%-- SG030_Dtl 查詢條件 --%>
            <asp:HiddenField ID="hid_qry_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_EMP_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_LEVEL_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_PJOB_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_EMP_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_PAY_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_EMP_CHG_CD" runat="server" ClientIDMode="Static" />



            <asp:HiddenField ID="HID_FREEZE_FLAG" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_APPROVE_STATUS" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_FESTIVAL_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_TARGET_GEN_DT" runat="server" ClientIDMode="Static" />

            <%-- 是否為supervisor  --%>
            <asp:HiddenField ID="HID_IS_SUPERVISOR" runat="server" ClientIDMode="Static" />
            <%-- 每頁的筆數 --%>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <%-- 是否要凍結視窗 --%>
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <%-- 進行新增或修改的檢核 --%>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <%--EXCEL下載用 --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SG0301ExcelDown" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
