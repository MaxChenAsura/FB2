<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA1200_Detail.aspx.cs" Inherits="WebContent_WFB2IA1200_Detail" Culture="auto" UICulture="auto" %>

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
            $(".number").mask('9999999');
            $(".number2").mask('999');
            $(".number3").mask('99.9');
            $(".numberr").css("text-align", "right");
            $(".amt").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });

            $("#txt_NEW_COMPANY_SNAME").attr("readonly", true);

            $("#txt_NEW_RC_TYPE").attr("readonly", true);
            $("#txt_NEW_HOLD_YEAR").attr("readonly", true);

            $("#txt_SUB_DESC").attr("readonly", true);
            $("#txt_NEW_FAMILY_NAME").attr("readonly", true);
            $("#txt_NEW_SUB_DESC").attr("readonly", true);

            $("#txt_NEW_FAMILY_NATION_CD").attr("readonly", true);
            $("#txt_NEW_FAMILY_BIRTH_DT").attr("readonly", true);
            $("#txt_CHG_TYPE_IN_NAME").attr("readonly", true);
            $("#txt_NEW_CHG_TYPE_IN_NAME").attr("readonly", true);
            $("#txt_CHG_REASON_CD_NAME").attr("readonly", true);
            $("#txt_NEW_CHG_REASON_CD_NAME").attr("readonly", true);
            $("#txt_NEW_EMP_NAME").attr("readonly", true);
            $("#txt_NEW_APPELLATION").attr("readonly", true);
            $("#txt_NEW_REDUCE_DESC").attr("readonly", true);

            gridviewScroll();
            $.unblockUI();
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });

            $('#<%=gv_result2.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });

            $('#<%=gv_result3.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });

            var isAdd = $("#HID_isAdd").val();
            if (isAdd == "1") {
                $('#<%=gv_result1.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F"
                });

                $('#<%=gv_result4.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F"
                });

                $('#<%=gv_result5.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F"
                });

            } else {
                $('#<%=gv_result1.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F",
                    freezesize: 4
                });
                CheckBoxCheckAllByfreeze("cb_1all_freezeheader", "cb_1check");

                $('#<%=gv_result4.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F",
                    freezesize: 5
                });
                CheckBoxCheckAllByfreeze("cb_4all_freezeheader", "cb_4check");

                $('#<%=gv_result5.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F"
                    //,freezesize: 4
                });
                //CheckBoxCheckAllByfreeze("cb_5all_freezeheader", "cb_5check");
            }

        }

        function SelectAllCheckboxes1(spanChk) {
            elm = document.forms[0];
            for (i = 0; i <= elm.length - 1; i++) {

                if (elm[i].type == "checkbox" && elm[i].id != spanChk.id && elm[i].id.substr(elm[i].id.length - 9, 9) == 'cb_1check') {

                    if (elm.elements[i].checked != spanChk.checked)
                        $('#' + elm[i].id).prop('checked', spanChk.checked);
                }
            }
        }

        function SelectAllCheckboxes2(spanChk) {
            elm = document.forms[0];
            for (i = 0; i <= elm.length - 1; i++) {

                if (elm[i].type == "checkbox" && elm[i].id != spanChk.id && elm[i].id.substr(elm[i].id.length - 9, 9) == 'cb_check2') {

                    if (elm.elements[i].checked != spanChk.checked)
                        $('#' + elm[i].id).prop('checked', spanChk.checked);
                }
            }
        }

        function SelectAllCheckboxes3(spanChk) {
            elm = document.forms[0];
            for (i = 0; i <= elm.length - 1; i++) {

                if (elm[i].type == "checkbox" && elm[i].id != spanChk.id && elm[i].id.substr(elm[i].id.length - 9, 9) == 'cb_check3') {

                    if (elm.elements[i].checked != spanChk.checked)
                        $('#' + elm[i].id).prop('checked', spanChk.checked);
                }
            }
        }

        function SelectAllCheckboxes4(spanChk) {
            elm = document.forms[0];
            for (i = 0; i <= elm.length - 1; i++) {

                if (elm[i].type == "checkbox" && elm[i].id != spanChk.id && elm[i].id.substr(elm[i].id.length - 9, 9) == 'cb_4check') {

                    if (elm.elements[i].checked != spanChk.checked)
                        $('#' + elm[i].id).prop('checked', spanChk.checked);
                }
            }
        }

        function SelectAllCheckboxes5(spanChk) {
            elm = document.forms[0];
            for (i = 0; i <= elm.length - 1; i++) {

                if (elm[i].type == "checkbox" && elm[i].id != spanChk.id && elm[i].id.substr(elm[i].id.length - 9, 9) == 'cb_5check') {

                    if (elm.elements[i].checked != spanChk.checked)
                        $('#' + elm[i].id).prop('checked', spanChk.checked);
                }
            }
        }

        function IsDelete() {
            var answer = confirm("確定要刪除?");
            if (answer)
                return true;
            else {
                document.getElementById('HID_cancel').click();
                return false;
            }
        }

        function getEmpFamily() {
            var emp_id = $("#txt_EMP_ID").val();
            var license_id = $("#txt_NEW_LICENSE_ID").val();
            OpenSearch('EmpFamily_Search.aspx', 'txt_NEW_LICENSE_ID', 'txt_NEW_FAMILY_NAME', 'EMP_ID=' + emp_id + '&FAMILY_LICENSE_ID=' + license_id, 'Y');
            //if (json != undefined) {
            //    $("#txt_NEW_LICENSE_ID").val(json.CD);    //身分證號
            //    $("#txt_NEW_FAMILY_NAME").val(json.DESC); //眷屬姓名
            //    $("#txt_NEW_SUB_DESC").val(json.Val1);    //稱謂
            //    $("#txt_NEW_FAMILY_NATION_CD").val(json.Val2); //國籍名稱

            //    $("#HID_EMP_ID").val(emp_id); //工號
            //    $("#HID_LICENSE_ID").val(json.CD);    //身分證號
            //    document.getElementById('HID_sql').click(); //眷屬出生日期
            //}
        }
        function EmpFamily_Search(id, name, value) {
            var emp_id = $("#txt_EMP_ID").val();
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#txt_NEW_LICENSE_ID").val(obj.CD);    //身分證號
                $("#txt_NEW_FAMILY_NAME").val(obj.DESC); //眷屬姓名
                $("#txt_NEW_SUB_DESC").val(obj.Val1);    //稱謂
                $("#txt_NEW_FAMILY_NATION_CD").val(obj.Val2); //國籍名稱

                $("#HID_EMP_ID").val(emp_id); //工號
                $("#HID_LICENSE_ID").val(obj.CD);    //身分證號
                document.getElementById('HID_sql').click(); //眷屬出生日期
            }
        }
        function getEmpAndFamily() {
            var identity_kind = "";
            if ($("#ddl_NEW_IDENTITY_KIND").val() != "-1") {
                identity_kind = $("#ddl_NEW_IDENTITY_KIND").val();
            }
            var emp_id = $("#txt_EMP_ID").val();
            var license_id = $("#txt_NEW_LICENSE_ID").val();
            OpenSearch('EmpAndFamily_Search.aspx', 'txt_NEW_LICENSE_ID', 'txt_NEW_EMP_NAME', 'IDENTITY_KIND=' + identity_kind + '&EMP_ID=' + emp_id + '&LICENSE_ID=' + license_id, 'Y');
            //if (json != undefined) {
            //    $("#txt_NEW_LICENSE_ID").val(json.CD); //身份證/居留證
            //    $("#txt_NEW_EMP_NAME").val(json.DESC); //姓名
            //    $("#txt_NEW_APPELLATION").val(json.Val2); //稱謂

            //    document.getElementById('hid_getEmpName5').click();
            //}
        }
        function EmpAndFamily_Search(id, name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#txt_NEW_LICENSE_ID").val(obj.CD); //身份證/居留證
                $("#txt_NEW_EMP_NAME").val(obj.DESC); //姓名
                $("#txt_NEW_APPELLATION").val(obj.Val2); //稱謂

                document.getElementById('hid_getEmpName5').click();
            }
        }
        function getCommCode(type) {
            var code_val1 = "";
            if (type == "add") {
                if ($("#ddl_NEW_CHG_TYPE_OUT").val() != "-1") {
                    code_val1 = $("#ddl_NEW_CHG_TYPE_OUT").val();
                }
                OpenSearch('CommCode_Search.aspx', 'txt_NEW_CHG_REASON_CD', 'txt_NEW_SUB_DESC', 'SYS_CD=IA&MAIN_CD=HEA_LEAVE&CODE_VAL1=' + code_val1, '');
            }
            else {
                if ($("#ddl_CHG_TYPE_OUT").val() != "-1") {
                    code_val1 = $("#ddl_CHG_TYPE_OUT").val();
                }
                OpenSearch('CommCode_Search.aspx', 'txt_CHG_REASON_CD', 'txt_SUB_DESC', 'SYS_CD=IA&MAIN_CD=HEA_LEAVE&CODE_VAL1=' + code_val1, '');
            }
        }

        function getCommCode2(type) {
            var code_val1 = "";
            if (type == "add") {
                if ($("#ddl_NEW_CHG_TYPE_OUT").val() != "-1") {
                    code_val1 = $("#ddl_NEW_CHG_TYPE_OUT").val();
                }
                OpenSearch('CommCode_Search.aspx', 'txt_NEW_CHG_REASON_CD', 'txt_NEW_CHG_REASON_CD_NAME', 'SYS_CD=IA&MAIN_CD=HEA_LEAVE&CODE_VAL1=' + code_val1, '');
            }
            else {
                if ($("#ddl_CHG_TYPE_OUT").val() != "-1") {
                    code_val1 = $("#ddl_CHG_TYPE_OUT").val();
                }
                OpenSearch('CommCode_Search.aspx', 'txt_CHG_REASON_CD', 'txt_CHG_REASON_CD_NAME', 'SYS_CD=IA&MAIN_CD=HEA_LEAVE&CODE_VAL1=' + code_val1, '');
            }
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
            if (LookUpCheckboxs() > 0) {
                var answer = confirm("確定要刪除?");
                if (answer)
                    return true;
                else {
                    var ItemCheckBoxs = $(":checkbox[id$=cb_all]");
                    for (var i = 0; i < ItemCheckBoxs.length; i++) {
                        ItemCheckBoxs[i].checked = false;
                    }
                    document.getElementById('HID_cancel').click();
                    return false;
                }
            }
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function LookUpCheckboxs2() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check2]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function CheckDelAction2() {
            if (LookUpCheckboxs2() > 0) {
                var answer = confirm("確定要刪除?");
                if (answer)
                    return true;
                else {
                    var ItemCheckBoxs = $(":checkbox[id$=cb_all]");
                    for (var i = 0; i < ItemCheckBoxs.length; i++) {
                        ItemCheckBoxs[i].checked = false;
                    }
                    document.getElementById('HID_cancel').click();
                    return false;
                }
            }
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function LookUpCheckboxs3() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check3]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function CheckDelAction3() {
            if (LookUpCheckboxs3() > 0) {
                var answer = confirm("確定要刪除?");
                if (answer)
                    return true;
                else {
                    var ItemCheckBoxs = $(":checkbox[id$=cb_all]");
                    for (var i = 0; i < ItemCheckBoxs.length; i++) {
                        ItemCheckBoxs[i].checked = false;
                    }
                    document.getElementById('HID_cancel').click();
                    return false;
                }
            }
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function LookUpCheckboxs1() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_1check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function CheckDelAction1() {
            if (LookUpCheckboxs1() > 0) {
                var answer = confirm("確定要刪除?");
                if (answer)
                    return true;
                else {
                    var ItemCheckBoxs = $(":checkbox[id$=cb_1all]");
                    for (var i = 0; i < ItemCheckBoxs.length; i++) {
                        ItemCheckBoxs[i].checked = false;
                    }
                    document.getElementById('HID_cancel').click();
                    return false;
                }
            }
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function LookUpCheckboxs4() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_4check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function CheckDelAction4() {
            if (LookUpCheckboxs4() > 0) {
                var answer = confirm("確定要刪除?");
                if (answer)
                    return true;
                else {
                    var ItemCheckBoxs = $(":checkbox[id$=cb_4all]");
                    for (var i = 0; i < ItemCheckBoxs.length; i++) {
                        ItemCheckBoxs[i].checked = false;
                    }
                    document.getElementById('HID_cancel').click();
                    return false;
                }
            }
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function LookUpCheckboxs5() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_5check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function CheckDelAction5() {
            if (LookUpCheckboxs5() > 0) {
                var answer = confirm("確定要刪除?");
                if (answer)
                    return true;
                else {
                    var ItemCheckBoxs = $(":checkbox[id$=cb_5all]");
                    for (var i = 0; i < ItemCheckBoxs.length; i++) {
                        ItemCheckBoxs[i].checked = false;
                    }
                    document.getElementById('HID_cancel').click();
                    return false;
                }
            }
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function openQry() {
            window.location.href("WFB2IA1200_Qry.aspx");
            return false;
        }

        function ShowRecord(obj) {
            if (obj == "grid")
                $("#HID_PageRow").val($("#ddlPerPageRow").val());
            else if (obj == "grid2")
                $("#HID_PageRow2").val($("#ddlPerPageRow2").val());
            else if (obj == "grid3")
                $("#HID_PageRow3").val($("#ddlPerPageRow3").val());
            else if (obj == "grid1")
                $("#HID_PageRow1").val($("#ddlPerPageRow1").val());
            else if (obj == "grid4")
                $("#HID_PageRow4").val($("#ddlPerPageRow4").val());
            else if (obj == "grid5")
                $("#HID_PageRow5").val($("#ddlPerPageRow5").val());
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
                                <col width="10%" />
                                <col width="25%" />
                                <col width="13%" />
                                <col width="22%" />
                                <col width="13%" />
                                <col width="17%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SUB_DESC" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_NATION_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SUB_DESC2" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BIRTH_DT" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_BIRTH_DT%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_BIRTH_DT" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LICENSE_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID2%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LICENSE_ID" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_JOIN_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_JOIN_DT" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DIV_DEPT_FULL_NAME" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_DIV_DEPT_FULL_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_DIV_DEPT_FULL_NAME" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static" Width="300px"></asp:TextBox>
                                    </td>

                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_DT" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LEAVE_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LEAVE_DT" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th></th>
                                    <td></td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <%--<input id="btn_back" type="button" value="<%$Resources:Resource,wfb2ia_btn_back%>" onclick="openQry();" runat="server" />--%>
                                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_back%>" OnClick="btn_back_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td class="Body_label">
                        <hr size="3">
                        【<asp:Label ID="lb_LABOR" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LABOR%>" align="left"></asp:Label>】
                        <div id="init_grid" align="right">
                            <aces:Btn ID="WFB2IA1200Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1200Add%>" OnClick="WFB2IA1200Add_Click" />
                            <aces:Btn ID="WFB2IA1200Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1200Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IA1200Delete_Click" />
                            <aces:Btn ID="WFB2IA1200Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1200Edit%>" OnClick="WFB2IA1200Edit_Click" />
                            <aces:Btn ID="WFB2IA1200Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1200Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2IA1200Save_Click" />

                            <%-- <asp:Button ID="WFB2IA1200Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1200Add%>" OnClick="WFB2IA1200Add_Click" />
                            <asp:Button ID="WFB2IA1200Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1200Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IA1200Delete_Click" />
                            <asp:Button ID="WFB2IA1200Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1200Edit%>" OnClick="WFB2IA1200Edit_Click" />
                            <asp:Button ID="WFB2IA1200Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1200Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2IA1200Save_Click" />--%>

                            <asp:Button ID="WFB2IA1200Cancel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1200Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2IA1200Cancel_Click" />
                        </div>
                        <asp:ObjectDataSource ID="ods" runat="server" SelectMethod="getLABOR_Data"
                            SelectCountMethod="getLABOR_Count" TypeName="CFB2IA1200DAO" EnablePaging="True"
                            SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                            StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                            <SelectParameters>
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:ControlParameter ControlID="hid_emp_id_key"
                                    Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                            OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1018px" OnDataBound="gv_result_DataBound"
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
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_RowNumber%>" HeaderStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="text-align: center; width: 100%">
                                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                        </div>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_APP_TYPE%>" SortExpression="CHG_APP_TYPE" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_CHG_APP_TYPE" runat="server" Text='<%#Bind("CHG_APP_TYPE")%>' Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_CHG_APP_TYPE" runat="server" Text='<%#Bind("CHG_APP_TYPE")%>' Width="80px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddl_NEW_CHG_APP_TYPE" runat="server" ClientIDMode="Static" Width="80px" BackColor="#FFD7D7">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_APP_TYPE%>"
                                            ControlToValidate="ddl_NEW_CHG_APP_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_COMPANY_CD%>" SortExpression="COMPANY_CD" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text='<%#Bind("COMPANY_CD")%>' Width="70px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text='<%#Bind("COMPANY_CD")%>' Width="70px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: left; width: 100%">
                                            <asp:TextBox ID="txt_NEW_COMPANY_CD" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="25px" MaxLength="3" OnTextChanged="txt_NEW_COMPANY_CD_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <input id="btn_COMPANY_CD" type="button" value="..." onclick="OpenSearch('Company_Search.aspx', 'txt_NEW_COMPANY_CD', 'txt_NEW_COMPANY_SNAME', '', '');" />
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_COMPANY_CD%>"
                                            ControlToValidate="txt_NEW_COMPANY_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_COMPANY_SNAME2%>" SortExpression="COMPANY_SNAME" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>' Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>' Width="80px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_COMPANY_SNAME" runat="server" ClientIDMode="Static" BorderWidth="0" Width="80px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_SALARY_AMT%>" SortExpression="SALARY_AMT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_SALARY_AMT" runat="server" Text='<%#Bind("SALARY_AMT","{0:n0}")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_SALARY_AMT" runat="server" Text='<%#Bind("SALARY_AMT")%>' BackColor="#FFD7D7" Width="100px" CssClass="number numberr"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SALARY_AMT%>"
                                            ControlToValidate="txt_SALARY_AMT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorSALARY_AMT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SALARY_AMT_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_AMT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_SALARY_AMT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" CssClass="number numberr"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SALARY_AMT%>"
                                            ControlToValidate="txt_NEW_SALARY_AMT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorSALARY_AMT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SALARY_AMT_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_NEW_SALARY_AMT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_AMT2%>" SortExpression="INS_AMT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_INS_AMT" runat="server" Text='<%#Bind("INS_AMT","{0:n0}")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="text-align: left; width: 100%">
                                            <asp:TextBox ID="txt_INS_AMT" runat="server" Text='<%#Bind("INS_AMT")%>' BackColor="#FFD7D7" Width="60px" CssClass="number numberr"></asp:TextBox>
                                            <input id="btn_INS_AMT" type="button" value="..." onclick="OpenSearch('Level_Search.aspx', 'txt_INS_TYPE', 'txt_INS_AMT', 'INS_TYPE=A', '');" />
                                            <asp:TextBox ID="txt_INS_TYPE" runat="server" ClientIDMode="Static" Visible="false"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT2%>"
                                            ControlToValidate="txt_INS_AMT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorINS_AMT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT2_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_INS_AMT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: left; width: 100%">
                                            <asp:TextBox ID="txt_NEW_INS_AMT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="60px" CssClass="number numberr"></asp:TextBox>
                                            <input id="btn_INS_AMT" type="button" value="..." onclick="OpenSearch('Level_Search.aspx', 'txt_INS_TYPE', 'txt_NEW_INS_AMT', 'INS_TYPE=A', '');" />
                                        </div>
                                        <asp:TextBox ID="txt_INS_TYPE" runat="server" ClientIDMode="Static" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT2%>"
                                            ControlToValidate="txt_NEW_INS_AMT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorINS_AMT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT2_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_NEW_INS_AMT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EFFECT_SDT%>" SortExpression="EFFECT_SDT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_EFFECT_SDT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" CssClass="date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDT%>"
                                            ControlToValidate="txt_NEW_EFFECT_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                                        <asp:CustomValidator ID="CustomValidatorEFFECT_SDT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_NEW_EFFECT_SDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EFFECT_EDT%>" SortExpression="EFFECT_EDT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT", "{0:yyyy/MM/dd}")%>' ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorEFFECT_EDT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_EFFECT_EDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_EFFECT_EDT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorEFFECT_EDT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_NEW_EFFECT_EDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_NEW_EFFECT_SDT"
                                            ControlToValidate="txt_NEW_EFFECT_EDT" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="100%" MaxLength="90"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" Width="100%" MaxLength="90"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table class="grid-view" width="1020px">
                                    <tr class="header">
                                        <td>
                                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_RowNumber%>" Width="40px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CHG_APP_TYPE%>" Width="80px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_COMPANY_CD%>" Width="70px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_COMPANY_SNAME2%>" Width="80px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label29" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_SALARY_AMT%>" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_INS_AMT2%>" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EFFECT_SDT%>" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EFFECT_EDT%>" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_REMARK%>" Width="100px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="normal">
                                        <td>
                                            <asp:CheckBox ID="cb_check" runat="server" Width="20px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lb_NewRowNumber" runat="server" Text="1" Width="40px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_NEW_CHG_APP_TYPE" runat="server" ClientIDMode="Static" Width="80px" BackColor="#FFD7D7">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_APP_TYPE%>"
                                                ControlToValidate="ddl_NEW_CHG_APP_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_COMPANY_CD" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="25px" MaxLength="3" OnTextChanged="txt_NEW_COMPANY_CD_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <input id="btn_COMPANY_CD" type="button" value="..." onclick="OpenSearch('Company_Search.aspx', 'txt_NEW_COMPANY_CD', 'txt_NEW_COMPANY_SNAME', '', '');" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_COMPANY_CD%>"
                                                ControlToValidate="txt_NEW_COMPANY_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_COMPANY_SNAME" runat="server" ClientIDMode="Static" BorderWidth="0" Width="80px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_SALARY_AMT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" CssClass="number numberr"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SALARY_AMT%>"
                                                ControlToValidate="txt_NEW_SALARY_AMT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidatorSALARY_AMT" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SALARY_AMT_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                                ControlToValidate="txt_NEW_SALARY_AMT" ValidationGroup="GroupA" Display="None">
                                            </asp:CustomValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_INS_AMT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" CssClass="number numberr"></asp:TextBox>
                                            <input id="btn_INS_AMT" type="button" value="..." onclick="OpenSearch('Level_Search.aspx', 'txt_INS_TYPE', 'txt_NEW_INS_AMT', 'INS_TYPE=A', '');" />
                                            <asp:TextBox ID="txt_INS_TYPE" runat="server" ClientIDMode="Static" Visible="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT2%>"
                                                ControlToValidate="txt_NEW_INS_AMT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidatorINS_AMT" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT2_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                                ControlToValidate="txt_NEW_INS_AMT" ValidationGroup="GroupA" Display="None">
                                            </asp:CustomValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_EFFECT_SDT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" CssClass="date"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDT%>"
                                                ControlToValidate="txt_NEW_EFFECT_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidatorEFFECT_SDT" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_NEW_EFFECT_SDT" ValidationGroup="GroupA" Display="None">
                                            </asp:CustomValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_EFFECT_EDT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                                            <asp:CustomValidator ID="CustomValidatorEFFECT_EDT" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_NEW_EFFECT_EDT" ValidationGroup="GroupA" Display="None">
                                            </asp:CustomValidator>

                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_NEW_EFFECT_SDT"
                                                ControlToValidate="txt_NEW_EFFECT_EDT" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                            </FooterTemplate>

                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" Width="100%" MaxLength="90"></asp:TextBox>
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
                                    <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('grid')" ClientIDMode="Static" AutoPostBack="true">
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
                    </td>
                </tr>

                <tr>
                    <td class="Body_label">
                        <hr size="3">
                        【<asp:Label ID="lb_PENSION" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_PENSION%>" align="left"></asp:Label>】
                        <div id="init_grid2" align="right">
                            <aces:Btn ID="WFB2IA1202Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1202Add%>" OnClick="WFB2IA1202Add_Click" />
                            <aces:Btn ID="WFB2IA1202Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1202Delete%>" OnClientClick="return CheckDelAction2();" OnClick="WFB2IA1202Delete_Click" />
                            <aces:Btn ID="WFB2IA1202Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1202Edit%>" OnClick="WFB2IA1202Edit_Click" />
                            <aces:Btn ID="WFB2IA1202Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1202Save%>" Visible="false" ValidationGroup="GroupB" OnClick="WFB2IA1202Save_Click" />

                            <%--  <asp:Button ID="WFB2IA1202Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1202Add%>" OnClick="WFB2IA1202Add_Click" />
                            <asp:Button ID="WFB2IA1202Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1202Delete%>" OnClientClick="return CheckDelAction2();" OnClick="WFB2IA1202Delete_Click" />
                            <asp:Button ID="WFB2IA1202Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1202Edit%>" OnClick="WFB2IA1202Edit_Click" />
                            <asp:Button ID="WFB2IA1202Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1202Save%>" Visible="false" ValidationGroup="GroupB" OnClick="WFB2IA1202Save_Click" />--%>

                            <asp:Button ID="WFB2IA1202Cancel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1202Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2IA1202Cancel_Click" />
                        </div>
                        <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="getPENSION_Data"
                            SelectCountMethod="getPENSION_Count" TypeName="CFB2IA1200DAO" EnablePaging="True"
                            SortParameterName="sortExpression2" OnSelecting="obs1_Selecting2"
                            StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected2">
                            <SelectParameters>
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:ControlParameter ControlID="hid_emp_id_key"
                                    Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result2_Sorting"
                            OnRowDataBound="gv_result2_RowDataBound" OnRowCreated="gv_result2_RowCreated" Width="1018px"
                            OnDataBound="gv_result2_DataBound" OnPageIndexChanging="gv_result2_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes2(this);" ClientIDMode="Static" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cb_check2" runat="server" ClientIDMode="AutoID" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_RowNumber%>" HeaderStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="text-align: center; width: 100%">
                                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                        </div>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_APP_TYPE%>" SortExpression="CHG_APP_TYPE" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_CHG_APP_TYPE" runat="server" Text='<%#Bind("CHG_APP_TYPE")%>' Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_CHG_APP_TYPE" runat="server" Text='<%#Bind("CHG_APP_TYPE")%>' Width="80px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddl_NEW_CHG_APP_TYPE" runat="server" ClientIDMode="Static" Width="80px" BackColor="#FFD7D7">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1_2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_APP_TYPE%>"
                                            ControlToValidate="ddl_NEW_CHG_APP_TYPE" ForeColor="Red" ValidationGroup="GroupB" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_COMPANY_CD%>" SortExpression="COMPANY_CD" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text='<%#Bind("COMPANY_CD")%>' Width="75px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text='<%#Bind("COMPANY_CD")%>' Width="75px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: left; width: 100%">
                                            <asp:TextBox ID="txt_NEW_COMPANY_CD" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="20px" MaxLength="3" OnTextChanged="txt_NEW_COMPANY_CD2_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <input id="btn_COMPANY_CD" type="button" value="..." onclick="OpenSearch('Company_Search.aspx', 'txt_NEW_COMPANY_CD', 'txt_NEW_COMPANY_SNAME', '', '');" />
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2_2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_COMPANY_CD%>"
                                            ControlToValidate="txt_NEW_COMPANY_CD" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_COMPANY_SNAME2%>" SortExpression="COMPANY_SNAME" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>' Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>' Width="80px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_COMPANY_SNAME" runat="server" ClientIDMode="Static" BorderWidth="0" Width="80px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_RC_TYPE%>" SortExpression="RC_TYPE" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_RC_TYPE" runat="server" Text='<%#Bind("RC_TYPE")%>' Width="40px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_RC_TYPE" runat="server" Text='<%#Bind("RC_TYPE")%>' Width="40px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_RC_TYPE" runat="server" Text="新制" ClientIDMode="Static" BorderWidth="0" Width="40px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_SALARY_AMT%>" SortExpression="SALARY_AMT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_SALARY_AMT" runat="server" Text='<%#Bind("SALARY_AMT","{0:n0}")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_SALARY_AMT" runat="server" Text='<%#Bind("SALARY_AMT")%>' BackColor="#FFD7D7" Width="100px" CssClass="number numberr"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3_2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SALARY_AMT%>"
                                            ControlToValidate="txt_SALARY_AMT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorSALARY_AMT_2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SALARY_AMT_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_AMT" ValidationGroup="GroupB" Display="None">
                                        </asp:CustomValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_SALARY_AMT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" CssClass="number numberr"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3_2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SALARY_AMT%>"
                                            ControlToValidate="txt_NEW_SALARY_AMT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorSALARY_AMT_2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SALARY_AMT_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_NEW_SALARY_AMT" ValidationGroup="GroupB" Display="None">
                                        </asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_AMT3%>" SortExpression="INS_AMT" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_INS_AMT" runat="server" Text='<%#Bind("INS_AMT","{0:n0}")%>' Width="110px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="text-align: left; width: 100%">
                                            <asp:TextBox ID="txt_INS_AMT" runat="server" Text='<%#Bind("INS_AMT")%>' BackColor="#FFD7D7" Width="55px" CssClass="number numberr"></asp:TextBox>
                                            <input id="btn_INS_AMT" type="button" value="..." onclick="OpenSearch('Level_Search.aspx', 'txt_INS_TYPE', 'txt_INS_AMT', 'INS_TYPE=C', '');" />
                                        </div>
                                        <asp:TextBox ID="txt_INS_TYPE" runat="server" ClientIDMode="Static" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4_2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT3%>"
                                            ControlToValidate="txt_INS_AMT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorINS_AMT_2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT3_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_INS_AMT" ValidationGroup="GroupB" Display="None">
                                        </asp:CustomValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: left; width: 100%">
                                            <asp:TextBox ID="txt_NEW_INS_AMT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="55px" CssClass="number numberr"></asp:TextBox>
                                            <input id="btn_INS_AMT" type="button" value="..." onclick="OpenSearch('Level_Search.aspx', 'txt_INS_TYPE', 'txt_NEW_INS_AMT', 'INS_TYPE=C', '');" />
                                        </div>
                                        <asp:TextBox ID="txt_INS_TYPE" runat="server" ClientIDMode="Static" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4_2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT3%>"
                                            ControlToValidate="txt_NEW_INS_AMT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorINS_AMT_2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT3_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_NEW_INS_AMT" ValidationGroup="GroupB" Display="None">
                                        </asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_HOLD_YEAR%>" SortExpression="HOLD_YEAR" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_HOLD_YEAR" runat="server" Text='<%#Bind("HOLD_YEAR")%>' Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_HOLD_YEAR" runat="server" Text='<%#Bind("HOLD_YEAR")%>' Width="80px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_HOLD_YEAR" runat="server" Text="0" ClientIDMode="Static" Width="80px" BorderWidth="0" CssClass="number2 numberr"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EFFECT_SDT%>" SortExpression="EFFECT_SDT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_EFFECT_SDT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" CssClass="date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5_2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDT%>"
                                            ControlToValidate="txt_NEW_EFFECT_SDT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>

                                        <asp:CustomValidator ID="CustomValidatorEFFECT_SDT_2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_NEW_EFFECT_SDT" ValidationGroup="GroupB" Display="None">
                                        </asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EFFECT_EDT%>" SortExpression="EFFECT_EDT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT", "{0:yyyy/MM/dd}")%>' ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>

                                        <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_EFFECT_EDT" ValidationGroup="GroupB" Display="None">
                                        </asp:CustomValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_EFFECT_EDT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>

                                        <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_NEW_EFFECT_EDT" ValidationGroup="GroupB" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator1_2" runat="server" ControlToCompare="txt_NEW_EFFECT_SDT"
                                            ControlToValidate="txt_NEW_EFFECT_EDT" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupB"></asp:CompareValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="100%" MaxLength="90"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" Width="100%" MaxLength="90"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table class="grid-view" width="1020px">
                                    <tr class="header">
                                        <td>
                                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes2(this);" Width="20px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_RowNumber%>" Width="40px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CHG_APP_TYPE%>" Width="80px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_COMPANY_CD%>" Width="70px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_COMPANY_SNAME2%>" Width="80px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_RC_TYPE%>" Width="40px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_SALARY_AMT%>" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_INS_AMT3%>" Width="110px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_HOLD_YEAR%>" Width="80px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EFFECT_SDT%>" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label16" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EFFECT_EDT%>" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_REMARK%>" Width="80px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="normal">
                                        <td>
                                            <asp:CheckBox ID="cb_check2" runat="server" Width="20px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lb_NewRowNumber" runat="server" Text="1" Width="40px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_NEW_CHG_APP_TYPE" runat="server" ClientIDMode="Static" Width="80px" BackColor="#FFD7D7">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1_2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_APP_TYPE%>"
                                                ControlToValidate="ddl_NEW_CHG_APP_TYPE" ForeColor="Red" ValidationGroup="GroupB" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_COMPANY_CD" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="20px" MaxLength="3" OnTextChanged="txt_NEW_COMPANY_CD2_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <input id="btn_COMPANY_CD" type="button" value="..." onclick="OpenSearch('Company_Search.aspx', 'txt_NEW_COMPANY_CD', 'txt_NEW_COMPANY_SNAME', '', '');" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2_2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_COMPANY_CD%>"
                                                ControlToValidate="txt_NEW_COMPANY_CD" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_COMPANY_SNAME" runat="server" ClientIDMode="Static" BorderWidth="0" Width="80px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_RC_TYPE" runat="server" Text="新制" ClientIDMode="Static" BorderWidth="0" Width="40px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_SALARY_AMT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" CssClass="number numberr"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3_2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SALARY_AMT%>"
                                                ControlToValidate="txt_NEW_SALARY_AMT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidatorSALARY_AMT_2" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SALARY_AMT_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                                ControlToValidate="txt_NEW_SALARY_AMT" ValidationGroup="GroupB" Display="None">
                                            </asp:CustomValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_INS_AMT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="55px" CssClass="number numberr"></asp:TextBox>
                                            <input id="btn_INS_AMT" type="button" value="..." onclick="OpenSearch('Level_Search.aspx', 'txt_INS_TYPE', 'txt_NEW_INS_AMT', 'INS_TYPE=C', '');" />
                                            <asp:TextBox ID="txt_INS_TYPE" runat="server" ClientIDMode="Static" Visible="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4_2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT2%>"
                                                ControlToValidate="txt_NEW_INS_AMT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidatorINS_AMT_2" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT3_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                                ControlToValidate="txt_NEW_INS_AMT" ValidationGroup="GroupB" Display="None">
                                            </asp:CustomValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_HOLD_YEAR" runat="server" Text="0" ClientIDMode="Static" Width="80px" BorderWidth="0" CssClass="number2 numberr"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_EFFECT_SDT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" CssClass="date"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5_2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDT%>"
                                                ControlToValidate="txt_NEW_EFFECT_SDT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidatorEFFECT_SDT_2" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_NEW_EFFECT_SDT" ValidationGroup="GroupB" Display="None">
                                            </asp:CustomValidator>

                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_EFFECT_EDT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                                            <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_2" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_NEW_EFFECT_EDT" ValidationGroup="GroupB" Display="None">
                                            </asp:CustomValidator>

                                            <asp:CompareValidator ID="CompareValidator1_2" runat="server" ControlToCompare="txt_NEW_EFFECT_SDT"
                                                ControlToValidate="txt_NEW_EFFECT_EDT" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                                Display="None" ValidationGroup="GroupB"></asp:CompareValidator>
                                            </FooterTemplate>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" Width="100%" MaxLength="90"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle CssClass="GridviewScrollPager" />
                        </asp:GridView>
                        <table id="OnePage2" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                            <tr height="100%" valign="top">
                                <td class="GridviewScrollPager TD">
                                    <asp:DropDownList ID="ddlPerPageRow2" runat="server" onchange="javascript:ShowRecord('grid2')" ClientIDMode="Static" AutoPostBack="true">
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px"></td>
                                <td style="font-size: 14px;">
                                    <asp:Label ID="lb_TotalCount2" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td class="Body_label">
                        <hr size="3">
                        【<asp:Label ID="lb_PENSION_SELF_RATIO" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_PENSION_SELF_RATIO%>" align="left"></asp:Label>】
                        <div id="init_grid3" align="right">
                            <aces:Btn ID="WFB2IA1203Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1203Add%>" OnClick="WFB2IA1203Add_Click" />
                            <aces:Btn ID="WFB2IA1203Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1203Delete%>" OnClientClick="return CheckDelAction3();" OnClick="WFB2IA1203Delete_Click" />
                            <aces:Btn ID="WFB2IA1203Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1203Edit%>" OnClick="WFB2IA1203Edit_Click" />
                            <aces:Btn ID="WFB2IA1203Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1203Save%>" Visible="false" ValidationGroup="GroupC" OnClick="WFB2IA1203Save_Click" />

                            <%-- <asp:Button ID="WFB2IA1203Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1203Add%>" OnClick="WFB2IA1203Add_Click" />
                            <asp:Button ID="WFB2IA1203Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1203Delete%>" OnClientClick="return CheckDelAction3();" OnClick="WFB2IA1203Delete_Click" />
                            <asp:Button ID="WFB2IA1203Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1203Edit%>" OnClick="WFB2IA1203Edit_Click" />
                            <asp:Button ID="WFB2IA1203Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1203Save%>" Visible="false" ValidationGroup="GroupC" OnClick="WFB2IA1203Save_Click" />--%>

                            <asp:Button ID="WFB2IA1203Cancel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1203Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2IA1203Cancel_Click" />
                        </div>
                        <asp:ObjectDataSource ID="ods3" runat="server" SelectMethod="getPENSION_SELF_RATIO_Data"
                            SelectCountMethod="getPENSION_SELF_RATIO_Count" TypeName="CFB2IA1200DAO" EnablePaging="True"
                            SortParameterName="sortExpression3" OnSelecting="obs1_Selecting3"
                            StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected3">
                            <SelectParameters>
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:ControlParameter ControlID="hid_emp_id_key"
                                    Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:GridView ID="gv_result3" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result3_Sorting"
                            OnRowDataBound="gv_result3_RowDataBound" Width="1018px"
                            OnDataBound="gv_result3_DataBound" OnPageIndexChanging="gv_result3_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes3(this);" ClientIDMode="Static" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cb_check3" runat="server" ClientIDMode="AutoID" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_RowNumber%>" HeaderStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="text-align: center; width: 100%">
                                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                        </div>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_SLEF_RATE%>" SortExpression="SLEF_RATE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_SLEF_RATE" runat="server" Text='<%#Bind("SLEF_RATE")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_SLEF_RATE" runat="server" Text='<%#Bind("SLEF_RATE")%>' Width="100px" BackColor="#FFD7D7" CssClass="number3 numberr"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1_3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SLEF_RATE%>"
                                            ControlToValidate="txt_SLEF_RATE" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorSLEF_RATE_3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_SLEF_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_SLEF_RATE" ValidationGroup="GroupC" Display="None">
                                        </asp:CustomValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_SLEF_RATE" runat="server" ClientIDMode="Static" Width="100px" BackColor="#FFD7D7" CssClass="number3 numberr"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1_3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SLEF_RATE%>"
                                            ControlToValidate="txt_NEW_SLEF_RATE" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorSLEF_RATE_3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_SLEF_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_NEW_SLEF_RATE" ValidationGroup="GroupC" Display="None">
                                        </asp:CustomValidator>

                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EFFECT_SDT%>" SortExpression="EFFECT_SDT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: left; width: 100%">
                                            <asp:TextBox ID="txt_NEW_EFFECT_SDT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" CssClass="date"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2_3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDT%>"
                                            ControlToValidate="txt_NEW_EFFECT_SDT" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorEFFECT_SDT_3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_NEW_EFFECT_SDT" ValidationGroup="GroupC" Display="None">
                                        </asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EFFECT_EDT%>" SortExpression="EFFECT_EDT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT", "{0:yyyy/MM/dd}")%>' Width="100px" CssClass="date"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_EFFECT_EDT" ValidationGroup="GroupC" Display="None">
                                        </asp:CustomValidator>

                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: left; width: 100%">
                                            <asp:TextBox ID="txt_NEW_EFFECT_EDT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                                        </div>
                                        <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_NEW_EFFECT_EDT" ValidationGroup="GroupC" Display="None">
                                        </asp:CustomValidator>

                                        <asp:CompareValidator ID="CompareValidator1_3" runat="server" ControlToCompare="txt_NEW_EFFECT_SDT"
                                            ControlToValidate="txt_NEW_EFFECT_EDT" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupC"></asp:CompareValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="100%" MaxLength="90"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: left; width: 100%">
                                            <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" Width="100%" MaxLength="90"></asp:TextBox>
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table class="grid-view" width="1020px">
                                    <tr class="header">
                                        <td>
                                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes3(this);" Width="20px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_RowNumber%>" Width="40px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_SLEF_RATE%>" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EFFECT_SDT%>" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EFFECT_EDT%>" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_REMARK%>" Width="80px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="normal">
                                        <td>
                                            <asp:CheckBox ID="cb_check3" runat="server" Width="20px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lb_NewRowNumber" runat="server" Text="1" Width="40px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_SLEF_RATE" runat="server" ClientIDMode="Static" Width="100px" BackColor="#FFD7D7" CssClass="number3 numberr"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1_3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SLEF_RATE%>"
                                                ControlToValidate="txt_NEW_SLEF_RATE" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidatorSLEF_RATE_3" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_SLEF_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                                ControlToValidate="txt_NEW_SLEF_RATE" ValidationGroup="GroupC" Display="None">
                                            </asp:CustomValidator>

                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_EFFECT_SDT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" CssClass="date"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2_3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDT%>"
                                                ControlToValidate="txt_NEW_EFFECT_SDT" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidatorEFFECT_SDT_3" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_NEW_EFFECT_SDT" ValidationGroup="GroupC" Display="None">
                                            </asp:CustomValidator>

                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_EFFECT_EDT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                                            <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_3" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_NEW_EFFECT_EDT" ValidationGroup="GroupC" Display="None">
                                            </asp:CustomValidator>

                                            <asp:CompareValidator ID="CompareValidator1_3" runat="server" ControlToCompare="txt_NEW_EFFECT_SDT"
                                                ControlToValidate="txt_NEW_EFFECT_EDT" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                                Display="None" ValidationGroup="GroupC"></asp:CompareValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" Width="100%" MaxLength="90"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle CssClass="GridviewScrollPager" />
                        </asp:GridView>
                        <table id="OnePage3" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                            <tr height="100%" valign="top">
                                <td class="GridviewScrollPager TD">
                                    <asp:DropDownList ID="ddlPerPageRow3" runat="server" onchange="javascript:ShowRecord('grid3')" ClientIDMode="Static" AutoPostBack="true">
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px"></td>
                                <td style="font-size: 14px;">
                                    <asp:Label ID="lb_TotalCount3" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td class="Body_label">
                        <hr size="3">
                        【<asp:Label ID="lb_HEALTH" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_HEALTH%>" align="left"></asp:Label>】
                        <div id="init_grid1" align="right">
                            <aces:Btn ID="WFB2IA1201Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1201Add%>" OnClick="WFB2IA1201Add_Click" />
                            <aces:Btn ID="WFB2IA1201Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1201Delete%>" OnClientClick="return CheckDelAction1();" OnClick="WFB2IA1201Delete_Click" />
                            <aces:Btn ID="WFB2IA1201Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1201Edit%>" OnClick="WFB2IA1201Edit_Click" />
                            <aces:Btn ID="WFB2IA1201Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1201Save%>" Visible="false" ValidationGroup="GroupD" OnClick="WFB2IA1201Save_Click" />

                            <%-- <asp:Button ID="WFB2IA1201Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1201Add%>" OnClick="WFB2IA1201Add_Click" />
                            <asp:Button ID="WFB2IA1201Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1201Delete%>" OnClientClick="return CheckDelAction1();" OnClick="WFB2IA1201Delete_Click" />
                            <asp:Button ID="WFB2IA1201Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1201Edit%>" OnClick="WFB2IA1201Edit_Click" />
                            <asp:Button ID="WFB2IA1201Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1201Save%>" Visible="false" ValidationGroup="GroupD" OnClick="WFB2IA1201Save_Click" />--%>

                            <asp:Button ID="WFB2IA1201Cancel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1201Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2IA1201Cancel_Click" />
                        </div>
                        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getHEALTH_Data"
                            SelectCountMethod="getHEALTH_Count" TypeName="CFB2IA1200DAO" EnablePaging="True"
                            SortParameterName="sortExpression1" OnSelecting="obs1_Selecting1"
                            StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected1">
                            <SelectParameters>
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:ControlParameter ControlID="hid_emp_id_key"
                                    Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:GridView ID="gv_result1" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result1_Sorting"
                            OnRowDataBound="gv_result1_RowDataBound" OnRowCreated="gv_result1_RowCreated" Width="1020px"
                            OnDataBound="gv_result1_DataBound" OnPageIndexChanging="gv_result1_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="cb_1all" runat="server" onclick="javascript:SelectAllCheckboxes1(this);" ClientIDMode="Static" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cb_1check" runat="server" ClientIDMode="AutoID" />
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_RowNumber%>" HeaderStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>                             
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="text-align: center; width: 40px">
                                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                            <asp:HiddenField ID="hid_LICENSE_ID_H" runat="server" Value='<%#Bind("LICENSE_ID")%>' />
                                            <asp:Label ID="lb_LICENSE_ID_H" runat="server" Text='<%#Bind("LICENSE_ID")%>' Width="40px" Visible="false"></asp:Label>
                                        </div>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_APP_TYPE%>" SortExpression="CHG_APP_TYPE" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_CHG_APP_TYPE" runat="server" Text='<%#Bind("CHG_APP_TYPE")%>' Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_CHG_APP_TYPE" runat="server" Text='<%#Bind("CHG_APP_TYPE")%>' Width="80px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddl_NEW_CHG_APP_TYPE" runat="server" ClientIDMode="Static" Width="80px" BackColor="#FFD7D7">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1_1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_APP_TYPE%>"
                                            ControlToValidate="ddl_NEW_CHG_APP_TYPE" ForeColor="Red" ValidationGroup="GroupD" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_COMPANY_CD%>" SortExpression="COMPANY_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text='<%#Bind("COMPANY_CD")%>' Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text='<%#Bind("COMPANY_CD")%>' Width="80px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: left; width: 80px">
                                            <asp:TextBox ID="txt_NEW_COMPANY_CD" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="20px" MaxLength="3" OnTextChanged="txt_NEW_COMPANY_CD1_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <input id="btn_COMPANY_CD" type="button" value="..." onclick="OpenSearch('Company_Search.aspx', 'txt_NEW_COMPANY_CD', 'txt_NEW_COMPANY_SNAME', '', '');" />
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2_1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_COMPANY_CD%>"
                                            ControlToValidate="txt_NEW_COMPANY_CD" ForeColor="Red" ValidationGroup="GroupD" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_COMPANY_SNAME2%>" SortExpression="COMPANY_SNAME" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>' Width="100px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_COMPANY_SNAME" runat="server" ClientIDMode="Static" BorderWidth="0" Width="80px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_AMT2%>" SortExpression="INS_AMT" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_INS_AMT" runat="server" Text='<%#Bind("INS_AMT","{0:n0}")%>' Width="110px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                            <asp:TextBox ID="txt_INS_AMT" runat="server" Text='<%#Bind("INS_AMT")%>' Width="55px" BackColor="#FFD7D7" CssClass="number numberr"></asp:TextBox>
                                            <input id="btn_INS_AMT" type="button" value="..." onclick="OpenSearch('Level_Search.aspx', 'txt_INS_TYPE', 'txt_INS_AMT', 'INS_TYPE=B', '');" />
                                        
                                        <asp:TextBox ID="txt_INS_TYPE" runat="server" ClientIDMode="Static" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3_1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT2%>"
                                            ControlToValidate="txt_INS_AMT" ForeColor="Red" ValidationGroup="GroupD" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorINS_AMT_1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT2_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_INS_AMT" ValidationGroup="GroupD" Display="None">
                                        </asp:CustomValidator>

                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: left; width: 100px">
                                            <asp:TextBox ID="txt_NEW_INS_AMT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="55px" CssClass="number numberr"></asp:TextBox>
                                            <input id="btn_INS_AMT" type="button" value="..." onclick="OpenSearch('Level_Search.aspx', 'txt_INS_TYPE', 'txt_NEW_INS_AMT', 'INS_TYPE=B', '');" />
                                        </div>
                                        <asp:TextBox ID="txt_INS_TYPE" runat="server" ClientIDMode="Static" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3_1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT2%>"
                                            ControlToValidate="txt_NEW_INS_AMT" ForeColor="Red" ValidationGroup="GroupD" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorINS_AMT_1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT2_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_NEW_INS_AMT" ValidationGroup="GroupD" Display="None">
                                        </asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EFFECT_SDT%>" SortExpression="EFFECT_SDT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_EFFECT_SDT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator100" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDT%>"
                                            ControlToValidate="txt_NEW_EFFECT_SDT" ForeColor="Red" ValidationGroup="GroupD" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorEFFECT_SDT_1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_NEW_EFFECT_SDT" ValidationGroup="GroupD" Display="None">
                                        </asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EFFECT_EDT%>" SortExpression="EFFECT_EDT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT", "{0:yyyy/MM/dd}")%>' Width="100px" CssClass="date"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_EFFECT_EDT" ValidationGroup="GroupD" Display="None">
                                        </asp:CustomValidator>

                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_EFFECT_EDT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_NEW_EFFECT_EDT" ValidationGroup="GroupD" Display="None">
                                        </asp:CustomValidator>

                                        <asp:CompareValidator ID="CompareValidator1_1" runat="server" ControlToCompare="txt_NEW_EFFECT_SDT"
                                            ControlToValidate="txt_NEW_EFFECT_EDT" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupD"></asp:CompareValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_TYPE_OUT%>" SortExpression="CHG_TYPE_OUT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_CHG_TYPE_OUT" runat="server" Text='<%#Bind("CHG_TYPE_OUT")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddl_CHG_TYPE_OUT" runat="server" ClientIDMode="Static" Width="100px" OnSelectedIndexChanged="ddl_CHG_TYPE_OUT_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <asp:HiddenField ID="hid_CHG_TYPE_OUT" runat="server" Value='<%#Bind("CHG_TYPE_OUT")%>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddl_NEW_CHG_TYPE_OUT" runat="server" ClientIDMode="Static" Width="100px" OnSelectedIndexChanged="ddl_NEW_CHG_TYPE_OUT_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_REASON_CD%>" SortExpression="CHG_REASON_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_CHG_REASON_CD" runat="server" Text='<%#Bind("CHG_REASON_CD")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_CHG_REASON_CD" runat="server" ClientIDMode="Static" Text='<%#Bind("CHG_REASON_CD")%>' Width="40px" OnTextChanged="txt_CHG_REASON_CD_TextChanged" AutoPostBack="true" MaxLength="1"></asp:TextBox>
                                        <input id="btn_CHG_REASON_CD" type="button" value="..." onclick="getCommCode('');" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_CHG_REASON_CD" runat="server" ClientIDMode="Static" Width="40px" OnTextChanged="txt_NEW_CHG_REASON_CD_TextChanged" AutoPostBack="true" MaxLength="1"></asp:TextBox>
                                        <input id="btn_CHG_REASON_CD" type="button" value="..." onclick="getCommCode('add');" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_REASON_NAME%>" SortExpression="SUB_DESC" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_SUB_DESC" runat="server" Text='<%#Bind("SUB_DESC")%>' Width="180px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_SUB_DESC" runat="server" ClientIDMode="Static" Text='<%#Bind("SUB_DESC")%>' Width="170px" BorderWidth="0"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_SUB_DESC" runat="server" ClientIDMode="Static" Width="180px" BorderWidth="0"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="100px" ></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="100px" MaxLength="90"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" Width="100px" MaxLength="90"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div style="width: 1020px; overflow: auto;">
                                    <table class="grid-view" width="1020px">
                                        <tr class="header">
                                            <td>
                                                <asp:CheckBox ID="cb_1all" runat="server" onclick="javascript:SelectAllCheckboxes1(this);" Width="20px" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_RowNumber%>" Width="40px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CHG_APP_TYPE%>" Width="80px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_COMPANY_CD%>" Width="70px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_COMPANY_SNAME2%>" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_INS_AMT2%>" Width="110px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EFFECT_SDT%>" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EFFECT_EDT%>" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label18" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CHG_TYPE_OUT%>" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label19" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CHG_REASON_CD%>" Width="140px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label20" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CHG_REASON_NAME%>" Width="180px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_REMARK%>" Width="100px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="normal">
                                            <td>
                                                <asp:CheckBox ID="cb_1check" runat="server" Width="20px" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lb_NewRowNumber" runat="server" Text="1" Width="40px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_NEW_CHG_APP_TYPE" runat="server" ClientIDMode="Static" Width="80px" BackColor="#FFD7D7">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1_1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_APP_TYPE%>"
                                                    ControlToValidate="ddl_NEW_CHG_APP_TYPE" ForeColor="Red" ValidationGroup="GroupD" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_COMPANY_CD" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="20px" MaxLength="3" OnTextChanged="txt_NEW_COMPANY_CD1_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <input id="btn_COMPANY_CD" type="button" value="..." onclick="OpenSearch('Company_Search.aspx', 'txt_NEW_COMPANY_CD', 'txt_NEW_COMPANY_SNAME', '', '');" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2_1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_COMPANY_CD%>"
                                                    ControlToValidate="txt_NEW_COMPANY_CD" ForeColor="Red" ValidationGroup="GroupD" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_COMPANY_SNAME" runat="server" ClientIDMode="Static" BorderWidth="0" Width="80px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_INS_AMT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="55px" CssClass="number numberr"></asp:TextBox>
                                                <input id="btn_INS_AMT" type="button" value="..." onclick="OpenSearch('Level_Search.aspx', 'txt_INS_TYPE', 'txt_NEW_INS_AMT', 'INS_TYPE=B', '');" />
                                                <asp:TextBox ID="txt_INS_TYPE" runat="server" ClientIDMode="Static" Visible="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3_1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT2%>"
                                                    ControlToValidate="txt_NEW_INS_AMT" ForeColor="Red" ValidationGroup="GroupD" Display="None"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="CustomValidatorINS_AMT_1" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_INS_AMT2_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                                    ControlToValidate="txt_NEW_INS_AMT" ValidationGroup="GroupD" Display="None">
                                                </asp:CustomValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_EFFECT_SDT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                                                <asp:CustomValidator ID="CustomValidatorEFFECT_SDT_1" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                    ControlToValidate="txt_NEW_EFFECT_SDT" ValidationGroup="GroupD" Display="None">
                                                </asp:CustomValidator>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_EFFECT_EDT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                                                <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_1" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                    ControlToValidate="txt_NEW_EFFECT_EDT" ValidationGroup="GroupD" Display="None">
                                                </asp:CustomValidator>

                                                <asp:CompareValidator ID="CompareValidator1_1" runat="server" ControlToCompare="txt_NEW_EFFECT_SDT"
                                                    ControlToValidate="txt_NEW_EFFECT_EDT" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                                    Display="None" ValidationGroup="GroupD"></asp:CompareValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_NEW_CHG_TYPE_OUT" runat="server" ClientIDMode="Static" Width="100px" OnSelectedIndexChanged="ddl_NEW_CHG_TYPE_OUT_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_CHG_REASON_CD" runat="server" ClientIDMode="Static" Width="70px" OnTextChanged="txt_NEW_CHG_REASON_CD_TextChanged" AutoPostBack="true" MaxLength="1"></asp:TextBox>
                                                <input id="btn_CHG_REASON_CD" type="button" value="..." onclick="getCommCode('add');" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_SUB_DESC" runat="server" ClientIDMode="Static" Width="180px" BorderWidth="0"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" Width="100%" MaxLength="90"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </EmptyDataTemplate>
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle CssClass="GridviewScrollPager" />
                        </asp:GridView>
                        <table id="OnePage1" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                            <tr height="100%" valign="top">
                                <td class="GridviewScrollPager TD">
                                    <asp:DropDownList ID="ddlPerPageRow1" runat="server" onchange="javascript:ShowRecord('grid1')" ClientIDMode="Static" AutoPostBack="true">
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px"></td>
                                <td style="font-size: 14px;">
                                    <asp:Label ID="lb_TotalCount1" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td class="Body_label">
                        <hr size="3">
                        【<asp:Label ID="lb_HEALTH_Dependents" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_HEALTH_DEPENDENTS%>" align="left"></asp:Label>】
                        <div id="init_grid4" align="right">
                            <aces:Btn ID="WFB2IA1204Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1204Add%>" OnClick="WFB2IA1204Add_Click" />
                            <aces:Btn ID="WFB2IA1204Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1204Delete%>" OnClientClick="return CheckDelAction4();" OnClick="WFB2IA1204Delete_Click" />
                            <aces:Btn ID="WFB2IA1204Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1204Edit%>" OnClick="WFB2IA1204Edit_Click" />
                            <aces:Btn ID="WFB2IA1204TRACEBACK" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1204TRACEBACK%>" OnClick="WFB2IA1204TRACEBACK_Click" />
                            <aces:Btn ID="WFB2IA1204Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1204Save%>" Visible="false" ValidationGroup="GroupE" OnClick="WFB2IA1204Save_Click" />

                            <%--<asp:Button ID="WFB2IA1204Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1204Add%>" OnClick="WFB2IA1204Add_Click" />
                            <asp:Button ID="WFB2IA1204Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1204Delete%>" OnClientClick="return CheckDelAction4();" OnClick="WFB2IA1204Delete_Click" />
                            <asp:Button ID="WFB2IA1204Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1204Edit%>" OnClick="WFB2IA1204Edit_Click" />
                            <asp:Button ID="WFB2IA1204TRACEBACK" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1204TRACEBACK%>" OnClick="WFB2IA1204TRACEBACK_Click" />
                            <asp:Button ID="WFB2IA1204Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1204Save%>" Visible="false" ValidationGroup="GroupE" OnClick="WFB2IA1204Save_Click" />--%>

                            <asp:Button ID="WFB2IA1204Cancel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1204Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2IA1204Cancel_Click" />
                        </div>
                        <asp:ObjectDataSource ID="ods4" runat="server" SelectMethod="getHEALTH_FAMILY_Data"
                            SelectCountMethod="getHEALTH_FAMILY_Count" TypeName="CFB2IA1200DAO" EnablePaging="True"
                            SortParameterName="sortExpression4" OnSelecting="obs1_Selecting4"
                            StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected4">
                            <SelectParameters>
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:ControlParameter ControlID="hid_emp_id_key"
                                    Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:GridView ID="gv_result4" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result4_Sorting"
                            OnRowDataBound="gv_result4_RowDataBound" OnRowCreated="gv_result4_RowCreated" Width="1018px"
                            OnDataBound="gv_result4_DataBound" OnPageIndexChanging="gv_result4_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="cb_4all" runat="server" onclick="javascript:SelectAllCheckboxes4(this);" ClientIDMode="Static" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cb_4check" runat="server" ClientIDMode="AutoID" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_RowNumber%>" HeaderStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="text-align: center; width: 100%">
                                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                        </div>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID2%>" SortExpression="LICENSE_ID" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>' Width="100px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div style="width: 140px;">
                                            <asp:TextBox ID="txt_NEW_LICENSE_ID" runat="server" ClientIDMode="Static" Width="80px" BackColor="#FFD7D7" MaxLength="20" OnTextChanged="txt_NEW_LICENSE_ID_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <input id="btn_LICENSE_ID" type="button" value="..." onclick="getEmpFamily();" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_LICENCE_ID%>"
                                                ControlToValidate="txt_NEW_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_FAMILY_NAME%>" SortExpression="FAMILY_NAME" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_FAMILY_NAME" runat="server" Text='<%#Bind("FAMILY_NAME")%>' Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_FAMILY_NAME" runat="server" Text='<%#Bind("FAMILY_NAME")%>' Width="80px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_FAMILY_NAME" runat="server" ClientIDMode="Static" Width="80px" BackColor="#FFD7D7" BorderWidth="0"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_FAMILY_NAME%>"
                                            ControlToValidate="txt_NEW_FAMILY_NAME" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_APPELLATION%>" SortExpression="SUB_DESC" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_SUB_DESC" runat="server" Text='<%#Bind("SUB_DESC")%>' Width="40px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_SUB_DESC" runat="server" Text='<%#Bind("SUB_DESC")%>' Width="40px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_SUB_DESC" runat="server" ClientIDMode="Static" Width="40px" BackColor="#FFD7D7" BorderWidth="0"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_APPELLATION%>"
                                            ControlToValidate="txt_NEW_SUB_DESC" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_NATION_NAME%>" SortExpression="FAMILY_NATION_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_FAMILY_NATION_CD" runat="server" Text='<%#Bind("FAMILY_NATION_CD")%>' Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_FAMILY_NATION_CD" runat="server" Text='<%#Bind("FAMILY_NATION_CD")%>' Width="80px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_FAMILY_NATION_CD" runat="server" ClientIDMode="Static" Width="80px" BackColor="#FFD7D7" BorderWidth="0"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_NATION_NAME%>"
                                            ControlToValidate="txt_NEW_FAMILY_NATION_CD" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_BIRTH_DT%>" SortExpression="FAMILY_BIRTH_DT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_FAMILY_BIRTH_DT" runat="server" Text='<%#Bind("FAMILY_BIRTH_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_FAMILY_BIRTH_DT" runat="server" Text='<%#Bind("FAMILY_BIRTH_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_FAMILY_BIRTH_DT" runat="server" ClientIDMode="Static" Width="100px" BackColor="#FFD7D7" BorderWidth="0"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_BIRTH_DT%>"
                                            ControlToValidate="txt_NEW_FAMILY_BIRTH_DT" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EFFECT_SDT2%>" SortExpression="EFFECT_SDT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_EFFECT_SDT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" CssClass="date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDT2%>"
                                            ControlToValidate="txt_NEW_EFFECT_SDT" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorEFFECT_SDT_4" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDATE2%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_NEW_EFFECT_SDT" ValidationGroup="GroupE" Display="None">
                                        </asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_TYPE_IN%>" SortExpression="CHG_TYPE_IN" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_CHG_TYPE_IN" runat="server" Text='<%#Bind("CHG_TYPE_IN")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_CHG_TYPE_IN" runat="server" Text='<%#Bind("CHG_TYPE_IN")%>' BackColor="#FFD7D7" Width="40px" MaxLength="3" OnTextChanged="txt_CHG_TYPE_IN_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <input id="btn_CHG_TYPE_IN" type="button" value="..." onclick="OpenSearch('CommCode_Search.aspx', 'txt_CHG_TYPE_IN', 'txt_CHG_TYPE_IN_NAME', 'SYS_CD=IA&MAIN_CD=HEA_ADD', '');" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_TYPE_IN%>"
                                            ControlToValidate="txt_CHG_TYPE_IN" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_CHG_TYPE_IN" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="40px" MaxLength="3" OnTextChanged="txt_NEW_CHG_TYPE_IN_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <input id="btn_CHG_TYPE_IN" type="button" value="..." onclick="OpenSearch('CommCode_Search.aspx', 'txt_NEW_CHG_TYPE_IN', 'txt_NEW_CHG_TYPE_IN_NAME', 'SYS_CD=IA&MAIN_CD=HEA_ADD', '');" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_TYPE_IN%>"
                                            ControlToValidate="txt_NEW_CHG_TYPE_IN" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_TYPE_IN_NAME%>" SortExpression="CHG_TYPE_IN_NAME" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_CHG_TYPE_IN_NAME" runat="server" Text='<%#Bind("CHG_TYPE_IN_NAME")%>' Width="180px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_CHG_TYPE_IN_NAME" runat="server" Text='<%#Bind("CHG_TYPE_IN_NAME")%>' BackColor="#FFD7D7" Width="180px" BorderWidth="0"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_TYPE_IN_NAME%>"
                                            ControlToValidate="txt_CHG_TYPE_IN_NAME" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_CHG_TYPE_IN_NAME" runat="server" ClientIDMode="Static" Width="180px" BackColor="#FFD7D7" BorderWidth="0"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_TYPE_IN_NAME%>"
                                            ControlToValidate="txt_NEW_CHG_TYPE_IN_NAME" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EFFECT_EDT2%>" SortExpression="EFFECT_EDT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT", "{0:yyyy/MM/dd}")%>' Width="100px" CssClass="date"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_4" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_EFFECT_EDT" ValidationGroup="GroupE" Display="None">
                                        </asp:CustomValidator>

                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_EFFECT_EDT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_4" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_NEW_EFFECT_EDT" ValidationGroup="GroupE" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator1_4" runat="server" ControlToCompare="txt_NEW_EFFECT_SDT"
                                            ControlToValidate="txt_NEW_EFFECT_EDT" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT_RANGE2%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupE"></asp:CompareValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_TYPE_OUT%>" SortExpression="CHG_TYPE_OUT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_CHG_TYPE_OUT" runat="server" Text='<%#Bind("CHG_TYPE_OUT")%>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddl_CHG_TYPE_OUT" runat="server" ClientIDMode="Static" Width="100px" OnSelectedIndexChanged="ddl_CHG_TYPE_OUT_SelectedIndexChanged4" AutoPostBack="true"></asp:DropDownList>
                                        <asp:HiddenField ID="hid_CHG_TYPE_OUT" runat="server" Value='<%#Bind("CHG_TYPE_OUT")%>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddl_NEW_CHG_TYPE_OUT" runat="server" ClientIDMode="Static" Width="100px" OnSelectedIndexChanged="ddl_NEW_CHG_TYPE_OUT_SelectedIndexChanged4" AutoPostBack="true"></asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_REASON_CD%>" SortExpression="CHG_REASON_CD" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_CHG_REASON_CD" runat="server" Text='<%#Bind("CHG_REASON_CD")%>' Width="140px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_CHG_REASON_CD" runat="server" Text='<%#Bind("CHG_REASON_CD")%>' Width="70px" OnTextChanged="txt_CHG_REASON_CD_TextChanged4" AutoPostBack="true" MaxLength="1"></asp:TextBox>
                                        <input id="btn_CHG_REASON_CD" type="button" value="..." onclick="getCommCode2('');" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_CHG_REASON_CD" runat="server" ClientIDMode="Static" Width="70px" OnTextChanged="txt_NEW_CHG_REASON_CD_TextChanged4" AutoPostBack="true" MaxLength="1"></asp:TextBox>
                                        <input id="btn_CHG_REASON_CD" type="button" value="..." onclick="getCommCode2('add');" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_REASON_NAME%>" SortExpression="CHG_REASON_CD_NAME" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_CHG_REASON_CD_NAME" runat="server" Text='<%#Bind("CHG_REASON_CD_NAME")%>' Width="180px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_CHG_REASON_CD_NAME" runat="server" Text='<%#Bind("CHG_REASON_CD_NAME")%>' Width="180px" BorderWidth="0"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_CHG_REASON_CD_NAME" runat="server" ClientIDMode="Static" Width="180px" BorderWidth="0"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="80px" MaxLength="90"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" Width="80px" MaxLength="90"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div style="width: 1020px; overflow: auto;">
                                    <table class="grid-view" width="1020px">
                                        <tr class="header">
                                            <td>
                                                <asp:CheckBox ID="cb_4all" runat="server" onclick="javascript:SelectAllCheckboxes4(this);" Width="20px" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_RowNumber%>" Width="40px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_FAMILY_NAME%>" Width="80px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_APPELLATION%>" Width="40px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_NATION_NAME%>" Width="80px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_BIRTH_DT%>" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EFFECT_SDT2%>" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CHG_TYPE_IN%>" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label21" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CHG_TYPE_IN_NAME%>" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label22" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EFFECT_EDT2%>" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label23" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CHG_TYPE_OUT%>" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label24" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CHG_REASON_CD%>" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label25" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CHG_REASON_NAME%>" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label26" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_REMARK%>" Width="80px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="normal">
                                            <td>
                                                <asp:CheckBox ID="cb_4check" runat="server" Width="20px" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lb_NewRowNumber" runat="server" Text="1" Width="40px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_LICENSE_ID" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" MaxLength="20" OnTextChanged="txt_NEW_LICENSE_ID_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <input id="btn_LICENSE_ID" type="button" value="..." onclick="getEmpFamily();" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_LICENCE_ID%>"
                                                    ControlToValidate="txt_NEW_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_FAMILY_NAME" runat="server" ClientIDMode="Static" Width="80px" BackColor="#FFD7D7" BorderWidth="0"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_FAMILY_NAME%>"
                                                    ControlToValidate="txt_NEW_FAMILY_NAME" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_SUB_DESC" runat="server" ClientIDMode="Static" Width="40px" BackColor="#FFD7D7" BorderWidth="0"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_APPELLATION%>"
                                                    ControlToValidate="txt_NEW_SUB_DESC" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_FAMILY_NATION_CD" runat="server" ClientIDMode="Static" Width="80px" BackColor="#FFD7D7" BorderWidth="0"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_NATION_NAME%>"
                                                    ControlToValidate="txt_NEW_FAMILY_NATION_CD" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_FAMILY_BIRTH_DT" runat="server" ClientIDMode="Static" Width="100px" BackColor="#FFD7D7" BorderWidth="0"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_BIRTH_DT%>"
                                                    ControlToValidate="txt_NEW_FAMILY_BIRTH_DT" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_EFFECT_SDT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" CssClass="date"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDT2%>"
                                                    ControlToValidate="txt_NEW_EFFECT_SDT" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="CustomValidatorEFFECT_SDT_4" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDATE2%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                    ControlToValidate="txt_NEW_EFFECT_SDT" ValidationGroup="GroupE" Display="None">
                                                </asp:CustomValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_CHG_TYPE_IN" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="50px" MaxLength="3" OnTextChanged="txt_NEW_CHG_TYPE_IN_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <input id="btn_CHG_TYPE_IN" type="button" value="..." onclick="OpenSearch('CommCode_Search.aspx', 'txt_NEW_CHG_TYPE_IN', 'txt_NEW_CHG_TYPE_IN_NAME', 'SYS_CD=IA&MAIN_CD=HEA_ADD', '');" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_TYPE_IN%>"
                                                    ControlToValidate="txt_NEW_CHG_TYPE_IN" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_CHG_TYPE_IN_NAME" runat="server" ClientIDMode="Static" Width="100px" BorderWidth="0"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8_4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_TYPE_IN_NAME%>"
                                                    ControlToValidate="txt_NEW_CHG_TYPE_IN_NAME" ForeColor="Red" ValidationGroup="GroupE" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_EFFECT_EDT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                                                <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_4" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                    ControlToValidate="txt_NEW_EFFECT_EDT" ValidationGroup="GroupE" Display="None">
                                                </asp:CustomValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_NEW_CHG_TYPE_OUT" runat="server" ClientIDMode="Static" Width="100px"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_CHG_REASON_CD" runat="server" ClientIDMode="Static" Width="50px"></asp:TextBox>
                                                <input id="btn_CHG_REASON_CD" type="button" value="..." onclick="getCommCode2('add');" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_CHG_REASON_CD_NAME" runat="server" ClientIDMode="Static" Width="100px" BorderWidth="0"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" Width="100%" MaxLength="90"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </EmptyDataTemplate>
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle CssClass="GridviewScrollPager" />
                        </asp:GridView>
                        <table id="OnePage4" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                            <tr height="100%" valign="top">
                                <td class="GridviewScrollPager TD">
                                    <asp:DropDownList ID="ddlPerPageRow4" runat="server" onchange="javascript:ShowRecord('grid4')" ClientIDMode="Static" AutoPostBack="true">
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px"></td>
                                <td style="font-size: 14px;">
                                    <asp:Label ID="lb_TotalCount4" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td class="Body_label">
                        <hr size="3">
                        【<asp:Label ID="lb_REDUCE_SET" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_REDUCE_SET%>" align="left"></asp:Label>】
                        <div id="init_grid5" align="right">
                            <aces:Btn ID="WFB2IA1205Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1205Add%>" OnClick="WFB2IA1205Add_Click" />
                            <aces:Btn ID="WFB2IA1205Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1205Delete%>" OnClientClick="return CheckDelAction5();" OnClick="WFB2IA1205Delete_Click" />
                            <aces:Btn ID="WFB2IA1205Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1205Edit%>" OnClick="WFB2IA1205Edit_Click" />
                            <aces:Btn ID="WFB2IA1205Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1205Save%>" Visible="false" ValidationGroup="GroupF" OnClick="WFB2IA1205Save_Click" />

                            <%--<asp:Button ID="WFB2IA1205Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1205Add%>" OnClick="WFB2IA1205Add_Click" />
                            <asp:Button ID="WFB2IA1205Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1205Delete%>" OnClientClick="return CheckDelAction5();" OnClick="WFB2IA1205Delete_Click" />
                            <asp:Button ID="WFB2IA1205Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1205Edit%>" OnClick="WFB2IA1205Edit_Click" />
                            <asp:Button ID="WFB2IA1205Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1205Save%>" Visible="false" ValidationGroup="GroupF" OnClick="WFB2IA1205Save_Click" />--%>

                            <asp:Button ID="WFB2IA1205Cancel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1205Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2IA1205Cancel_Click" />
                        </div>
                        <asp:ObjectDataSource ID="ods5" runat="server" SelectMethod="getREDUCE_TXN_Data"
                            SelectCountMethod="getREDUCE_TXN_Count" TypeName="CFB2IA1200DAO" EnablePaging="True"
                            SortParameterName="sortExpression5" OnSelecting="obs1_Selecting5"
                            StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected5">
                            <SelectParameters>
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:ControlParameter ControlID="hid_emp_id_key"
                                    Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:GridView ID="gv_result5" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result5_Sorting"
                            OnRowDataBound="gv_result5_RowDataBound" OnRowCreated="gv_result5_RowCreated" Width="1018px"
                            OnDataBound="gv_result5_DataBound" OnPageIndexChanging="gv_result5_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="cb_5all" runat="server" onclick="javascript:SelectAllCheckboxes5(this);" ClientIDMode="Static" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cb_5check" runat="server" ClientIDMode="AutoID" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="text-align: center; width: 100%">
                                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                        </div>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_IDENTITY_KIND%>" SortExpression="IDENTITY_KIND" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_IDENTITY_KIND" runat="server" Text='<%#Bind("IDENTITY_KIND")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_IDENTITY_KIND" runat="server" Text='<%#Bind("IDENTITY_KIND")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddl_NEW_IDENTITY_KIND" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" OnSelectedIndexChanged="ddl_NEW_IDENTITY_KIND_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1_5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_IDENTITY_KIND%>"
                                            ControlToValidate="ddl_NEW_IDENTITY_KIND" ForeColor="Red" ValidationGroup="GroupF" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID2%>" SortExpression="LICENSE_ID" HeaderStyle-Width="145px" ItemStyle-Width="145px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>' ></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_LICENSE_ID" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="80px" OnTextChanged="txt_NEW_LICENSE_ID_TextChanged1" AutoPostBack="true" MaxLength="20"></asp:TextBox>
                                        <input id="btn_LICENSE_ID" type="button" value="..." onclick="getEmpAndFamily();" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2_5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_LICENCE_ID2%>"
                                            ControlToValidate="txt_NEW_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupF" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' ></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' ></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_EMP_NAME" runat="server" ClientIDMode="Static" Width="90px" BorderWidth="0" BackColor="#FFD7D7"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3_5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EMP_NAME%>"
                                            ControlToValidate="txt_NEW_EMP_NAME" ForeColor="Red" ValidationGroup="GroupF" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_APPELLATION%>" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_APPELLATION" runat="server" Text='<%#Bind("APPELLATION")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_APPELLATION" runat="server" Text='<%#Bind("APPELLATION")%>' ></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_APPELLATION" runat="server" ClientIDMode="Static" Width="50px" BorderWidth="0" BackColor="#FFD7D7"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4_5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_APPELLATION%>"
                                            ControlToValidate="txt_NEW_APPELLATION" ForeColor="Red" ValidationGroup="GroupF" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_REDUCE_CD%>" SortExpression="REDUCE_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_REDUCE_CD" runat="server" Text='<%#Bind("REDUCE_CD")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_REDUCE_CD" runat="server" Text='<%#Bind("REDUCE_CD")%>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: left; width: 70px">
                                            <asp:TextBox ID="txt_NEW_REDUCE_CD" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="25px" OnTextChanged="txt_NEW_REDUCE_CD_TextChanged" AutoPostBack="true" MaxLength="3"></asp:TextBox>
                                            <input id="btn_REDUCE_CD" type="button" value="..." onclick="OpenSearch('Reduce_Search.aspx', 'txt_NEW_REDUCE_CD', 'txt_NEW_REDUCE_DESC', '', '');" />
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5_5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_REDUCE_CD%>"
                                            ControlToValidate="txt_NEW_REDUCE_CD" ForeColor="Red" ValidationGroup="GroupF" Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_REDUCE_LEVEL_DESC%>" SortExpression="REDUCE_DESC" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_REDUCE_DESC" runat="server" Text='<%#Bind("REDUCE_DESC")%>' Width="110px" ></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_REDUCE_DESC" runat="server" Text='<%#Bind("REDUCE_DESC")%>' Width="110px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_REDUCE_DESC" runat="server" ClientIDMode="Static"  Width="110px" BorderWidth="0"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EFFECT_SDT%>" SortExpression="EFFECT_SDT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT", "{0:yyyy/MM/dd}")%>' Width="90px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_EFFECT_SDT" runat="server" Text='<%#Bind("EFFECT_SDT", "{0:yyyy/MM/dd}")%>' Width="90px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_EFFECT_SDT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="80px" CssClass="date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6_5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDT%>"
                                            ControlToValidate="txt_NEW_EFFECT_SDT" ForeColor="Red" ValidationGroup="GroupF" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorEFFECT_SDT_5" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_NEW_EFFECT_SDT" ValidationGroup="GroupF" Display="None">
                                        </asp:CustomValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EFFECT_EDT%>" SortExpression="EFFECT_EDT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT", "{0:yyyy/MM/dd}")%>' Width="90px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT", "{0:yyyy/MM/dd}")%>' ClientIDMode="Static" CssClass="date" Width="90px"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_5" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_EFFECT_EDT" ValidationGroup="GroupF" Display="None">
                                        </asp:CustomValidator>

                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_EFFECT_EDT" runat="server" ClientIDMode="Static" Width="80px" CssClass="date"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_5" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_NEW_EFFECT_EDT" ValidationGroup="GroupF" Display="None">
                                        </asp:CustomValidator>

                                        <asp:CompareValidator ID="CompareValidator1_5" runat="server" ControlToCompare="txt_NEW_EFFECT_SDT"
                                            ControlToValidate="txt_NEW_EFFECT_EDT" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupF"></asp:CompareValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="130px" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>' ></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_REMARK" runat="server" Text='<%#Bind("REMARK")%>' Width="110px" MaxLength="90"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" Width="110px" MaxLength="90"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table class="grid-view" width="1020px">
                                    <tr class="header">
                                        <td>
                                            <asp:CheckBox ID="cb_5all" runat="server" onclick="javascript:SelectAllCheckboxes5(this);" Width="20px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_RowNumber%>" Width="40px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_IDENTITY_KIND%>" Width="60px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID2%>" Width="130px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_NAME%>" Width="60px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label28" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_APPELLATION%>" Width="60px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_REDUCE_CD%>" Width="80px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_REDUCE_LEVEL_DESC%>" Width="120px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EFFECT_SDT%>" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label27" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EFFECT_EDT%>" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_REMARK%>" Width="80px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="normal">
                                        <td>
                                            <asp:CheckBox ID="cb_5check" runat="server" Width="20px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lb_NewRowNumber" runat="server" Text="1" Width="40px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_NEW_IDENTITY_KIND" runat="server" ClientIDMode="Static" Width="60px" BackColor="#FFD7D7" OnSelectedIndexChanged="ddl_NEW_IDENTITY_KIND_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1_5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_IDENTITY_KIND%>"
                                                ControlToValidate="ddl_NEW_IDENTITY_KIND" ForeColor="Red" ValidationGroup="GroupF" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_LICENSE_ID" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="80px" OnTextChanged="txt_NEW_LICENSE_ID_TextChanged1" AutoPostBack="true" MaxLength="20"></asp:TextBox>
                                            <input id="btn_LICENSE_ID" type="button" value="..." onclick="getEmpAndFamily();" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2_5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_LICENCE_ID2%>"
                                                ControlToValidate="txt_NEW_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupF" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_EMP_NAME" runat="server" ClientIDMode="Static" Width="60px" BorderWidth="0" BackColor="#FFD7D7"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3_5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EMP_NAME%>"
                                                ControlToValidate="txt_NEW_EMP_NAME" ForeColor="Red" ValidationGroup="GroupF" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_APPELLATION" runat="server" ClientIDMode="Static" Width="60px" BorderWidth="0" BackColor="#FFD7D7"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4_5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_APPELLATION%>"
                                                ControlToValidate="txt_NEW_APPELLATION" ForeColor="Red" ValidationGroup="GroupF" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_REDUCE_CD" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="35px" OnTextChanged="txt_NEW_REDUCE_CD_TextChanged" AutoPostBack="true" MaxLength="3"></asp:TextBox>
                                            <input id="btn_REDUCE_CD" type="button" value="..." onclick="OpenSearch('Reduce_Search.aspx', 'txt_NEW_REDUCE_CD', 'txt_NEW_REDUCE_DESC', '', '');" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5_5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_REDUCE_CD%>"
                                                ControlToValidate="txt_NEW_REDUCE_CD" ForeColor="Red" ValidationGroup="GroupF" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_REDUCE_DESC" runat="server" ClientIDMode="Static" Width="120px" BorderWidth="0"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_EFFECT_SDT" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="100px" CssClass="date"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6_5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDT%>"
                                                ControlToValidate="txt_NEW_EFFECT_SDT" ForeColor="Red" ValidationGroup="GroupF" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidatorEFFECT_SDT_5" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_SDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_NEW_EFFECT_SDT" ValidationGroup="GroupF" Display="None">
                                            </asp:CustomValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_EFFECT_EDT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                                            <asp:CustomValidator ID="CustomValidatorEFFECT_EDT_5" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_EDATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_NEW_EFFECT_EDT" ValidationGroup="GroupF" Display="None">
                                            </asp:CustomValidator>
                                            <asp:CompareValidator ID="CompareValidator1_5" runat="server" ControlToCompare="txt_NEW_EFFECT_SDT"
                                                ControlToValidate="txt_NEW_EFFECT_EDT" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                                Display="None" ValidationGroup="GroupF"></asp:CompareValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" Width="100%" MaxLength="90"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle CssClass="GridviewScrollPager" />
                        </asp:GridView>
                        <table id="OnePage5" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                            <tr height="100%" valign="top">
                                <td class="GridviewScrollPager TD">
                                    <asp:DropDownList ID="ddlPerPageRow5" runat="server" onchange="javascript:ShowRecord('grid5')" ClientIDMode="Static" AutoPostBack="true">
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px"></td>
                                <td style="font-size: 14px;">
                                    <asp:Label ID="lb_TotalCount5" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr size="8">
                    </td>
                </tr>
            </table>

            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow3" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow1" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow4" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow5" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupC" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary4" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupD" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary5" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupE" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary6" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupF" ShowSummary="false" />
            <asp:Button ID="HID_cancel" runat="server" Style="display: none" ClientIDMode="Static" OnClick="HID_cancel_Click" />
            <asp:HiddenField ID="HID_isAdd" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_emp_id_key" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_LICENSE_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_FAMILY_BIRTH_DT" runat="server" ClientIDMode="Static" />
            <asp:Button ID="HID_sql" runat="server" Style="display: none" ClientIDMode="Static" OnClick="HID_sql_Click" />

            <asp:Button ID="hid_getEmpName5" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getEmpName5_Click" />

            <asp:HiddenField ID="hid_IDENTITY_KIND" runat="server" ClientIDMode="Static" Value="" />

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2IA1204TRACEBACK" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>

