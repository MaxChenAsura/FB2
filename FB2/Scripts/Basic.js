document.write('<link href=\"../../Content/themes/base/jquery-ui.css\" rel=\"stylesheet\" type=\"text/css\" />');
document.write('<link href=\"../../Content/themes/base/jquery.ui.datepicker.css\" rel=\"stylesheet\" type=\"text/css\" />');
document.write('<link href=\"../../Content/themes/base/styles.css\" rel=\"stylesheet\" type=\"text/css\" />');
document.write('<link href=\"../../Content/themes/base/GridView.css\" rel=\"stylesheet\" type=\"text/css\" />');
document.write('<script type=\"text/javascript\" src=\"../../Scripts/jquery-1.9.1.js\"></script>');
document.write('<script type=\"text/javascript\" src=\"../../Scripts/jquery-ui-1.10.4.min.js\"></script>');
document.write('<script type=\"text/javascript\" src=\"../../Scripts/gridviewScroll.min.js\"></script>');
document.write('<script type=\"text/javascript\" src=\"../../Scripts/jquery.mask.min.js\"></script>');
document.write('<script type=\"text/javascript\" src=\"../../Scripts/jshashtable-3.0.js\"></script>');
document.write('<script type=\"text/javascript\" src=\"../../Scripts/jquery.numberformatter-1.2.4.min.js\"></script>');
document.write('<script type=\"text/javascript\" src=\"../../Scripts/jquery.blockUI.js\"></script>');
document.write('<script type=\"text/javascript\" src=\"../../Scripts/datepicker-zh-TW.js\"></script>');
var parentFuncID = "";





function SelectNonDisabledCheckboxes(spanChk) {
if (spanChk.checked == false) {
        $(':checkbox[id*=cb_check]').prop('checked', false);
        return;
    }
$(":checkbox[id*=cb_check]").each(function (index, ele) {
        if ($(this).prop("disabled") == false) {
            $(this).prop('checked', true);
        }
    });

}

function SelectAllCheckboxes(spanChk) {
    elm = document.forms[0];
    for (i = 0; i <= elm.length - 1; i++) {
       
        if (elm[i].type == "checkbox" && elm[i].id != spanChk.id && elm[i].id.substr(elm[i].id.length - 8, 18) == 'cb_check') {
            
            if (elm.elements[i].checked != spanChk.checked)
                $('#' + elm[i].id).prop('checked', spanChk.checked);
        }
    }
}

function CheckBoxCheckAllByfreeze(checkboxs, checkbox) {
    $('#' + checkboxs).change(function () {
        var checked = $(this).is(':checked');
        if (checked == true) {
            $(':checkbox[id*="' + checkbox + '"]').each(function (index, ele) {
                if ($(this).prop("disabled") == false) {
                    $(this).prop('checked', true);
                }
            });
            //$('input[id*="' + checkbox + '"]').prop('checked', checked);
        }
        else
            $('input[id*="' + checkbox + '"]').prop('checked', '');

    });

    $('input[id*="' + checkbox + '"]').change(function () {
        var checked = $(this).is(':checked');
        var id = this.id.replace('_freezeitem', '');
        $('#'+id).prop('checked', checked);
        if (!checked) {
            $('#' + checkboxs).prop('checked', false);
        }
        else {
            var allchecked = true;
            $('input[id*="' + checkbox + '"]').each(function () {
                var checked = $(this).prop('checked');
                if (!checked) {
                    allchecked = false;
                }
            });
            $('#' + checkboxs).prop('checked', allchecked);
        }
    });
}
//Gridview radiobutton 不postback情況
function SelectOne(nameregex, current) {

    re = new RegExp(nameregex);



    for (i = 0; i < document.forms[0].elements.length; i++) {

        elm = document.forms[0].elements[i];

        if (elm.type == 'radio') {

            if (re.test(elm.name)) {

                elm.checked = false;

            }

        }

    }

    current.checked = true;

}

function OpenDeptSearch(dept_no, dept_name, supervisor,flag) {
    var myiFrameId = "iframe";
    var Url = "../comm/Dept_Search.aspx?mode=dept&super=" + supervisor + "&parentFuncId=" + parentFuncID;
    var dialogID = 'div_iframeID';
    var $dialog = $('<div id = "' + dialogID + '"></div>')
                .html('<iframe style="border: 0px; " src="' + Url + '" id="' + myiFrameId + '" width="100%" height="100%"></iframe>')
                .dialog({
                    autoOpen: false,
                    modal: true,
                    draggable: true,
                    resizable: false,
                    height: 500,
                    width: 350,
                    close: function (ev, ui) {
                        $("#" + dialogID).dialog("destroy");
                    }
                });
    $('#' + dialogID).attr('flag', flag);
    $('#' + dialogID).attr('stid', dept_no);
    $('#' + dialogID).attr('stname', dept_name);

    $dialog.dialog('open');   
}

function returnDEPT(id, name, value) {
    var returnValue = value;
    if (value == undefined) {
        returnValue = 'undefined';
    }
    //alert(returnValue);
    if (!(typeof returnValue === 'undefined')) {
        var obj = jQuery.parseJSON(returnValue);
        window.parent.$('#' + id).val(obj.DEPT_NO);
        window.parent.$('#' + name).val(obj.DEPT_NAME_DESC);

        return obj;

    }
}

//function OpenDeptSearch(dept_no, dept_name, supervisor) {
//    var returnValue = window.showModalDialog("../comm/Dept_Search.aspx?mode=dept&super=" + supervisor + "&parentFuncId=" + parentFuncID, self, 'dialogWidth=300px;dialogHeight=400px;scroll=no');
//    if (returnValue == undefined) {
//        returnValue = window.returnValue;
//    }
//    if (!(typeof returnValue === 'undefined')) {
//        var obj = jQuery.parseJSON(returnValue);
//        $("#" + dept_no).val(obj.DEPT_NO);
//        $("#" + dept_name).val(obj.DEPT_NAME_DESC);
        
//        return obj;

//    }
//}

function OpenDeptFullSearch(dept_no, dept_name, supervisor) {
    var returnValue = window.showModalDialog("../comm/Dept_Search.aspx?mode=dept&super=" + supervisor + "&parentFuncId=" + parentFuncID, self, 'dialogWidth=200px;dialogHeight=400px;scroll=no');
    if (returnValue == undefined) {
        returnValue = window.returnValue;
    }
    if (!(typeof returnValue === 'undefined')) {
        var obj = jQuery.parseJSON(returnValue);
        $("#" + dept_no).val(obj.DEPT_NO);
        $("#" + dept_name).val(obj.DEPT_FULL_NAME);

        return obj;

    }
}

function OpenDivDeptFullSearch(dept_no, dept_name, supervisor) {
    var returnValue = window.showModalDialog("../comm/Dept_Search.aspx?mode=dept&super=" + supervisor + "&parentFuncId=" + parentFuncID, self, 'dialogWidth=200px;dialogHeight=400px;scroll=no');
    if (returnValue == undefined) {
        returnValue = window.returnValue;
    }
    if (!(typeof returnValue === 'undefined')) {
        var obj = jQuery.parseJSON(returnValue);
        $("#" + dept_no).val(obj.DEPT_NO);
        $("#" + dept_name).val(obj.DIV_DEPT_FULL_NAME);

        return obj;

    }
}

/*
 Button Id,Emp_Id,Emp_Name,Supervisor,Flag
*/

//function OpenEmpSearch(id, emp_id, emp_name, supervisor, flag) {
//    alert("entry");
//    var Url = '../comm/Dept_Search.aspx?mode=all&super=' + supervisor + '&parentFuncId=' + parentFuncID + '&button_Id=' + id + '&flag=' + flag;
//    var cls = $('#' + id).attr('class');
//    var cla = $('#' + id).attr('target');
//    var clz = $('#' + id).attr('href');
//    alert(cls);
//    alert(cla);
//    alert(clz);
//    //$('#' + id).attr('class', 'nyroModal');
//    //$('#' + id).attr('target', '_blank');
//    $('#' + id).attr('href', Url);
//    $('#' + id).attr('eid', emp_id);
//    $('#' + id).attr('ename', emp_name);
    
//    nyroModal();

//}

function returnEMP(eid,ename,value) {
    var returnValue = value;
    if (value == undefined) {
        returnValue = 'undefined';
    }
    //alert(returnValue);
    if (!(typeof returnValue === 'undefined')) {        
        var obj = jQuery.parseJSON(returnValue);
        window.parent.$('#' + eid).val(obj.EMP_ID.trim());
        window.parent.$('#' + ename).val(obj.EMP_NAME.trim());      

        return obj;

    }
}


function OpenEmpSearch(emp_id, emp_name, supervisor,flag) {    
    var myiFrameId = "iframe";
    var Url = '../comm/Dept_Search.aspx?mode=all&super=' + supervisor + '&parentFuncId=' + parentFuncID;
    var dialogID = 'div_iframeID';
    var $dialog = $('<div id = "' + dialogID + '"></div>')
                .html('<iframe style="border: 0px; " src="' + Url + '" id="' + myiFrameId + '" width="100%" height="100%"></iframe>')
                .dialog({
                    autoOpen: false,
                    modal: true,
                    draggable: true,
                    resizable: false,
                    height: 500,
                    width: 950,
                    close: function (ev, ui) {                        
                        $("#" + dialogID).dialog("destroy");
                    }
                });
    $('#' + dialogID).attr('flag', flag);
    $('#' + dialogID).attr('stid', emp_id);
    $('#' + dialogID).attr('stname', emp_name);
        
    $dialog.dialog('open');
    
}



//function OpenEmpSearch(emp_id, emp_name, supervisor,flag) {
//    var returnValue;
//    var myiFrameId = "iframe";
//    var Url = '../comm/Dept_Search.aspx?mode=all&super=' + supervisor + '&parentFuncId=' + parentFuncID;    
//    window.open(Url,'NewWindows','height=450,width=1000px,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,top=240,left=370');
   
//}

//function showDialog(url, dialogID) {  //load content and open dialog
//    $("#" + dialogID).dialog({  //create dialog, but keep it closed
//        autoOpen: false,
//        height: 300,
//        width: 350,
//        modal: true
//    });
//    $("#" + dialogID).load(url);
//    $("#" + dialogID).dialog("open");
//}

//function returnEMP(emp_id, emp_name, value) {
//    var returnValue = value;
//    if (value == undefined) {
//        returnValue = 'undefined';
//    }
//    if (!(typeof returnValue === 'undefined')) {

//        var obj = jQuery.parseJSON(returnValue);
//        window.parent.$('#' + emp_id).val(obj.EMP_ID.trim());
//        window.parent.$('#' + emp_name).val(obj.EMP_NAME.trim());      

//        return obj;

//    }
//}

//function OpenEmpSearch(emp_id, emp_name,supervisor) {
//    var returnValue = window.showModalDialog("../comm/Dept_Search.aspx?mode=all&super=" + supervisor + "&parentFuncId=" + parentFuncID, self, 'dialogWidth=1000px;dialogHeight=400px;scroll=no;addressbar:No;');
//    if (returnValue == undefined) {
//        returnValue = window.returnValue;
//    }
//    if (!(typeof returnValue === 'undefined')) {

//        var obj = jQuery.parseJSON(returnValue);
//        window.parent.$('#' + emp_id).val(obj.EMP_ID.trim());
//        window.parent.$('#' + emp_name).val(obj.EMP_NAME.trim());
       
//        return obj;

//    }
//}

//function OpenEmpSearch(emp_id, emp_name,supervisor) {
//    var returnValue = window.showModalDialog("../comm/Dept_Search.aspx?mode=all&super=" + supervisor + "&parentFuncId=" + parentFuncID, self, 'dialogWidth=1000px;dialogHeight=400px;scroll=no;addressbar:No;');
//    if (returnValue == undefined) {
//        returnValue = window.returnValue;
//    }
//    if (!(typeof returnValue === 'undefined')) {

//        var obj = jQuery.parseJSON(returnValue);
//        $("#" + emp_id).val(obj.EMP_ID.trim());
//        $("#" + emp_name).val(obj.EMP_NAME.trim());

//        return obj;

//    }
//}

function OpenSearch(window_name, obj_cd, obj_desc, para,flag) {
    var myiFrameId = "iframe";
    var Url = "../comm/" + window_name + "?" + para + "&parentFuncId=" + parentFuncID;
    var dialogID = 'div_iframeID';
    var $dialog = $('<div id = "' + dialogID + '"></div>')
                .html('<iframe style="border: 0px; " src="' + Url + '" id="' + myiFrameId + '" width="100%" height="100%"></iframe>')
                .dialog({
                    autoOpen: false,
                    modal: true,
                    draggable: true,
                    resizable: false,
                    height: 550,
                    width: 550,
                    close: function (ev, ui) {
                        $("#" + dialogID).dialog("destroy");
                    }
                });
    $('#' + dialogID).attr('flag', flag);
    $('#' + dialogID).attr('stid', obj_cd);
    $('#' + dialogID).attr('stname', obj_desc);

    $dialog.dialog('open');

}

function returnOpenSearch(obj_cd, obj_desc, value) {
    var returnValue = value;
    if (returnValue == undefined) {
        returnValue = window.returnValue;
    }
    if (!(typeof returnValue === 'undefined')) {
        var obj = jQuery.parseJSON(returnValue);
        window.parent.$('#' + obj_cd).val(obj.CD);
        window.parent.$('#' + obj_desc).val(obj.DESC);
        
    }
}

function OpenPjobSearch(pjob_cd, pjob_desc) {
    var returnValue = window.showModalDialog("../comm/Pjob_Search.aspx", self, 'dialogWidth=520px;dialogHeight=400px;scroll=no');
    if (returnValue == undefined) {
        returnValue = window.returnValue;
    }
    if (!(typeof returnValue === 'undefined')) {

        var obj = jQuery.parseJSON(returnValue);
        window.parent.$("#" + pjob_cd).val(obj.PJOB_CD);
        window.parent.$("#" + pjob_desc).val(obj.PJOB_DESC);

    }
}
/* 目前無使用 */
function OpenMultiSelect(targetObj, TableName,TextColumn,ValueColumn,flag) {
    var myiFrameId = "iframe";
    var Url = "../comm/Multi_Select.aspx?TableName=" + TableName + "&TextColumn=" + TextColumn + "&ValueColumn=" + ValueColumn +
                        "&WhereColumn=" + WhereColumn + "&WhereValue=" + WhereValue + "&SelectValue=" + SelectValue + "&parentFuncId=" + parentFuncID;
    var dialogID = 'div_iframeID';
    var $dialog = $('<div id = "' + dialogID + '"></div>')
                .html('<iframe style="border: 0px; " src="' + Url + '" id="' + myiFrameId + '" width="100%" height="100%"></iframe>')
                .dialog({
                    autoOpen: false,
                    modal: true,
                    draggable: true,
                    resizable: false,
                    height: 550,
                    width: 550,
                    close: function (ev, ui) {
                        $("#" + dialogID).dialog("destroy");
                    }
                });
    $('#' + dialogID).attr('flag', flag);
    $('#' + dialogID).attr('target', targetObj);

    $dialog.dialog('open');

   
}

//targetObj: 要回傳資料的欄位
//TableName: 表格名稱  
//TextColumn +"-" ValueColumn :為選單的text, 
//ValueColumn:為選單的value,
//WhereValue :條件欄位的值
//WhereColumn:table 條件的欄位
//flag:Y  返回原始呼叫頁面function  returnMuiltSelectToPage(),進行其它的處理,若沒值或"" 會呼叫共用的 function  returnMultiSelect()
function OpenMultiSelectNew(targetObj, TableName, TextColumn, ValueColumn,WhereColumn,WhereValue,SelectValue, flag) {
    var myiFrameId = "iframe";
    var Url = "../comm/Multi_Select.aspx?TableName=" + TableName + "&TextColumn=" + TextColumn + "&ValueColumn=" + ValueColumn +
                        "&WhereColumn=" + WhereColumn + "&WhereValue=" + WhereValue + "&SelectValue=" + SelectValue + "&parentFuncId=" + parentFuncID;
    var dialogID = 'div_iframeID';
    var $dialog = $('<div id = "' + dialogID + '"></div>')
                .html('<iframe style="border: 0px; " src="' + Url + '" id="' + myiFrameId + '" width="100%" height="100%"></iframe>')
                .dialog({
                    autoOpen: false,
                    modal: true,
                    draggable: true,
                    resizable: false,
                    height: 550,
                    width: 600,
                    close: function (ev, ui) {
                        $("#" + dialogID).dialog("destroy");
                    }
                });
    $('#' + dialogID).attr('flag', flag);
    $('#' + dialogID).attr('target', targetObj);

    $dialog.dialog('open');
   
}
/*
function OpenMultiSelect(targetObj, TableName, TextColumn, ValueColumn, flag) {
    var myiFrameId = "iframe";
    var Url = "../comm/Multi_Select.aspx?TableName=" + TableName + "&TextColumn=" + TextColumn + "&ValueColumn=" + ValueColumn +
                        "&WhereColumn=" + WhereColumn + "&WhereValue=" + WhereValue + "&SelectValue=" + SelectValue + "&parentFuncId=" + parentFuncID;
    var dialogID = 'div_iframeID';
    var $dialog = $('<div id = "' + dialogID + '"></div>')
                .html('<iframe style="border: 0px; " src="' + Url + '" id="' + myiFrameId + '" width="100%" height="100%"></iframe>')
                .dialog({
                    autoOpen: false,
                    modal: true,
                    draggable: true,
                    resizable: false,
                    height: 550,
                    width: 550,
                    close: function (ev, ui) {
                        $("#" + dialogID).dialog("destroy");
                    }
                });
    $('#' + dialogID).attr('flag', flag);
    $('#' + dialogID).attr('target', targetObj);

    $dialog.dialog('open');

    //var returnValue = window.showModalDialog("../comm/Multi_Select.aspx?TableName=" + TableName + "&TextColumn=" + TextColumn + "&ValueColumn=" + ValueColumn,
    //                    self, 'dialogWidth=520px;dialogHeight=400px;scroll=no');
    //if (returnValue == undefined) {
    //    returnValue = window.returnValue;
    //}
    //if (!(typeof returnValue === 'undefined')) {

    //    $("#" + targetObj).val(returnValue);

    //}
}
*/
function returnMultiSelect(targetObj, send_select) {
    returnValue = send_select;
    if (returnValue == undefined) {
        returnValue = window.returnValue;
    }
    if (!(typeof returnValue === 'undefined')) {

        window.parent.$("#" + targetObj).val(returnValue);

    }
}
/*
此功能如要使用，需再轉換成jquery.dialog，以免ipad無法popup(20150115 目前尚未使用此function -- Terry)
*/
function OpenMultiSelectWindow(window_name, targetObj, TableName, TextColumn, ValueColumn) {
    var returnValue = window.showModalDialog("../comm/" + window_name + "?TableName=" + TableName + "&TextColumn=" + TextColumn + "&ValueColumn=" + ValueColumn,
                        self, 'dialogWidth=520px;dialogHeight=400px;scroll=no');
    if (returnValue == undefined) {
        returnValue = window.returnValue;
    }
    if (!(typeof returnValue === 'undefined')) {

        $("#" + targetObj).val(returnValue);

    }
}


function openWindowWithPost(url, name, keys, values) {
    var newWindow = window.open(url, name);
    if (!newWindow) return false;
    var html = "";
    html += "<html><head></head><body><form id='formid' method='post' action='" + url + "'>";
    if (keys && values && (keys.length == values.length))
        for (var i = 0; i < keys.length; i++)
            html += "<input type='hidden' name='" + keys[i] + "' value='" + values[i] + "'/>";
    html += "</form><script type='text/javascript'>document.getElementById(\"formid\").submit()</script></body></html>";
    newWindow.document.write(html);
    return newWindow;
}

function checkLicenseID(id) {
    tab = "ABCDEFGHJKLMNPQRSTUVXYWZIO"
    A1 = new Array(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3);
    A2 = new Array(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5);
    Mx = new Array(9, 8, 7, 6, 5, 4, 3, 2, 1, 1);

    if (id.length != 10) return false;
    i = tab.indexOf(id.charAt(0));
    if (i == -1) return false;
    sum = A1[i] + A2[i] * 9;

    for (i = 1; i < 10; i++) {
        v = parseInt(id.charAt(i));
        if (isNaN(v)) return false;
        sum = sum + v * Mx[i];
    }
    if (sum % 10 != 0) return false;
    return true;
}

function CheckValid() {
    setTimeout("delayedShow()", 1);
}

function delayedShow() {
    if (Page_IsValid != null && Page_IsValid == true) {
        BlockUI();
    }
}

function BlockUI() {
    $.blockUI({
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        },
        message:"請稍候..."
    });
}


//使數字格式每三位有comma (999,999,999)
//id: like % XXX  
//可以只傳  (txt_EDIT_ENV_ALLOWANCE_VALUE => ENV_ALLOWANCE_VALUE)，則新增修改也會同時有用
//dotnum: 小數點 (小數幾位)
function reComma(id, dotnum) {
    if (dotnum == 0) {
        $("input[id$=" + id + "]").blur(function () {
            $(this).parseNumber({ format: "#,###", locale: "tw" });
            $(this).formatNumber({ format: "#,###", locale: "tw" });
        });
        $("input[id$=" + id + "]").focus(function () {
            $(this).parseNumber({ format: "#,###", locale: "tw" });
            $(this).formatNumber({ format: ",####", locale: "tw" });
        });
    }
    if (dotnum == 1) {
        $("input[id$=" + id + "]").blur(function () {
            $(this).parseNumber({ format: "#,###.#", locale: "tw" });
            $(this).formatNumber({ format: "#,###.#", locale: "tw" });
        });
        $("input[id$=" + id + "]").focus(function () {
            $(this).parseNumber({ format: "#,###.#", locale: "tw" });
            $(this).formatNumber({ format: "#,###.#", locale: "tw" });
        });
    }
    if (dotnum == 2) {
        $("input[id$=" + id + "]").blur(function () {
            $(this).parseNumber({ format: "#,###.##", locale: "tw" });
            $(this).formatNumber({ format: "#,###.##", locale: "tw" });
        });
        $("input[id$=" + id + "]").focus(function () {
            $(this).parseNumber({ format: "#,###.##", locale: "tw" });
            $(this).formatNumber({ format: "#,###.##", locale: "tw" });
        });
    }
    if (dotnum == 3) {
        $("input[id$=" + id + "]").blur(function () {
            $(this).parseNumber({ format: "#,###.###", locale: "tw" });
            $(this).formatNumber({ format: "#,###.###", locale: "tw" });
        });
        $("input[id$=" + id + "]").focus(function () {
            $(this).parseNumber({ format: "#,###.###", locale: "tw" });
            $(this).formatNumber({ format: "#,###.###", locale: "tw" });
        });
    }
}


//使數字格式每三位有comma (999,999,999)
//id: like % XXX  
//可以只傳  (txt_EDIT_ENV_ALLOWANCE_VALUE => ENV_ALLOWANCE_VALUE)，則新增修改也會同時有用
//dotnum: 小數點 (小數幾位)
function reCommaOnly(id, dotnum) {
    if (dotnum == 0) {
        $("input[id$=" + id + "]").parseNumber({ format: "#,###", locale: "tw" });
        $("input[id$=" + id + "]").formatNumber({ format: "#,###", locale: "tw" });
    }
    if (dotnum == 1) {
        $("input[id$=" + id + "]").parseNumber({ format: "#,###.#", locale: "tw" });
        $("input[id$=" + id + "]").formatNumber({ format: "#,###.#", locale: "tw" });
        }
    if (dotnum == 2) {
        $("input[id$=" + id + "]").parseNumber({ format: "#,###.##", locale: "tw" });
        $("input[id$=" + id + "]").formatNumber({ format: "#,###.##", locale: "tw" });
    }
    if (dotnum == 3) {
        $("input[id$=" + id + "]").parseNumber({ format: "#,###.###", locale: "tw" });
        $("input[id$=" + id + "]").formatNumber({ format: "#,###.###", locale: "tw" });
    }
}


//將GridView的scollbar移到最低
function gridViewScrollBottom(id) {
    $("table[id$="+id+"]").parent().scrollTop(99999);
}

//取得今天的日期(yyyy/MM/dd)
function getTodayDate() {
    var day = new Date();
    var year = day.getFullYear();
    var month = day.getMonth() + 1;
    var date = day.getDate();
    month = (month < 10) ? "0" + month : month;
    date = (date < 10) ? "0" + date : date;
    var todayDate = year + "/" + month + "/" + date;
    return todayDate;
}

//取得今天的年月(yyyy/MM)
function getThisMonth() {
    var day = new Date();
    var year = day.getFullYear();
    var month = day.getMonth() + 1;
    var date = day.getDate();
    month = (month < 10) ? "0" + month : month;
    date = (date < 10) ? "0" + date : date;
    var todayDate = year + "/" + month ;
    return todayDate;
}

//檢驗只能輸入數字
function CheckIsNaN(source, arguments) {
    var value = $.trim(arguments.Value);
    if (isNaN(value)) {
        arguments.IsValid = false;
        return;
    }
    else {
        arguments.IsValid = true;
        return;
    }
}

//檢驗只能輸入數字(回傳true/false)
function CheckIsNumber(value) {
    if (isNaN(value)) {
        return false;
    } else {
        return true;
    }
}


/**
* 功能: 檢查出生日期的年度不可以大於系統年-14   <BR>
* 傳入: myForm,txtDate,true <BR>
*/
function CheckBirthday(source, arguments) {
    var value = $.trim(arguments.Value);
    if (value == "") {
        arguments.IsValid = true;
        return;
    }
    var day = new Date();
    var year = day.getFullYear() - 14;
    var intYear;
    intYear = parseInt(value.substr(0, 4), 10);
    if (intYear > year) {
        arguments.IsValid = false;
        return;
    }
    arguments.IsValid = true;
    return;
}

/**
* 功能: 檢查年月日正確性  <BR>
* 傳入: myForm,txtDate,true <BR>
* 備註: <input type="text" name="txtDate"><BR>
	<input type="button" onclick='CheckDate( myForm.txtDate, true );'><BR>
*/
function CheckDate(source, arguments) {

    var value = $.trim(arguments.Value);
    if (value == "") {
        arguments.IsValid = true;
        return;
    }

    if (value.length != 10) {
        arguments.IsValid = false;
        return;
    }
    var intYear, intday, intMonth;
    intYear = parseInt(value.substr(0, 4), 10);
    intMonth = parseInt(value.substr(5, 2), 10);
    intday = parseInt(value.substr(8, 2), 10);

    if (isNaN(intYear)) {
        arguments.IsValid = false;
        return;
    }
    else if (intYear < 1910) {
        arguments.IsValid = false;
        return;
    }
    else if (intMonth > 12 || intMonth < 1) {
        arguments.IsValid = false;
        return;
    }
    else if ((intMonth == 1 || intMonth == 3 || intMonth == 5 || intMonth == 7 || intMonth == 8 || intMonth == 10 || intMonth == 12) && (intday > 31 || intday < 1)) {
        arguments.IsValid = false;
        return;
    }
    else if ((intMonth == 4 || intMonth == 6 || intMonth == 9 || intMonth == 11) && (intday > 30 || intday < 1)) {
        arguments.IsValid = false;
        return;
    }
    else if (intMonth == 2) {

        if (intday < 1) {
            arguments.IsValid = false;
            return;
        }
        if (LeapYear(intYear) == true) {
            //alert(intday);
            if (intday > 29) {
                arguments.IsValid = false;
                return;
            }
        }
        else {
            if (intday > 28) {
                arguments.IsValid = false;
                return;
            }
        }
    }
}


/**
* 功能: 檢驗是否為潤年  <BR>
* 傳入: intYear<BR>
* 備註: <BR>
*/
function LeapYear(intYear) {
    if (intYear % 100 == 0) {
        if (intYear % 400 == 0) { return true; }
    }
    else if ((intYear % 4) == 0) {
        return true;
    }
    else {
        return false;
    }
}


//相關資料下載
function checkDowning(msg) {
    var processed = true;
    BlockUI();
    processed = confirm("確定要進行" + msg + "?");
    if (!processed) {
        $.unblockUI();
    }
    return processed;
}

function doUnBlock() {
    $.unblockUI();
}

//檢驗mail格式
function checkEmail(tStr) {
    var tReg = new RegExp("[a-zA-Z0-9._%-]+@[a-zA-Z0-9._%-]+\.[a-zA-Z]{2,4}");
    if (tStr.match(tReg)) {
        return true;
    }
    else {
        return false;
    }
}

//將json物件顯示出來
var printObj = typeof JSON !== "undefined" ? JSON.stringify : function (
        obj) {
    var arr = [];
    $.each(obj, function (key, val) {
        var next = key + ": ";
        next += $.isPlainObject(val) ? printObj(val) : val;
        arr.push(next);
    });
    return "{ " + arr.join(", ") + " }";
};

function doButtonConfirm(msg) {
    var processed = true;
    BlockUI();
    processed = confirm("確定要進行" + msg + "?");
    if (!processed) {
        $.unblockUI();
    } else {
        BlockUI();
    }
    return processed;
}

