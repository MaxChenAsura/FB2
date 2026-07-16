<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/comm/Dept_Search.aspx.cs" Inherits="tw_co_toyota_kuozui_web_comm_Dept_Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <base target="_self" />
    <title></title>
    <script type="text/javascript" src="../../Scripts/Basic.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //document.onkeydown = onkeydownhandler;
            //onkeydownhandler();
            $("#txt_EMP_ID").mask("99999");
        });
        function onkeydownhandler() {
            if (navigator.userAgent.indexOf("Safari") > -1) {
                document.onkeydown = twice;
                function twice() {
                    if (event.keyCode == 8 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 9) {

                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
        }
        function ReturnValue(json) {
           
            var mode = $('#HID_mode').val();
            var flag = window.parent.$('#div_iframeID').attr("flag");            
            var eid = window.parent.$('#div_iframeID').attr("stid");
            var ename = window.parent.$('#div_iframeID').attr("stname");
            //alert(mode + "   " + flag + "   " + eid + "   " + ename);
            if (mode == 'dept') {
                if (typeof flag === 'undefined' || flag === '') {
                    returnDEPT(eid, ename, json);//回公用js
                }
                else if (flag === 'Y') {
                    parent.returnDEPTValueToPage(eid, ename, json);//返回原始呼叫頁面
                }
            } else {
                if (typeof flag === 'undefined' || flag === '') {
                    returnEMP(eid, ename, json);//回公用js
                }
                else if (flag === 'Y') {
                    parent.returnEMPValueToPage(eid, ename, json);//返回原始呼叫頁面
                }
            }
            //202111/10 add 考課開窗回傳FUN
            
            if (mode == 'all') {
                if (typeof (parent.popAssessEmpReturn) != undefined) {
                    if (typeof (parent.popAssessEmpReturn) == "function") {
                        parent.popAssessEmpReturn(json);
                    }
                }
            }
            if (mode == 'dept') {
                if (typeof (parent.popAssessReturn) != undefined) {
                    if (typeof (parent.popAssessReturn) == "function") {
                        parent.popAssessReturn(json);
                    }
                }
            }
            window.parent.$("#div_iframeID").dialog('close');           
        }

        function closeWin() {
            window.parent.$("#div_iframeID").dialog('close');
        }

        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels
                CheckUncheckParents(src, src.checked);
            }
        }

        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }

        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;

            if (parentNodeTable) {
                var checkUncheckSwitch;

                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        return; //do not need to check parent if any(one or more) child not checked
                }
                else //checkbox unchecked
                {
                    checkUncheckSwitch = false;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }
        //清空畫面
        function ClearAll() {

            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");

        }


    </script>
    <style type="text/css">
        div.clear {
            clear: both;
            width: 0;
            height: 0;
            margin: 0;
            margin-top: -5px;
            padding: 0;
            overflow: hidden;
            font-size: 0em;
        }
    </style>
</head>
<body style="background-color: aliceblue">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="float: left; width: 300px; height: 400px; overflow: auto;">
                    <asp:TreeView ID="tv_view" runat="server" LineImagesFolder="~/images/TreeLineImages"
                        ShowLines="True" OnSelectedNodeChanged="tv_view_SelectedNodeChanged" ForeColor="Blue">
                        <Nodes>
                            <asp:TreeNode Text="公司別" SelectAction="Expand" Value=""></asp:TreeNode>


                        </Nodes>
                    </asp:TreeView>
                </div>
                <div style="float: right; width: 520px; height: 400px; overflow: auto; text-align: center;" id="div_emp" runat="server">
                    <table width="500" border="0" cellspacing="0" cellpadding="0">

                        <tr>
                            <th align="left" class="Body_TableHeader">員工編號:</th>
                            <td>
                                <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" ClientIDMode="Static" Width="100px"></asp:TextBox>
                            </td>
                            <th align="left" class="Body_TableHeader">員工姓名:</th>
                            <td>
                                <asp:TextBox ID="txt_EMP_NAME" runat="server" ClientIDMode="Static" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right">
                                <asp:Button ID="btn_search" runat="server" Text="查詢" OnClick="btn_search_Click" />
                                <input id="Button1" runat="server" type="button" value="清除" onclick="ClearAll();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center" valign="top">
                                <div style="overflow: scroll; overflow-x: hidden; width: 100%; height: 300px">
                                    <asp:GridView ID="gv_result" runat="server" CssClass="grid-view" Width="450px"
                                        AutoGenerateColumns="False" OnRowDataBound="gv_result_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:RadioButton ID="rbl_emp_id" runat="server" GroupName="rblg_emp_id" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ROWID" HeaderText="序號" />
                                            <asp:BoundField DataField="EMP_ID" HeaderText="工號" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="EMP_NAME" HeaderText="姓名" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="DEPT_NO" HeaderText="直屬部門代號" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="DEPT_NAME" HeaderText="直屬部門名稱" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="EMP_NAME" HeaderText="員工名稱" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="PJOB_CD" />
                                            <asp:BoundField DataField="EMP_CD" />
                                            <asp:BoundField DataField="LEVEL_CD" />
                                            <asp:BoundField DataField="GRADE_CD" />
                                            <asp:BoundField DataField="JOIN_DT" />
                                            <asp:BoundField DataField="BE_EMP_DT" />
                                            <asp:BoundField DataField="WS_CD" />
                                            <asp:BoundField DataField="EMP_STATUS" />
                                            <asp:BoundField DataField="PLANT_NAME" />
                                            <asp:BoundField DataField="WORK_SHIFT_DESC" />
                                            <asp:BoundField DataField="EMP_STATUS_DESC" />

                                            <asp:BoundField DataField="DEPT_NO_20" />
                                            <asp:BoundField DataField="DEPT_NAME_20" />
                                            <asp:BoundField DataField="DEPT_NO_30" />
                                            <asp:BoundField DataField="DEPT_NAME_30" />
                                            <asp:BoundField DataField="DEPT_NO_40" />
                                            <asp:BoundField DataField="DEPT_NAME_40" />
                                            <asp:BoundField DataField="DEPT_NO_50" />
                                            <asp:BoundField DataField="DEPT_NAME_50" />
                                            <asp:BoundField DataField="DEPT_NO_60" />
                                            <asp:BoundField DataField="DEPT_NAME_60" />
                                            <asp:BoundField DataField="DEPT_NO_70" />
                                            <asp:BoundField DataField="DEPT_NAME_70" />
                                            <asp:BoundField DataField="DEPT_NAME_DESC" />
                                            <asp:BoundField DataField="DEPT_FULL_NAME" />
                                            <asp:BoundField DataField="DIV_DEPT_FULL_NAME" />
                                            <asp:BoundField DataField="PJOB_DESC" />
                                            
                                        </Columns>

                                    </asp:GridView>
                                </div>
                            </td>

                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Button ID="btn_confirm" runat="server" Text="確定" OnClick="btn_confirm_Click" />
                                <input id="btn_cancel" type="button" value="取消" onclick="closeWin();" />
                            </td>

                        </tr>
                        
                    </table>
                    <asp:HiddenField ID="HID_selectDeptNo" runat="server" />
                    
                </div>
                <asp:HiddenField ID="HID_mode" runat="server" ClientIDMode="Static"/>
                <div class="clear"></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
