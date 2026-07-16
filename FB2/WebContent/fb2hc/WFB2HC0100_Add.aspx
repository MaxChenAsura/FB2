<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hc/WFB2HC0100_Add.aspx.cs" Inherits="WebContent_WFB2HC0100_Add" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <style type="text/css">
        .gv_result_empty_td {
            padding-top: 3px;
            font-size: 14px;
            padding-left: 5px;
            padding-right: 5px;
        }
    </style>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
            //ini_gv_result1();

        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date2").mask('9999/99');
            $(".EnNum30").mask("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

            //setDetail_btn(true);
            //人事異動編號
            $("#txt_HR_CHG_NO").attr("ReadOnly", true);
            //生效處理狀態
            $("#txt_HR_CHG_PROC_STATUS_DESC").attr("ReadOnly", true);
            //員工姓名
            $("#txt_EMP_NAME").attr("ReadOnly", true);
            //人事異動代碼、說明
            //2.在考慮資料安全性下，僅能用查詢視窗開啟，不能自行輸入
            //$("#txt_HR_CHG_CD").attr("ReadOnly", true);
            //$("#txt_HR_CHG_CD_DESC").attr("ReadOnly", true);
            //異動主編號說明
            $("#txt_MAIN_HR_CHG_NO_DESC").attr("ReadOnly", true);
            //gv_result 部門名稱、職務名稱
            if ($("#txt_DEPT_NAME").length > 0)
                $("#txt_DEPT_NAME").attr("ReadOnly", true);
            if ($("#txt_PJOB_DESC").length > 0)
                $("#txt_PJOB_DESC").attr("ReadOnly", true);
            //gv_result2 異動前代碼、說明、異動後代碼說明
            if ($("#txt_BEFORE_CD").length > 0)
                $("#txt_BEFORE_CD").attr("ReadOnly", true);
            if ($("#txt_BEFORE_DESC").length > 0)
                $("#txt_BEFORE_DESC").attr("ReadOnly", true);
            if ($("#txt_AFTER_DESC").length > 0)
                $("#txt_AFTER_DESC").attr("ReadOnly", true);

            hr_chg_cd_input_set();

            $('#txt_PLAN_END_DT').change(function () {
                jQuery(document).ready(function () {
                    check_PLAN_END_DT();
                });
            });
          
            $.unblockUI();
        }

        function txt_EMP_ID_change() {
            if ($('#txt_EMP_ID').val().length == 5) {
                //ajax 取得員工姓名
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "GET_EMP_NAME",
                        EMP_ID: $('#txt_EMP_ID').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache:false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            $('#txt_EMP_NAME').val("");
                            $('#hid_PLAN_DESPATCH_DT').val("");
                            $('#hid_EMP_CD').val("");
                            $('#hid_LEVEL_CD').val("");
                            alert(JData.errMsg);
                        }
                        else if (JData.bolSTATUS) {
                            $('#txt_EMP_NAME').val(JData.strEMP_NAME);
                            $('#hid_PLAN_DESPATCH_DT').val(JData.strPLAN_DESPATCH_DT);
                            $('#hid_PLAN_DESPATCH_NEXT_DT').val(JData.strPLAN_DESPATCH_NEXT_DT);
                            $('#hid_EMP_CD').val(JData.strEMP_CD);
                            $('#hid_LEVEL_CD').val(JData.strLEVEL_CD);
                            chekc_C13();
                            check_same_data1();
                            check_same_data2();
                            //get_IS_END();
                            //get_MAIN_HR_CHG_NO();
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            } else {
                $('#txt_EMP_NAME').val("");
                $('#hid_PLAN_DESPATCH_DT').val("");
                $('#hid_EMP_CD').val("");
                $('#hid_LEVEL_CD').val("");
            }
        }

        function txt_START_DT_change() {
            if ($('#txt_START_DT').val().length == 10) {
                //ajax 異動生效日檢核
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "Check_FN_S_SALARY_YM",
                        START_DT: $('#txt_START_DT').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            alert(JData.errMsg);
                        } else if (JData.bolSTATUS) {
                            chekc_C13();
                            check_same_data1();
                            check_same_data2();
                            //check_INS_PLAN_PROC_DT();
                            //get_PLAN_END_DT();                            
                            check_PLAN_END_DT();
                            //get_IS_END();
                            //get_MAIN_HR_CHG_NO();
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
        }

        function txt_DEPT_NO_onblur() {
            $.ajax({
                url: "WFB2HC0100_DataProc.ashx",
                data: {
                    DATA_TYPE: "ADJUNCT_GET_DEPT_NAME",
                    DEPT_NO: $('#txt_DEPT_NO').val(),
                    START_DT: $('#txt_START_DT').val()
                },
                type: "GET",
                dataType: 'json',
                async: false,
                cache: false,
                success: function (JData) {
                    if (JData.errMsg != "" && JData.errMsg != null) {
                        $('#txt_DEPT_NAME').val("");
                        alert(JData.errMsg);
                    } else if (JData.bolSTATUS) {
                        $('#txt_DEPT_NAME').val(JData.strDEPT_NAME);
                    } else {
                        $('#txt_DEPT_NAME').val("");
                    }
                },

                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }

        function txt_PJOB_CD_onblur() {
            $.ajax({
                url: "WFB2HC0100_DataProc.ashx",
                data: {
                    DATA_TYPE: "ADJUNCT_GET_PJOB_DESC",
                    PJOB_CD: $('#txt_PJOB_CD').val(),
                    START_DT: $('#txt_START_DT').val()
                },
                type: "GET",
                dataType: 'json',
                async: false,
                cache: false,
                success: function (JData) {
                    if (JData.errMsg != "" && JData.errMsg != null) {
                        $('#txt_PJOB_DESC').val("");
                        alert(JData.errMsg);
                    } else if (JData.bolSTATUS) {
                        $('#txt_PJOB_DESC').val(JData.strPJOB_DESC);
                    } else {
                        $('#txt_PJOB_DESC').val("");
                    }
                },

                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }

        function txt_HR_CHG_CD_change() {
            chekc_C13();
            hr_chg_cd_input_set();
            check_same_data1();
            check_PLAN_END_DT();
           
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
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }

            //清空畫面
            function ClearAll() {
                $("#txt_EMP_ID_search").val("");
                $("#txt_EMP_NAME_search").val("");
                $("#txt_START_SDT_search").val("");
                $("#txt_START_EDT_search").val("");
                $("#txt_HR_CHG_CD_search").val("");
                $("#txt_HR_CHG_CD_DESC_search").val("");
                $("#rb_HR_CHG_PROC_STATUS_A_search").prop('checked', true);
            }

            
            function setDetail_btn(display) {
                //ini, save, cancel clicked
                if (display) {
                    //$("#init_add").show();
                    //$("#init_grid").show();
                    //$("#grid_add").hide();
                    $("#gv_result_empty_add").hide();
                    $("#gv_result2_empty_add").hide();
                }
                    //add, edit clicked
                else {
                    //$("#init_add").hide();
                    //$("#init_grid").hide();
                    //$("#grid_add").show();
                    $("#gv_result_empty_add").show();
                    $("#gv_result2_empty_add").show();
                }
            }

            //［判斷 人事異動代碼是否為  C13(契約期滿)］																																																																		
            //目的：當期間工離職時，且異動生效日(離職日) = 預計派遣日 時，建議人事異動代碼設為 C13(契約期滿)，因C13屬於非自願性離職
            //工號、異動生效日及人事異動代碼之後，其三個欄位任一onblur時，進行檢核動作。
            //同時滿足下面3個條件時，顯示錯誤訊息"人事異動代碼建議為「C13 契約期滿」"。
            //1. E.員工區分(hidden) = 2 (期間社員)
            //2.明細畫面.異動生效日 = E.預計派遣日(hidden)
            //3.明細畫面.人事異動代碼 <> C13
            function chekc_C13() {
                if ($("#ddl_HR_CHG_CD").val() != "" && $("#ddl_HR_CHG_CD").val().length == 3 && $("#ddl_HR_CHG_CD").val().substr(0, 1)=="C" ) {
                    if ($("#hid_EMP_CD").val() == "2" && $("#txt_START_DT").val() == $("#hid_PLAN_DESPATCH_DT").val() && $("#ddl_HR_CHG_CD").val() != "C13") {
                        alert($("#hid_wfb2hc_HR_CHG_CD_Proposal_C13_Message").val());
                    }
                }
            }
            //人事異動代碼
            //1.必須先輸入工號，才可以讀取可用的人事異動代碼，否則顯示錯誤訊息"必須先輸入工號，才可以輸入人事異動代碼"。
            function check_EMP_ID() {
                if ($("#txt_EMP_ID").val() == "") {
                    alert($("#hid_wfb2hc_must_enter_EMP_ID_before_enter_HR_CHG_CD_Message").val());
                    return false;
                } else {
                    return true;
                }
            }

            function hr_chg_cd_input_set() {
                //取得人事異動代碼之後，
                //(2.1)若 人事異動代碼，為 'B07'(國內出向支援) 或 'B08'(GCC駐在) 或 'B09'(ICT)，則 ［外調資料］區塊欄位ENABLED 可以輸入，
                if ($("#ddl_HR_CHG_CD").val() == 'B07' || $("#ddl_HR_CHG_CD").val() == 'B08' || $("#ddl_HR_CHG_CD").val() == 'B09') {
                    $("#fs_TRANSFER").children().attr("disabled", false);
                    //     其中，為'B09'時，才可以輸入 ICT類別欄位，否則DISABLED，下拉選單內容參考Sheet<Item Desc>的說明。
                    if ($("#ddl_HR_CHG_CD").val() == 'B09') {
                        $("#ddl_ICT_TYPE").prop("disabled", false);
                    } else {
                        $("#ddl_ICT_TYPE").prop("disabled", true);
                        $("#ddl_ICT_TYPE").val("");
                    }
                    //<受入國家>
                    //  1.若 明細畫面.人事異動代碼為'B07'(國內出向支援)，則 此欄位預設為'TWN-台灣'，且DISABLED不可修改。
                    if ($("#ddl_HR_CHG_CD").val() == 'B07') {
                        $("#ddl_TRANSFER_NATION_CD").val("TWN");
                        $("#ddl_TRANSFER_NATION_CD").prop("disabled", true);
                    } else {
                        $("#ddl_TRANSFER_NATION_CD").prop("disabled", false);
                    }
                } else {
                    //(2.2)若 人事異動代碼，不為 'B07' 且不為 'B08' 且不為 'B09'，則 ［外調資料］區塊欄位DISABLED 不可以輸入。
                    $("#fs_TRANSFER").children().attr("disabled", true);
                    $("#ddl_ICT_TYPE").val("");
                    $("#ddl_TRANSFER_NATION_CD").val("");
                    $("#ddl_TRANSFER_COMPANY_CD").val("");
                    $("#txt_TRANSFER_DEPT").val("");
                    $("#cb_IS_PAY_SUBSIST").prop("checked", false);
                }
            }

            //3.讀取 人事異動主檔 H
            //取得:	H.人事異動代碼
            //條件:	H.工號 = 明細畫面.工號
            //且 H.人事異動生效日 = 明細畫面.異動生效日
            //且 H.人事異動代碼 = 明細畫面.人事異動代碼
            //若讀得到資料，顯示錯誤訊息"相同工號、人事異動代碼、異動生效日期 的資料已經存在，請確認"
            //若讀不到資料，繼續作業。
            function check_same_data1() {
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "CHECK_SAME_DATA1",
                        EMP_ID: $('#txt_EMP_ID').val(),
                        START_DT: $('#txt_START_DT').val(),
                        HR_CHG_CD: $('#ddl_HR_CHG_CD').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            alert(JData.errMsg);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }

            //4.讀取 人事異動主檔 H
            //取得:	H.人事異動代碼
            //條件:	H.工號 = 明細畫面.工號
            //且 H.人事異動生效日 = 明細畫面.異動生效日
            //若讀得到資料，可能是多筆，
            //讀取 人事異動代碼檔 G1
            //取得:	G1.人事異動代碼說明
            //條件:	G1.人事異動代碼 = H.人事異動代碼
            //顯示提醒訊息"相同異動生效日存在 XXXXX, XXXXX 的人事異動單，是否繼續輸入？"	<-XXXXX 為 G1.人事異動代碼說明
            //若選擇不繼續輸入，則游標停留在異動生效日欄位。
            //若讀不到資料，繼續作業。
            function check_same_data2() {
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "CHECK_SAME_DATA2",
                        EMP_ID: $('#txt_EMP_ID').val(),
                        START_DT: $('#txt_START_DT').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            if (!confirm(JData.errMsg + '，' + $('#hid_wfb2hc_User_confirm_to_continue_to_enter').val())) {
                                $("#txt_START_DT").focus();
                            }
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
           
            //<狀態預計結束日>
            //若 G.是否暫時狀態(IS_TEMP)為'Y'，則 明細畫面.狀態預計結束日必須輸入，且必須>明細畫面.異動生效日，否則顯示錯誤訊息"狀態預計結束日必須輸入，且必須＞異動生效日"；
            function check_PLAN_END_DT() {
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "CHECK_PLAN_END_DT",
                        HR_CHG_CD: $('#ddl_HR_CHG_CD').val(),
                        START_DT: $('#txt_START_DT').val(),
                        PLAN_END_DT: $('#txt_PLAN_END_DT').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            alert(JData.errMsg);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
            function ddl_HR_CHG_ITEM_change() {
                if ($('#ddl_HR_CHG_ITEM').val() != "") {
                    var item = $('#ddl_HR_CHG_ITEM').val();
                    $.ajax({
                        url: "WFB2HC0100_DataProc.ashx",
                        data: {
                            DATA_TYPE: "Get_HR_CHG_ITEM_" + item + "_BEFORE",
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        async: false,
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "" && JData.errMsg != null) {
                                $("#txt_BEFORE_CD").val("");
                                $("#txt_BEFORE_DESC").val("");
                                $("#txt_AFTER_CD").val("");
                                $("#txt_AFTER_DESC").val("");
                                alert(JData.errMsg);
                            } else if (JData.bolSTATUS) {
                                $("#txt_BEFORE_CD").val(JData.strSUB_CD);
                                $("#txt_BEFORE_DESC").val(JData.strSUB_DESC);
                                $("#txt_AFTER_CD").val("");
                                $("#txt_AFTER_DESC").val("");
                            } else {
                                $("#txt_BEFORE_CD").val("");
                                $("#txt_BEFORE_DESC").val("");
                                $("#txt_AFTER_CD").val("");
                                $("#txt_AFTER_DESC").val("");
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $("#txt_BEFORE_CD").val("");
                    $("#txt_BEFORE_DESC").val("");
                    $("#txt_AFTER_CD").val("");
                    $("#txt_AFTER_DESC").val("");
                }
            }
            function txt_AFTER_CD_change() {
                if ($("#txt_AFTER_CD").val() != "" && $("#ddl_HR_CHG_ITEM").val() != "") {
                    $("#txt_AFTER_DESC").val("");
                    if ($("#ddl_HR_CHG_ITEM").val() == "01")
                        get_HR_CHG_ITEM_01_AFTER_DESC();
                    else if ($("#ddl_HR_CHG_ITEM").val() == "02")
                        get_HR_CHG_ITEM_02_AFTER_DESC();
                    else if ($("#ddl_HR_CHG_ITEM").val() == "03")
                        get_HR_CHG_ITEM_03_AFTER_DESC();
                    else if ($("#ddl_HR_CHG_ITEM").val() == "04")
                        get_HR_CHG_ITEM_04_AFTER_DESC();
                    else if ($("#ddl_HR_CHG_ITEM").val() == "05")
                        get_HR_CHG_ITEM_05_AFTER_DESC();
                    else if ($("#ddl_HR_CHG_ITEM").val() == "06")
                        get_HR_CHG_ITEM_06_AFTER_DESC();
                    else if ($("#ddl_HR_CHG_ITEM").val() == "07")
                        get_HR_CHG_ITEM_07_AFTER_DESC();
                    else if ($("#ddl_HR_CHG_ITEM").val() == "08")
                        get_HR_CHG_ITEM_08_AFTER_DESC();
                    else if ($("#ddl_HR_CHG_ITEM").val() == "09")
                        get_HR_CHG_ITEM_09_AFTER_DESC();
                    else if ($("#ddl_HR_CHG_ITEM").val() == "10")
                        get_HR_CHG_ITEM_10_AFTER_DESC();
                } else {
                    $("#txt_AFTER_DESC").val("");
                }
            }
            function get_HR_CHG_ITEM_01_AFTER_DESC() {
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "Get_HR_CHG_ITEM_01_AFTER",
                        COMPANY_CD: $('#txt_AFTER_CD').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            $("#txt_AFTER_DESC").val("");
                            alert(JData.errMsg);
                        } else if (JData.bolSTATUS) {
                            $("#txt_AFTER_DESC").val(JData.strSUB_DESC);
                        } else {
                            $("#txt_AFTER_DESC").val("");
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
            function get_HR_CHG_ITEM_02_AFTER_DESC() {
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "Get_HR_CHG_ITEM_02_AFTER",
                        PLANT_CD: $('#txt_AFTER_CD').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            $("#txt_AFTER_DESC").val("");
                            alert(JData.errMsg);
                        } else if (JData.bolSTATUS) {
                            $("#txt_AFTER_DESC").val(JData.strSUB_DESC);
                        } else {
                            $("#txt_AFTER_DESC").val("");
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
            function get_HR_CHG_ITEM_03_AFTER_DESC() {
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "Get_HR_CHG_ITEM_03_AFTER",
                        WS_CD: $('#txt_AFTER_CD').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            $("#txt_AFTER_DESC").val("");
                            alert(JData.errMsg);
                        } else if (JData.bolSTATUS) {
                            $("#txt_AFTER_DESC").val(JData.strSUB_DESC);
                        } else {
                            $("#txt_AFTER_DESC").val("");
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
            function get_HR_CHG_ITEM_04_AFTER_DESC() {
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "Get_HR_CHG_ITEM_04_AFTER",
                        EMP_CD: $('#txt_AFTER_CD').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            $("#txt_AFTER_DESC").val("");
                            alert(JData.errMsg);
                        } else if (JData.bolSTATUS) {
                            $("#txt_AFTER_DESC").val(JData.strSUB_DESC);
                        } else {
                            $("#txt_AFTER_DESC").val("");
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
            function get_HR_CHG_ITEM_05_AFTER_DESC() {
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "Get_HR_CHG_ITEM_05_AFTER",
                        DEPT_NO: $('#txt_AFTER_CD').val(),
                        START_DT: $('#txt_START_DT').val(),
                        EMP_ID: $('#txt_EMP_ID').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            $("#txt_AFTER_DESC").val("");
                            alert(JData.errMsg);
                        } else if (JData.bolSTATUS) {
                            $("#txt_AFTER_DESC").val(JData.strSUB_DESC);
                        } else {
                            $("#txt_AFTER_DESC").val("");
                        }
                        //判斷部門代號與廠別是否一致
                        if (JData.checkPlantMsg != "" && JData.checkPlantMsg != null) {
                            alert(JData.checkPlantMsg);
                        }

                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
            function get_HR_CHG_ITEM_06_AFTER_DESC() {
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "Get_HR_CHG_ITEM_06_AFTER",
                        LEVEL_CD: $('#txt_AFTER_CD').val(),
                        START_DT: $('#txt_START_DT').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            $("#txt_AFTER_DESC").val("");
                            $("#hid_LEVEL_CD_AFTER").val("");
                            alert(JData.errMsg);
                        } else if (JData.bolSTATUS) {
                            $("#txt_AFTER_DESC").val(JData.strSUB_DESC);
                            $("#hid_LEVEL_CD_AFTER").val($('#txt_AFTER_CD').val());
                        } else {
                            $("#txt_AFTER_DESC").val("");
                            $("#hid_LEVEL_CD_AFTER").val("");
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
            function get_HR_CHG_ITEM_07_AFTER_DESC() {
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "Get_HR_CHG_ITEM_07_AFTER",
                        GRADE_CD: $('#txt_AFTER_CD').val(),
                        LEVEL_CD: $('#hid_LEVEL_CD_AFTER').val(),
                        EMP_ID: $('#txt_EMP_ID').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            $("#txt_AFTER_DESC").val("");
                            alert(JData.errMsg);
                        } else if (JData.bolSTATUS) {
                            $("#txt_AFTER_DESC").val(JData.strSUB_DESC);
                        } else {
                            $("#txt_AFTER_DESC").val("");
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
            function get_HR_CHG_ITEM_08_AFTER_DESC() {
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "Get_HR_CHG_ITEM_08_AFTER",
                        PJOB_CD: $('#txt_AFTER_CD').val(),
                        START_DT: $('#txt_START_DT').val(),
                        EMP_ID: $('#txt_EMP_ID').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            $("#txt_AFTER_DESC").val("");
                            alert(JData.errMsg);
                        } else if (JData.bolSTATUS) {
                            $("#txt_AFTER_DESC").val(JData.strSUB_DESC);
                        } else {
                            $("#txt_AFTER_DESC").val("");
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
            function get_HR_CHG_ITEM_09_AFTER_DESC() {
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "Get_HR_CHG_ITEM_09_AFTER",
                        WORK_SHIFT_CD: $('#txt_AFTER_CD').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            $("#txt_AFTER_DESC").val("");
                            alert(JData.errMsg);
                        } else if (JData.bolSTATUS) {
                            $("#txt_AFTER_DESC").val(JData.strSUB_DESC);
                        } else {
                            $("#txt_AFTER_DESC").val("");
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
            function get_HR_CHG_ITEM_10_AFTER_DESC() {
                $.ajax({
                    url: "WFB2HC0100_DataProc.ashx",
                    data: {
                        DATA_TYPE: "Get_HR_CHG_ITEM_10_AFTER",
                        WORK_CD: $('#txt_AFTER_CD').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            $("#txt_AFTER_DESC").val("");
                            alert(JData.errMsg);
                        } else if (JData.bolSTATUS) {
                            $("#txt_AFTER_DESC").val(JData.strSUB_DESC);
                        } else {
                            $("#txt_AFTER_DESC").val("");
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
            function btn_AFTER_CD_click() {
                if ($("#ddl_HR_CHG_ITEM").val() == "01") {
                    //(1.3)若CLICK異動後代碼欄的BUTTON，開啟<<共用代碼查詢視窗>>傳入('HB','COMPANY_CD')(明細畫面.異動後代碼)(無)，選取其一之後取得代碼及說明，顯示於畫面上。                    
                    OpenSearch('Company_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'COMPANY_CD=' + $("#txt_AFTER_CD").val());
                } else if ($("#ddl_HR_CHG_ITEM").val() == "02") {
                    //(2.3)若CLICK異動後代碼欄的BUTTON，開啟<<共用代碼查詢視窗>>傳入('HB','PLANT_CD')(明細畫面.異動後代碼)(無)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																		
                    OpenSearch('CommCode_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'MAIN_CD=PLANT_CD&SYS_CD=HB&SUB_CD=' + $("#txt_AFTER_CD").val());
                } else if ($("#ddl_HR_CHG_ITEM").val() == "03") {
                    //(3.3)若CLICK異動後代碼欄的BUTTON，開啟<<共用代碼查詢視窗>>傳入('HB','WS_CD')(明細畫面.異動後代碼)(無)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																
                    OpenSearch('CommCode_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'MAIN_CD=WS_CD&SYS_CD=HB&SUB_CD=' + $("#txt_AFTER_CD").val());
                } else if ($("#ddl_HR_CHG_ITEM").val() == "04") {
                    //(4.3)若CLICK異動後代碼欄的BUTTON，開啟<<共用代碼查詢視窗>>傳入('HB','EMP_CD')(明細畫面.異動後代碼)(無)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																	
                    OpenSearch('CommCode_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'MAIN_CD=EMP_CD&SYS_CD=HB&SUB_CD=' + $("#txt_AFTER_CD").val());
                } else if ($("#ddl_HR_CHG_ITEM").val() == "05") {
                    //(5.3)若CLICK異動後代碼欄的BUTTON，開啟<<部門查詢視窗-清單>>傳入(無)(明細畫面.異動後代碼)(明細畫面.異動生效日)，選取其一之後取得代號及部門名稱，顯示於畫面上。																																																																																							
                    OpenSearch('DeptGrid_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'DEPT_NO=' + $("#txt_AFTER_CD").val() + '&START_DT=' + $("#txt_START_DT").val(),"Y");
                } else if ($("#ddl_HR_CHG_ITEM").val() == "06") {
                    //(6.3)若CLICK異動後代碼欄的BUTTON，開啟<<資格查詢視窗>>傳入(無)(明細畫面.異動後代碼)(明細畫面.異動生效日)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																		
                    OpenSearch('EmpLevel_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'LEVEL_CD=' + $("#txt_AFTER_CD").val() + '&START_DT=' + $("#txt_START_DT").val());
                } else if ($("#ddl_HR_CHG_ITEM").val() == "07") {
                    //(7.3)若CLICK異動後代碼欄的BUTTON，開啟<<資格級數查詢視窗>>傳入(無)(異動項目:'06-資格' 之異動後代碼 或 E.資格代號,明細畫面.異動後代碼)(明細畫面.異動生效日)，																																																																																					
                    //選取其一之後取得代碼及說明，顯示於畫面上。
                    OpenSearch('EmpGrade_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'GRADE_CD=' + $("#txt_AFTER_CD").val() + '&IS_VALID=Y&LEVEL_CD=' + $("#hid_LEVEL_CD_AFTER").val() + '&EMP_ID=' + $("#txt_EMP_ID").val());
                } else if ($("#ddl_HR_CHG_ITEM").val() == "08") {
                    //(8.3)若CLICK異動後代碼欄的BUTTON，開啟<<職務查詢視窗>>傳入(無)(E.職種,E.資格代號,明細畫面.異動後代碼)(明細畫面.異動生效日)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																												
                    //(8.4)若G.是否專業職務為'Y'，則所選取的異動後代碼，必須是P開頭的職務代號，否則顯示錯誤訊息"職務代號必須是專業職的代號"																																																																																												
                    OpenSearch('Pjob_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'WS_CD=' + $("#hid_WS_CD").val() + '&LEVEL_CD=' + $("#hid_LEVEL_CD").val() + '&PJOB_CD=' + $("#txt_AFTER_CD").val() + '&START_DT=' + $("#txt_START_DT").val());
                } else if ($("#ddl_HR_CHG_ITEM").val() == "09") {
                    //(9.3)若CLICK異動後代碼欄的BUTTON，開啟<<輪值表查詢視窗>>傳入(無)(明細畫面.異動後代碼)(資料權限之「小分類」)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																				
                    OpenSearch('WorkShift_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'WORK_SHIFT_CD=' + $("#txt_AFTER_CD").val() + '&WORKER_WORK_SHIFT=' + $("#hid_SysCodeAtt").val());
                } else if ($("#ddl_HR_CHG_ITEM").val() == "10") {
                    //(10.3)若CLICK異動後代碼欄的BUTTON，開啟<<共用代碼查詢視窗>>傳入('HB','WORK_CD')(明細畫面.異動後代碼)(無)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																			
                    OpenSearch('CommCode_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'MAIN_CD=WORK_CD&SUB_CD=' + $("#txt_AFTER_CD").val());
                }
            }
            function emp_id_btn_click() {
                var json = OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');
            }

            function Check_ID_LENGTH(source, arguments) {
                if ($("#txt_EMP_ID").val().length != 5)
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }


            function DeptGrid_Search(id, name, json) {
                var returnValue = json;
                if (json == undefined) {
                    returnValue = 'undefined';
                }
                if (!(typeof returnValue === 'undefined')) {
                    var obj = jQuery.parseJSON(returnValue);
                    $("#txt_AFTER_CD").val(obj.CD);
                    //$("#txt_AFTER_DESC").val(obj.DESC);
                    get_HR_CHG_ITEM_05_AFTER_DESC();
                } 
            }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">
                <tr valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="14%" />
                                <col width="24%" />
                                <col width="14%" />
                                <col width="16%" />
                                <col width="14%" />
                                <col width="18%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <td height="10px;"></td>
                                </tr>
                                <tr>
                                    <%--人事異動編號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HR_CHG_NO" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_HR_CHG_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_HR_CHG_NO" runat="server" size="10" ClientIDMode="Static" Style="border: none"></asp:TextBox>
                                    </td>
                                    <%--生效處理狀態--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HR_CHG_PROC_STATUS" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_HR_CHG_PROC_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:TextBox ID="txt_HR_CHG_PROC_STATUS_DESC" runat="server" size="10" ClientIDMode="Static" Style="border: none" Text="<%$Resources:Resource,wfb2hc_lb_HR_CHG_PROC_STATUS_N%>"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--工號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" size="5" ClientIDMode="Static" CssClass="MandatoryField" OnTextChanged="txt_EMP_ID_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <input type="button" id="btn_EMP_ID" onclick="emp_id_btn_click();" value="..." />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" size="5" ClientIDMode="Static" Style="border: none"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_EMP_ID" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_EMP_ID%>"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" Display="None" ValidationGroup="GroupA"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hc_ERR_EMP_ID_LENGTH5%>" ClientValidationFunction="Check_ID_LENGTH" ForeColor="Red"
                                            ControlToValidate="txt_EMP_ID" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_EMP_ID%>"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" Display="None" ValidationGroup="GroupB"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hc_ERR_EMP_ID_LENGTH5%>" ClientValidationFunction="Check_ID_LENGTH" ForeColor="Red"
                                            ControlToValidate="txt_EMP_ID" ValidationGroup="GroupB" Display="None">
                                        </asp:CustomValidator>

                                    </td>
                                    <%--異動生效日--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_START_DT1%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_START_DT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" CssClass="MandatoryField date" OnTextChanged="txt_START_DT_TextChanged" AutoPostBack="true"  CausesValidation="true" ValidationGroup="GroupC"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_START_DT" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_START_DT%>"
                                            ControlToValidate="txt_START_DT" ForeColor="Red" Display="None" ValidationGroup="GroupA"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cv_START_DT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hc_START_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_START_DT%>"
                                            ControlToValidate="txt_START_DT" ForeColor="Red" Display="None" ValidationGroup="GroupB"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hc_START_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>

                                       <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hc_START_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
                                    </td>
                                    <%--人事異動代碼--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HR_CHG_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_HR_CHG_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                         <asp:DropDownList ID="ddl_HR_CHG_CD" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="txt_HR_CHG_CD_TextChanged" AutoPostBack="true" CssClass="MandatoryField"></asp:DropDownList>
                                        <%--<asp:TextBox ID="txt_HR_CHG_CD" runat="server" MaxLength="3" size="5" ClientIDMode="Static" CssClass="MandatoryField" OnTextChanged="txt_HR_CHG_CD_change" AutoPostBack="true"></asp:TextBox>
                                        <input type="button" id="btn_HR_CHG_CD" onclick="hr_chg_cd_btn_click();" value="..." />                                             
                                        <asp:TextBox ID="txt_HR_CHG_CD_DESC" runat="server" size="20" ClientIDMode="Static" Style="border: none"></asp:TextBox>--%>
                                        <asp:RequiredFieldValidator ID="rfv_HR_CHG_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_HR_CHG_CD%>"
                                            ControlToValidate="ddl_HR_CHG_CD" ForeColor="Red" Display="None" ValidationGroup="GroupA"></asp:RequiredFieldValidator>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_HR_CHG_CD%>"
                                            ControlToValidate="ddl_HR_CHG_CD" ForeColor="Red" Display="None" ValidationGroup="GroupB"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--保險預計處理日--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_INS_PLAN_PROC_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_INS_PLAN_PROC_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_INS_PLAN_PROC_DT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" CssClass="date" OnTextChanged="txt_INS_PLAN_PROC_DT_TextChanged" AutoPostBack="true"   CausesValidation="true" ValidationGroup="GroupC"></asp:TextBox>
                                        <asp:CustomValidator ID="cv_INS_PLAN_PROC_DT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hc_INS_PLAN_PROC_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_INS_PLAN_PROC_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                                        <asp:CustomValidator ID="groupc_INS_PLAN_PROC_DT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hc_INS_PLAN_PROC_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_INS_PLAN_PROC_DT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>

                                    </td>
                                    <%--狀態預計結束日--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PLAN_END_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_PLAN_END_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:TextBox ID="txt_PLAN_END_DT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        <asp:CustomValidator ID="cv_PLAN_END_DT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hc_PLAN_END_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_PLAN_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--狀態結束--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_END" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_IS_END%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:CheckBox ID="cb_IS_END" runat="server" ClientIDMode="Static" OnCheckedChanged="cb_IS_END_CheckedChanged" AutoPostBack="true" />
                                    </td>
                                    <%--異動主編號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MAIN_HR_CHG_NO" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_MAIN_HR_CHG_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:DropDownList ID="ddl_MAIN_HR_CHG_NO" runat="server" ClientIDMode="Static" OnTextChanged="ddl_MAIN_HR_CHG_NO_TextChanged" AutoPostBack="true"></asp:DropDownList>
                                        <asp:TextBox ID="txt_MAIN_HR_CHG_NO_DESC" runat="server" size="80" Style="border: none;" ClientIDMode="Static" />
                                    </td>
                                </tr>
                                <tr>
                                    <!--
                               		<th align="left" class="Body_TableHeader">生效處理訊息:</th>
                                	<td align="left" class="Body_label" colspan=5>
										<input type="text" name="prodDt" maxlength="10" size="20" tabindex="4" value="" style="border:none" readonly>
									</td>
									-->
                                </tr>
                                <tr>
                                    <td align="left" class="Body_label" colspan="8">
                                        <fieldset id="fs_TRANSFER" style="padding: 5px; border-color: darkblue;">
                                            <legend class="Body_label"><font color="darkblue">
                                                <asp:Label ID="lb_TRANSFER" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_TRANSFER%>"></asp:Label></font></legend>
                                            <table border="0" width="100%">
                                                <tr>
                                                    <%--ICT類別--%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_ICT_TYPE" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_ICT_TYPE%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_ICT_TYPE" runat="server" ClientIDMode="Static" />
                                                    </td>
                                                    <%--受入國家--%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_TRANSFER_NATION_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_TRANSFER_NATION_CD%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_TRANSFER_NATION_CD" runat="server" ClientIDMode="Static" />
                                                    </td>
                                                    <%--受入公司--%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_TRANSFER_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_TRANSFER_COMPANY_CD%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_TRANSFER_COMPANY_CD" runat="server" ClientIDMode="Static" />
                                                        <%--<select id="ddl_TRANSFER_COMPANY_CD" onchange="ddl_TRANSFER_COMPANY_CD_change();" />--%>
                                                    </td>
                                                    <%--受入部門--%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_TRANSFER_DEPT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_TRANSFER_DEPT%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_TRANSFER_DEPT" runat="server" MaxLength="30" size="30" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                    <%--發放預付薪--%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_IS_PAY_SUBSIST" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_IS_PAY_SUBSIST%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:CheckBox ID="cb_IS_PAY_SUBSIST" runat="server" ClientIDMode="Static" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" height="1" colspan="10">
                                        <hr>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="10">
                                        <div id="grid1">
                                            <div id="init_add" style="display: inline">
                                                <aces:Btn ID="WFB2HC0100Detail_Add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="WFB2HC0100Detail_Add_Click" CausesValidation="false" />

                                                <%--<asp:Button ID="WFB2HC0100Detail_Add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="WFB2HC0100Detail_Add_Click" CausesValidation="false" />--%>
                                            </div>
                                            <div id="init_grid" style="display: inline">
                                            <aces:Btn ID="WFB2HC0100Detail_Delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClick="WFB2HC0100Detail_Delete_Click" />
                                                <aces:Btn ID="WFB2HC0100Detail_Edit" runat="server" Text="<%$Resources:Resource,btn_edit%>" OnClick="WFB2HC0100Detail_Edit_Click" />

<%--                                                <asp:Button ID="WFB2HC0100Detail_Delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClick="WFB2HC0100Detail_Delete_Click" />
                                                <asp:Button ID="WFB2HC0100Detail_Edit" runat="server" Text="<%$Resources:Resource,btn_edit%>" OnClick="WFB2HC0100Detail_Edit_Click" />--%>
                                            </div>
                                            <div id="grid_add">
                                                <aces:Btn ID="WFB2HC0100Detail_Save" runat="server" Text="<%$Resources:Resource,wfb2hc_btn_detail_ok%>" OnClick="WFB2HC0100Detail_Save_Click" ValidationGroup="GroupB" />

<%--                                                <asp:Button ID="WFB2HC0100Detail_Save" runat="server" Text="<%$Resources:Resource,wfb2hc_btn_detail_ok%>" OnClick="WFB2HC0100Detail_Save_Click" ValidationGroup="GroupB" />--%>
                                                <asp:Button ID="WFB2HC0100Detail_Cancel" runat="server" Text="<%$Resources:Resource,btn_cancel%>" OnClick="WFB2HC0100Detail_Cancel_Click" />
                                            </div>
                                            <asp:HiddenField ID="hid_EMP_ID_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_START_SDT_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_START_EDT_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_HR_CHG_CD_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_HR_CHG_PROC_STATUS_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_PLAN_DESPATCH_DT" ClientIDMode="Static" runat="server" />
                                            <%--預計派遣日--%>
                                            <asp:HiddenField ID="hid_PLAN_DESPATCH_NEXT_DT" ClientIDMode="Static" runat="server" />
                                            <%--預計派遣日次日--%>
                                            <asp:HiddenField ID="hid_EMP_CD" ClientIDMode="Static" runat="server" />
                                            <%--員工區分--%>
                                            <asp:HiddenField ID="hid_WS_CD" ClientIDMode="Static" runat="server" />
                                            <%--職種--%>
                                            <asp:HiddenField ID="hid_LEVEL_CD" ClientIDMode="Static" runat="server" />
                                            <%--資格--%>
                                            <asp:HiddenField ID="hid_SysCodeAtt" ClientIDMode="Static" runat="server" />
                                            <%--資料權限之「小分類」--%>
                                            <asp:HiddenField ID="hid_LOGIN_ID" ClientIDMode="Static" runat="server" />
                                            <%--登入者帳號--%>
                                            <asp:HiddenField ID="hid_IS_TEMP" ClientIDMode="Static" runat="server" />
                                            <%--是否暫時狀態--%>
                                            <asp:HiddenField ID="hid_LEVEL_CD_AFTER" ClientIDMode="Static" runat="server" />
                                            <%--明細06-資格之異動後代碼--%>
                                            <asp:HiddenField ID="hid_IS_PROFESSION_PJOB" ClientIDMode="Static" runat="server" />
                                            <%--是否專業職務--%>
                                            <asp:HiddenField ID="hid_wfb2hc_CheckBox_NotChoiceMessage" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_wfb2hc_Delete_Confirm_Message" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_wfb2hc_HR_CHG_CD_Proposal_C13_Message" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_wfb2hc_must_enter_EMP_ID_before_enter_HR_CHG_CD_Message" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_wfb2hc_Cancel_Confirm_Message" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_wfb2hc_User_confirm_to_continue_to_enter" ClientIDMode="Static" runat="server" />
                                            <%--<asp:HiddenField ID="hid_wfb2hc_HR_CHG_CD_can_not_change_because_detail_is_modify_Message" ClientIDMode="Static" runat="server" />--%>
                                        </div>
                                    </td>
                                </tr>
                                <!-- END: Create a line -->
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
            <div id="div_GridView1">
                <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                    AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                    OnRowDataBound="gv_result_RowDataBound" Width="1020px"
                    OnRowCreated="gv_result_RowCreated">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID='cb_all' runat='server' onclick='javascript:SelectAllCheckboxes(this);' ClientIDMode='Static' />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                            </ItemTemplate>
                            <HeaderStyle Width="20px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lb_RowNumber" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_RowNumber%>"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_DEPT_NO%>"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_NO")%>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" size="7" Text='<%#Bind("DEPT_NO")%>' ClientIDMode="Static" CssClass="MandatoryField" />
                                <asp:Button runat="server" ID="btn_DEPT_NO" Text="..." ClientIDMode="Static" />
                                <asp:RequiredFieldValidator ID="rfv_DEPT_NO" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_DEPT_NO%>"
                                    ControlToValidate="txt_DEPT_NO" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" size="7" Text='<%#Bind("DEPT_NO")%>' ClientIDMode="Static" CssClass="MandatoryField" />
                                <asp:Button runat="server" ID="btn_DEPT_NO" Text="..." ClientIDMode="Static" />
                                <asp:RequiredFieldValidator ID="rfv_DEPT_NO" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_DEPT_NO%>"
                                    ControlToValidate="txt_DEPT_NO" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                </asp:RequiredFieldValidator>
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="150px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lb_DEPT_NAME" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_DEPT_NAME%>"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME")%>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_DEPT_NAME" runat="server" ClientIDMode="Static" size="43" Style="border: none;" Text='<%#Bind("DEPT_NAME")%>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txt_DEPT_NAME" runat="server" ClientIDMode="Static" size="43" Style="border: none;" Text='<%#Bind("DEPT_NAME")%>'></asp:TextBox>
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="300px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_PJOB_CD%>"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_PJOB_CD" runat="server" Text='<%#Bind("PJOB_CD")%>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_PJOB_CD" MaxLength="4" size="4" runat="server" Text='<%#Bind("PJOB_CD")%>' ClientIDMode="Static" CssClass="MandatoryField" />
                                <asp:Button runat="server" ID="btn_PJOB_CD" Text="..." ClientIDMode="Static" />
                                <asp:RequiredFieldValidator ID="rfv_PJOB_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_PJOB_CD%>"
                                    ControlToValidate="txt_PJOB_CD" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txt_PJOB_CD" MaxLength="4" size="4" runat="server" Text='<%#Bind("PJOB_CD")%>' ClientIDMode="Static" CssClass="MandatoryField" />
                                <asp:Button runat="server" ID="btn_PJOB_CD" Text="..." ClientIDMode="Static" />
                                <asp:RequiredFieldValidator ID="rfv_PJOB_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_PJOB_CD%>"
                                    ControlToValidate="txt_PJOB_CD" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                </asp:RequiredFieldValidator>
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="150px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lb_PJOB_DESC" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_PJOB_DESC%>"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_PJOB_DESC" runat="server" ClientIDMode="Static" Text='<%#Bind("PJOB_DESC")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_PJOB_DESC" runat="server" ClientIDMode="Static" size="43" Style="border: none;" Text='<%#Bind("PJOB_DESC")%>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txt_PJOB_DESC" runat="server" ClientIDMode="Static" size="43" Style="border: none;" Text='<%#Bind("PJOB_DESC")%>'></asp:TextBox>
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="300px" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="GridviewScrollPager" />
                    <FooterStyle CssClass="GridviewScrollPager" />
                    <EmptyDataTemplate>
                        <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                            <tr class="header">
                                <td width="20px"></td>
                                <td width="80px">
                                    <asp:Label ID="lb_RowNumber" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_RowNumber%>"></asp:Label>
                                </td>
                                <td width="150px">
                                    <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_DEPT_NO%>"></asp:Label>
                                </td>
                                <td width="300px">
                                    <asp:Label ID="lb_DEPT_NAME" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_DEPT_NAME%>"></asp:Label>
                                </td>
                                <td width="150px">
                                    <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_PJOB_CD%>"></asp:Label>
                                </td>
                                <td width="300px">
                                    <asp:Label ID="lb_PJOB_DESC" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_PJOB_DESC%>"></asp:Label>
                                </td>
                            </tr>
                            <tr id="gv_result_empty_add" class="normal" style="display: none;">
                                <td class="gv_result_empty_td"></td>
                                <td class="gv_result_empty_td">
                                    <div style="text-align: center;">
                                        <asp:Label ID="txt_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' ClientIDMode="Static"></asp:Label>
                                    </div>
                                </td>
                                <td class="gv_result_empty_td">
                                    <div style="text-align: left;">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" size="7" Text='<%#Bind("DEPT_NO")%>' ClientIDMode="Static" CssClass="MandatoryField" />
                                        <asp:Button runat="server" ID="btn_DEPT_NO" Text="..." ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="rfv_DEPT_NO" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_DEPT_NO%>"
                                            ControlToValidate="txt_DEPT_NO" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td class="gv_result_empty_td">
                                    <div style="text-align: left;">
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" ClientIDMode="Static" size="43" Style="border: none;"></asp:TextBox>
                                    </div>
                                </td>
                                <td class="gv_result_empty_td">
                                    <div style="text-align: left;">
                                        <asp:TextBox ID="txt_PJOB_CD" MaxLength="4" size="4" runat="server" Text='<%#Bind("PJOB_CD")%>' ClientIDMode="Static" CssClass="MandatoryField" />
                                        <asp:Button runat="server" ID="btn_PJOB_CD" Text="..." ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="rfv_PJOB_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_PJOB_CD%>"
                                            ControlToValidate="txt_PJOB_CD" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td class="gv_result_empty_td">
                                    <div style="text-align: left;">
                                        <asp:TextBox ID="txt_PJOB_DESC" runat="server" ClientIDMode="Static" size="43" Style="border: none;"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>

            <div id="div_GridView2">
                <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                    AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting2"
                    OnRowDataBound="gv_result_RowDataBound2" Width="1020px"
                    OnRowCreated="gv_result_RowCreated2">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID='cb_all' runat='server' onclick='javascript:SelectAllCheckboxes(this);' ClientIDMode='Static' />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                            </ItemTemplate>
                            <HeaderStyle Width="20px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lb_RowNumber" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_RowNumber%>"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="70px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lb_HR_CHG_ITEM" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_HR_CHG_ITEM%>"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_HR_CHG_ITEM_DESC" runat="server" Text='<%#Bind("HR_CHG_ITEM_DESC")%>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddl_HR_CHG_ITEM" runat="server" ClientIDMode="Static" CssClass="MandatoryField" />
                                <asp:RequiredFieldValidator ID="rfv_HR_CHG_ITEM" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_HR_CHG_ITEM%>"
                                    ControlToValidate="ddl_HR_CHG_ITEM" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hid_HR_CHG_ITEM" ClientIDMode="Static" runat="server" Value='<%#Bind("HR_CHG_ITEM")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddl_HR_CHG_ITEM" runat="server" Text='<%#Bind("HR_CHG_ITEM")%>' ClientIDMode="Static" CssClass="MandatoryField" />
                                <asp:RequiredFieldValidator ID="rfv_HR_CHG_ITEM" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_HR_CHG_ITEM%>"
                                    ControlToValidate="ddl_HR_CHG_ITEM" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                </asp:RequiredFieldValidator>
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="150px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lb_BEFORE_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_BEFORE_CD%>"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_BEFORE_CD" runat="server" Text='<%#Bind("BEFORE_CD")%>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_BEFORE_CD" size="10" runat="server" Text='<%#Bind("BEFORE_CD")%>' ClientIDMode="Static" Style="border: none"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txt_BEFORE_CD" size="10" runat="server" Text='<%#Bind("BEFORE_CD")%>' ClientIDMode="Static" Style="border: none"></asp:TextBox>
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="150px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lb_BEFORE_CD_DESC" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_BEFORE_CD_DESC%>"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_BEFORE_DESC" runat="server" Text='<%#Bind("BEFORE_DESC")%>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_BEFORE_DESC" size="32" runat="server" Text='<%#Bind("BEFORE_DESC")%>' ClientIDMode="Static" Style="border: none"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txt_BEFORE_DESC" size="32" runat="server" Text='<%#Bind("BEFORE_DESC")%>' ClientIDMode="Static" Style="border: none"></asp:TextBox>
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="230px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lb_AFTER_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_AFTER_CD%>"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_AFTER_CD" runat="server" Text='<%#Bind("AFTER_CD")%>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_AFTER_CD" runat="server" size="10" MaxLength="30" Text='<%#Bind("AFTER_CD")%>' ClientIDMode="Static" CssClass="EnNum30 MandatoryField" />
                                <asp:Button runat="server" ID="btn_AFTER_CD" Text="..." ClientIDMode="Static" />
                                <asp:RequiredFieldValidator ID="rfv_AFTER_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_AFTER_CD%>"
                                    ControlToValidate="txt_AFTER_CD" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txt_AFTER_CD" runat="server" size="10" MaxLength="30" Text='<%#Bind("AFTER_CD")%>' ClientIDMode="Static" CssClass="EnNum30 MandatoryField" />
                                <asp:Button runat="server" ID="btn_AFTER_CD" Text="..." ClientIDMode="Static" />
                                <asp:RequiredFieldValidator ID="rfv_AFTER_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_AFTER_CD%>"
                                    ControlToValidate="txt_AFTER_CD" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                </asp:RequiredFieldValidator>
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="150px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lb_AFTER_CD_DESC" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_AFTER_CD_DESC%>"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_AFTER_DESC" runat="server" Text='<%#Bind("AFTER_DESC")%>' ClientIDMode="Static"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_AFTER_DESC" size="32" runat="server" Text='<%#Bind("AFTER_DESC")%>' ClientIDMode="Static" Style="border: none"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txt_AFTER_DESC" size="32" runat="server" Text='<%#Bind("AFTER_DESC")%>' ClientIDMode="Static" Style="border: none"></asp:TextBox>
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="230px" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="GridviewScrollPager" />
                    <FooterStyle CssClass="GridviewScrollPager" />
                    <EmptyDataTemplate>
                        <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                            <tr class="header">
                                <td width="20px"></td>
                                <td width="70px">
                                    <asp:Label ID="lb_RowNumber" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_RowNumber%>"></asp:Label>
                                </td>
                                <td width="150px">
                                    <asp:Label ID="lb_HR_CHG_ITEM" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_HR_CHG_ITEM%>"></asp:Label>
                                </td>
                                <td width="150px">
                                    <asp:Label ID="lb_BEFORE_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_BEFORE_CD%>"></asp:Label>
                                </td>
                                <td width="230px">
                                    <asp:Label ID="lb_BEFORE_CD_DESC" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_BEFORE_CD_DESC%>"></asp:Label>
                                </td>
                                <td width="150px">
                                    <asp:Label ID="lb_AFTER_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_AFTER_CD%>"></asp:Label>
                                </td>
                                <td width="230px">
                                    <asp:Label ID="lb_AFTER_CD_DESC" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_AFTER_CD_DESC%>"></asp:Label>
                                </td>
                            </tr>
                            <tr id="gv_result2_empty_add" class="normal" style="display: none;">
                                <td class="gv_result_empty_td"></td>
                                <td class="gv_result_empty_td"></td>
                                <td class="gv_result_empty_td">
                                    <div style="text-align: left;">
                                        <asp:DropDownList ID="ddl_HR_CHG_ITEM" runat="server" Text='<%#Bind("HR_CHG_ITEM_DESC")%>' ClientIDMode="Static" CssClass="MandatoryField" />
                                        <asp:RequiredFieldValidator ID="rfv_HR_CHG_ITEM" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_HR_CHG_ITEM%>"
                                            ControlToValidate="ddl_HR_CHG_ITEM" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td class="gv_result_empty_td">
                                    <div style="text-align: left;">
                                        <asp:TextBox ID="txt_BEFORE_CD" size="10" runat="server" Text='<%#Bind("BEFORE_CD")%>' ClientIDMode="Static" Style="border: none"></asp:TextBox>
                                    </div>
                                </td>
                                <td class="gv_result_empty_td">
                                    <div style="text-align: left;">
                                        <asp:TextBox ID="txt_BEFORE_DESC" size="32" runat="server" Text='<%#Bind("BEFORE_DESC")%>' ClientIDMode="Static" Style="border: none"></asp:TextBox>
                                    </div>
                                </td>
                                <td class="gv_result_empty_td">
                                    <div style="text-align: left;">
                                        <asp:TextBox ID="txt_AFTER_CD" runat="server" size="10" MaxLength="30" Text='<%#Bind("AFTER_CD")%>' ClientIDMode="Static" CssClass="EnNum30 MandatoryField" />
                                        <asp:Button runat="server" ID="btn_AFTER_CD" Text="..." ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="rfv_AFTER_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_AFTER_CD%>"
                                            ControlToValidate="txt_AFTER_CD" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td class="gv_result_empty_td">
                                    <div style="text-align: left;">
                                        <asp:TextBox ID="txt_AFTER_DESC" size="32" runat="server" Text='<%#Bind("AFTER_DESC")%>' ClientIDMode="Static" Style="border: none"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
            </div>
            <div style="height: 30px"></div>
            <div style="width: 1018px; text-align: right;">
                <aces:Btn ID="WFB2HC0100Save" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2HC0100Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />

                <%--<asp:Button ID="WFB2HC0100Save" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2HC0100Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                <asp:Button ID="WFB2HC0100Cancel" runat="server" Text="<%$Resources:Resource,btn_cancel%>" OnClientClick="return confirm($('#hid_wfb2hc_Cancel_Confirm_Message').val())" OnClick="WFB2HC0100Cancel_Click" />
            </div>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />

            <asp:HiddenField ID="hid_is_super" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_is_dept" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_departments" runat="server" ClientIDMode="Static" />

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="WFB2HC0100Detail_Add" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="WFB2HC0100Detail_Delete" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="WFB2HC0100Detail_Edit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="WFB2HC0100Detail_Save" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="WFB2HC0100Detail_Cancel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="gv_result" EventName="DataBound" />
            <asp:AsyncPostBackTrigger ControlID="gv_result2" EventName="DataBound" />
            <asp:AsyncPostBackTrigger ControlID="txt_EMP_ID" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txt_START_DT" EventName="TextChanged" />
            <%--<asp:AsyncPostBackTrigger ControlID="txt_HR_CHG_CD" EventName="TextChanged" />--%>
            <asp:AsyncPostBackTrigger ControlID="txt_INS_PLAN_PROC_DT" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="cb_IS_END" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddl_MAIN_HR_CHG_NO" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

