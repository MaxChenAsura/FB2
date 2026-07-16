<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hc/WFB2HC0100_Detail.aspx.cs" Inherits="WebContent_WFB2HC0100_Detail" Culture="auto" UICulture="auto" %>

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

            //人事異動編號
            $("#txt_HR_CHG_NO").attr("ReadOnly", true);
            //生效處理狀態
            $("#txt_HR_CHG_PROC_STATUS_DESC").attr("ReadOnly", true);
            //員工姓名
            $("#txt_EMP_ID").attr("ReadOnly", true);
            $("#txt_EMP_NAME").attr("ReadOnly", true);
            //異動生效日
            $("#txt_START_DT").attr("ReadOnly", true);
            //人事異動代碼、說明
            //2.在考慮資料安全性下，僅能用查詢視窗開啟，不能自行輸入
            $("#txt_HR_CHG_CD").attr("ReadOnly", true);
            $("#txt_HR_CHG_CD_DESC").attr("ReadOnly", true);
            $("#btn_HR_CHG_CD").attr("disabled", true);
            //保險預計處理日
            $("#txt_INS_PLAN_PROC_DT").attr("ReadOnly", true);
            //狀態預計結束日
            $("#txt_PLAN_END_DT").attr("ReadOnly", true);
            //狀態結束
            $("#cb_IS_END").attr("disabled", true);
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
            //外調資料
            $("#fs_TRANSFER").children().attr("disabled", false);
            $("#ddl_ICT_TYPE").attr("disabled", true);
            $("#ddl_TRANSFER_NATION_CD").attr("disabled", true);
            $("#ddl_TRANSFER_COMPANY_CD").attr("disabled", true);
            $("#txt_TRANSFER_DEPT").attr("ReadOnly", true);
            $("#cb_IS_PAY_SUBSIST").attr("disabled", true);
            //按鈕
            $("#init_add").hide();
            $("#init_grid").hide();
            $("#grid_add").hide();
            $("#divbtn").hide();
            
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
                    cache: false,
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
                            //check_same_data1();
                            //check_same_data2();
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
                            //check_same_data1();
                            //check_same_data2();
                            get_PLAN_END_DT();
                            check_INS_PLAN_PROC_DT();
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
            //get_TRANSFER_COMPANY_CD();            
            chekc_C13();
            hr_chg_cd_input_set();
            //check_same_data1();
            check_INS_PLAN_PROC_DT();
            check_PLAN_END_DT();
            get_PLAN_END_DT();
            //get_IS_END();
            //get_HR_CHG_ITEM_List();
            //ini_gv_result1();
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


        //［判斷 人事異動代碼是否為  C13(契約期滿)］																																																																		
        //目的：當期間工離職時，且異動生效日(離職日) = 預計派遣日 時，建議人事異動代碼設為 C13(契約期滿)，因C13屬於非自願性離職
        //工號、異動生效日及人事異動代碼之後，其三個欄位任一onblur時，進行檢核動作。
        //同時滿足下面3個條件時，顯示錯誤訊息"人事異動代碼建議為「C13 契約期滿」"。
        //1. E.員工區分(hidden) = 2 (期間社員)
        //2.明細畫面.異動生效日 = E.預計派遣日(hidden)
        //3.明細畫面.人事異動代碼 <> C13
        function chekc_C13() {
            if ($("#txt_HR_CHG_CD").val() != "" && $("#txt_HR_CHG_CD").val().length == 3) {
                if ($("#hid_EMP_CD").val() == "2" && $("#txt_START_DT").val() == $("#hid_PLAN_DESPATCH_DT").val() && $("#txt_HR_CHG_CD").val() != "C13") {
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
        //◎若人事異動代碼是click查詢按鍵開啟<<人事異動代碼查詢視窗>>，
        function hr_chg_cd_btn_click() {
            //if ($("#init_add").is(":visible")) {
            if (check_EMP_ID()) {
                //若 資料權限之「小分類」為Y(管理部主管)，                    
                //傳入(無)(畫面.人事異動代碼)(無, 無, E.員工區分, 'Y', 無)，選取其中之一取得人事異動代碼及說明，顯示於畫面上。
                if ($("#hid_SysCodeAtt").val() == "Y") {
                    OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_CD_DESC', 'HR_CHG_CD=' + $("#hid_EMP_CD").val() + '&IS_FOR_BATCH=Y');
                    txt_HR_CHG_CD_change();
                }
                //若 資料權限之「小分類」為N(管理部擔當)，
                //傳入(無)(畫面.人事異動代碼)(無, 無, E.員工區分, 'Y', 登入者帳號)，選取其中之一取得人事異動代碼及說明，顯示於畫面上；
                if ($("#hid_SysCodeAtt").val() == "N") {
                    OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_CD_DESC', 'HR_CHG_CD=' + $("#hid_EMP_CD").val() + '&IS_FOR_BATCH=Y&EMP_ID=' + $("#hid_LOGIN_ID").val());
                    txt_HR_CHG_CD_change();
                }
                //若 資料權限之「小分類」為W(各單位擔當)，
                //傳入(無)(畫面.人事異動代碼)('D', 無, E.員工區分, 'Y', 無)，選取其中之一取得人事異動代碼及說明，顯示於畫面上；
                if ($("#hid_SysCodeAtt").val() == "W") {
                    OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_CD_DESC', 'UPD_RIGHT_CD=D&HR_CHG_CD=' + $("#hid_EMP_CD").val() + '&IS_FOR_BATCH=Y');
                    txt_HR_CHG_CD_change();
                }
            }
            //}
            //else {
            //    alert($("#hid_wfb2hc_HR_CHG_CD_can_not_change_because_detail_is_modify_Message").val());
            //}
        }

        function hr_chg_cd_input_set() {
            //取得人事異動代碼之後，
            //(2.1)若 人事異動代碼，為 'B07'(國內出向支援) 或 'B08'(GCC駐在) 或 'B09'(ICT)，則 ［外調資料］區塊欄位ENABLED 可以輸入，
            if ($("#txt_HR_CHG_CD").val() == 'B07' || $("#txt_HR_CHG_CD").val() == 'B08' || $("#txt_HR_CHG_CD").val() == 'B09') {
                $("#fs_TRANSFER").children().attr("disabled", false);
                //     其中，為'B09'時，才可以輸入 ICT類別欄位，否則DISABLED，下拉選單內容參考Sheet<Item Desc>的說明。
                if ($("#txt_HR_CHG_CD").val() == 'B09') {
                    $("#ddl_ICT_TYPE").prop("disabled", false);
                } else {
                    $("#ddl_ICT_TYPE").prop("disabled", true);
                    $("#ddl_ICT_TYPE").val("");
                }
                //<受入國家>
                //  1.若 明細畫面.人事異動代碼為'B07'(國內出向支援)，則 此欄位預設為'TWN-台灣'，且DISABLED不可修改。
                if ($("#txt_HR_CHG_CD").val() == 'B07') {
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
                    HR_CHG_CD: $('#txt_HR_CHG_CD').val()
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
                        if (!confirm(JData.errMsg)) {
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
        //<保險預計處理日>
        //1.若 G.保險提前生效(IS_INS_EARLIER)為'Y'，則 明細畫面.保險預計處理日 必須< 明細畫面.異動生效日，且必須>系統日，
        //  否則顯示錯誤訊息"保險預計處理日 必須＜異動生效日 且必須＞系統日"；
        //  若 G.保險提前生效(IS_INS_EARLIER)為'N'，則 明細畫面.保險預計處理日=明細畫面.異動生效日，且不可修改，將其DISABLED。
        function check_INS_PLAN_PROC_DT() {
            $.ajax({
                url: "WFB2HC0100_DataProc.ashx",
                data: {
                    DATA_TYPE: "CHECK_INS_PLAN_PROC_DT",
                    HR_CHG_CD: $('#txt_HR_CHG_CD').val(),
                    INS_PLAN_PROC_DT: $('#txt_INS_PLAN_PROC_DT').val(),
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
                        if (JData.strIS_INS_EARLIER == "N") {
                            //若 G.保險提前生效(IS_INS_EARLIER)為'N'，則 明細畫面.保險預計處理日=明細畫面.異動生效日，且不可修改，將其DISABLED。
                            $("#txt_INS_PLAN_PROC_DT").prop("disabled", true);
                            $("#txt_INS_PLAN_PROC_DT").val($("#txt_START_DT").val());
                        } else {
                            $("#txt_INS_PLAN_PROC_DT").prop("disabled", false);
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
        //1.若 G.是否暫時狀態(IS_TEMP)為'Y'，
        //  若 G.人事異動身份狀態(EMP_CHG_STATUS) 為'31'(期間工)，
        //  讀取 參數檔 A
        //  取得:參數值(CODE_VAL1)
        //  條件:子作業='HB' 且參數別='KZ_CONTRACT_MONTHS'
        //  預設 明細畫面.狀態預計結束日 = 明細畫面.異動生效日 的月份 + 參數值(CODE_VAL1) ，取該月月初日，再減1天，可修改，不可清空。

        //  若 G.人事異動身份狀態(EMP_CHG_STATUS) 為'32'(派遣)，
        //  讀取 參數檔 A
        //  取得:參數值(CODE_VAL1)
        //  條件:子作業='HB' 且參數別='OTH1_CONTRACT_MONTHS'
        //  預設 明細畫面.狀態預計結束日 = 明細畫面.異動生效日 的月份 + 參數值(CODE_VAL1) ，取該月月初日，再減1天，可修改，不可清空。        
        //  若 G.是否暫時狀態(IS_TEMP)為'Y'，則 明細畫面.狀態預計結束日必須輸入，且必須>明細畫面.異動生效日，否則顯示錯誤訊息"狀態預計結束日必須輸入，且必須＞異動生效日"；
        //  否則，明細畫面.狀態預計結束日不可輸入，將其DISABLED。
        function get_PLAN_END_DT() {
            $.ajax({
                url: "WFB2HC0100_DataProc.ashx",
                data: {
                    DATA_TYPE: "GET_PLAN_END_DT",
                    HR_CHG_CD: $('#txt_HR_CHG_CD').val(),
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
                        if (JData.strIS_TEMP == "Y") {
                            //取得狀態預計結束日
                            $("#txt_PLAN_END_DT").prop("disabled", false);
                            $("#txt_PLAN_END_DT").val(JData.strPLAN_END_DT);
                            $("#hid_IS_TEMP").val(JData.strIS_TEMP);
                        } else {
                            //  否則，明細畫面.狀態預計結束日不可輸入，將其DISABLED。
                            $("#txt_PLAN_END_DT").prop("disabled", true);
                            $("#txt_PLAN_END_DT").val("");
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
                    HR_CHG_CD: $('#txt_HR_CHG_CD').val(),
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
                OpenSearch('CommCode_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'SYS_CD=HB&MAIN_CD=PLANT_CD&SUB_CD=' + $("#txt_AFTER_CD").val());
            } else if ($("#ddl_HR_CHG_ITEM").val() == "03") {
                //(3.3)若CLICK異動後代碼欄的BUTTON，開啟<<共用代碼查詢視窗>>傳入('HB','WS_CD')(明細畫面.異動後代碼)(無)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																
                OpenSearch('CommCode_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'SYS_CD=HB&MAIN_CD=WS_CD&SUB_CD=' + $("#txt_AFTER_CD").val());
            } else if ($("#ddl_HR_CHG_ITEM").val() == "04") {
                //(4.3)若CLICK異動後代碼欄的BUTTON，開啟<<共用代碼查詢視窗>>傳入('HB','EMP_CD')(明細畫面.異動後代碼)(無)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																	
                OpenSearch('CommCode_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'SYS_CD=HB&MAIN_CD=EMP_CD&SUB_CD=' + $("#txt_AFTER_CD").val());
            } else if ($("#ddl_HR_CHG_ITEM").val() == "05") {
                //(5.3)若CLICK異動後代碼欄的BUTTON，開啟<<部門查詢視窗-清單>>傳入(無)(明細畫面.異動後代碼)(明細畫面.異動生效日)，選取其一之後取得代號及部門名稱，顯示於畫面上。																																																																																							
                OpenSearch('DeptGrid_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'DEPT_NO=' + $("#txt_AFTER_CD").val() + '&START_DT=' + $("#txt_START_DT").val());
            } else if ($("#ddl_HR_CHG_ITEM").val() == "06") {
                //(6.3)若CLICK異動後代碼欄的BUTTON，開啟<<資格查詢視窗>>傳入(無)(明細畫面.異動後代碼)(明細畫面.異動生效日)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																		
                //OpenSearch('Pjob_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'LEVEL_CD=' + $("#txt_AFTER_CD").val() + '&START_DT=' + $("#txt_START_DT").val());
            } else if ($("#ddl_HR_CHG_ITEM").val() == "07") {
                //(7.3)若CLICK異動後代碼欄的BUTTON，開啟<<資格級數查詢視窗>>傳入(無)(異動項目:'06-資格' 之異動後代碼 或 E.資格代號,明細畫面.異動後代碼)(明細畫面.異動生效日)，																																																																																					
                //選取其一之後取得代碼及說明，顯示於畫面上。
                //OpenSearch('Level_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'INS_TYPE=A&INS=20000');
                //$("#hid_LEVEL_CD_AFTER").val(), $("#hid_LEVEL_CD").val()
            } else if ($("#ddl_HR_CHG_ITEM").val() == "08") {
                //(8.3)若CLICK異動後代碼欄的BUTTON，開啟<<職務查詢視窗>>傳入(無)(E.職種,E.資格代號,明細畫面.異動後代碼)(明細畫面.異動生效日)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																												
                //(8.4)若G.是否專業職務為'Y'，則所選取的異動後代碼，必須是P開頭的職務代號，否則顯示錯誤訊息"職務代號必須是專業職的代號"																																																																																												
                OpenSearch('Pjob_Search.aspx', 'txt_PJOB_CD', 'txt_PJOB_DESC', 'WS_CD=' + $("#hid_WS_CD").val() + '&LEVEL_CD=' + $("#hid_LEVEL_CD").val() + '&PJOB_CD=' + $("#txt_AFTER_CD").val() + '&START_DT=' + $("#txt_START_DT").val());
            } else if ($("#ddl_HR_CHG_ITEM").val() == "09") {
                //(9.3)若CLICK異動後代碼欄的BUTTON，開啟<<輪值表查詢視窗>>傳入(無)(明細畫面.異動後代碼)(資料權限之「小分類」)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																				
                OpenSearch('WorkShift_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'WORK_SHIFT_CD=' + $("#txt_AFTER_CD").val() + '&WORKER_WORK_SHIFT=' + $("#hid_SysCodeAtt").val());
            } else if ($("#ddl_HR_CHG_ITEM").val() == "10") {
                //(10.3)若CLICK異動後代碼欄的BUTTON，開啟<<共用代碼查詢視窗>>傳入('HB','WORK_CD')(明細畫面.異動後代碼)(無)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																			
                OpenSearch('CommCode_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'SYS_CD=HB&MAIN_CD=WORK_CD&SUB_CD=' + $("#txt_AFTER_CD").val());
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
                                <col width="12%" />
                                <col width="15%" />
                                <col width="12%" />
                                <col width="15%" />
                                <col width="12%" />
                                <col width="11%" />
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
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" size="5" ClientIDMode="Static"  Style="border: none;" ></asp:TextBox>
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" size="5" ClientIDMode="Static" Style="border: none"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_EMP_ID" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_EMP_ID%>"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" Display="None" ValidationGroup="GroupA"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_EMP_ID%>"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" Display="None" ValidationGroup="GroupB"></asp:RequiredFieldValidator>

                                    </td>
                                    <%--異動生效日--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_START_DT1%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_START_DT" runat="server" MaxLength="10" size="8" ClientIDMode="Static"  Style="border: none;" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_START_DT" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_START_DT%>"
                                            ControlToValidate="txt_START_DT" ForeColor="Red" Display="None" ValidationGroup="GroupA"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cv_START_DT" runat="server" ControlToValidate="txt_START_DT" ErrorMessage="<%$Resources:Resource,wfb2hc_START_DT_Format_Error%>" Type="Date" Operator="DataTypeCheck"
                                            Display="None" ValidationGroup="GroupA" ForeColor="Red"></asp:CompareValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_START_DT%>"
                                            ControlToValidate="txt_START_DT" ForeColor="Red" Display="None" ValidationGroup="GroupB"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txt_START_DT" ErrorMessage="<%$Resources:Resource,wfb2hc_START_DT_Format_Error%>" Type="Date" Operator="DataTypeCheck"
                                            Display="None" ValidationGroup="GroupB" ForeColor="Red"></asp:CompareValidator>
                                    </td>
                                    <%--人事異動代碼--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HR_CHG_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_HR_CHG_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_HR_CHG_CD" runat="server" MaxLength="3" size="5" ClientIDMode="Static" Style="border: none;" ></asp:TextBox>
                                        <%-- 
                                        <input type="button" id="btn_HR_CHG_CD" onclick="hr_chg_cd_btn_click();" value="..." />                                        
                                        --%>
                                        <asp:TextBox ID="txt_HR_CHG_CD_DESC" runat="server" size="20" ClientIDMode="Static" Style="border: none"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_HR_CHG_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_HR_CHG_CD%>"
                                            ControlToValidate="txt_HR_CHG_CD" ForeColor="Red" Display="None" ValidationGroup="GroupA"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_HR_CHG_CD%>"
                                            ControlToValidate="txt_HR_CHG_CD" ForeColor="Red" Display="None" ValidationGroup="GroupB"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--保險預計處理日--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_INS_PLAN_PROC_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_INS_PLAN_PROC_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_INS_PLAN_PROC_DT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" Style="border: none;"></asp:TextBox>
                                        <asp:CompareValidator ID="cv_INS_PLAN_PROC_DT" runat="server" ControlToValidate="txt_INS_PLAN_PROC_DT" ErrorMessage="<%$Resources:Resource,wfb2hc_INS_PLAN_PROC_DT_Format_Error%>" Type="Date" Operator="DataTypeCheck"
                                            Display="None" ValidationGroup="GroupA" ForeColor="Red"></asp:CompareValidator>
                                    </td>
                                    <%--狀態預計結束日--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PLAN_END_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_PLAN_END_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:TextBox ID="txt_PLAN_END_DT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" Style="border: none;"></asp:TextBox>
                                        <asp:CompareValidator ID="cv_PLAN_END_DT" runat="server" ControlToValidate="txt_PLAN_END_DT" ErrorMessage="<%$Resources:Resource,wfb2hc_PLAN_END_DT_Format_Error%>" Type="Date" Operator="DataTypeCheck"
                                            Display="None" ValidationGroup="GroupA" ForeColor="Red"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--狀態結束--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_END" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_IS_END%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:CheckBox ID="cb_IS_END" runat="server" ClientIDMode="Static"  />
                                    </td>
                                    <%--異動主編號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MAIN_HR_CHG_NO" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_MAIN_HR_CHG_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:DropDownList ID="ddl_MAIN_HR_CHG_NO" runat="server" ClientIDMode="Static" Style="display: none" ></asp:DropDownList>
                                        <asp:Label ID="txt_MAIN_HR_CHG_NO" runat="server" ClientIDMode="Static"></asp:Label>&nbsp;
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
                                    <td align="right" class="Body_label" colspan="8">
                                        <asp:Button ID="WFB2HC0100_BackPage" runat="server" Text="<%$Resources:Resource,wfb2hc_btn_backPage%>" OnClick="WFB2HC0100_BackPage_Click" />
                                    </td>
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
                                                        <asp:TextBox ID="txt_TRANSFER_DEPT" runat="server" MaxLength="30" size="30" ClientIDMode="Static" Style="border: none;"></asp:TextBox>
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
                        <%--<asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID='cb_all' runat='server' onclick='javascript:SelectAllCheckboxes(this);' ClientIDMode='Static' />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />                                
                            </ItemTemplate>
                            <HeaderStyle Width="20px" />
                        </asp:TemplateField> --%>
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
                        <%--<asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID='cb_all' runat='server' onclick='javascript:SelectAllCheckboxes(this);' ClientIDMode='Static' />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />                                
                            </ItemTemplate>
                            <HeaderStyle Width="20px" />
                        </asp:TemplateField>--%>
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
                                <asp:TextBox ID="txt_AFTER_CD" runat="server" size="10" MaxLength="30" Text='<%#Bind("AFTER_CD")%>' ClientIDMode="Static" CssClass="MandatoryField" />
                                <asp:Button runat="server" ID="btn_AFTER_CD" Text="..." ClientIDMode="Static" />
                                <asp:RequiredFieldValidator ID="rfv_AFTER_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_AFTER_CD%>"
                                    ControlToValidate="txt_AFTER_CD" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txt_AFTER_CD" runat="server" size="10" MaxLength="30" Text='<%#Bind("AFTER_CD")%>' ClientIDMode="Static" CssClass="MandatoryField" />
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
                                        <asp:TextBox ID="txt_AFTER_CD" runat="server" size="10" MaxLength="30" Text='<%#Bind("AFTER_CD")%>' ClientIDMode="Static" CssClass="MandatoryField" />
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
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gv_result" EventName="DataBound" />
            <asp:AsyncPostBackTrigger ControlID="gv_result2" EventName="DataBound" />
            <asp:AsyncPostBackTrigger ControlID="txt_EMP_ID" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txt_START_DT" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txt_HR_CHG_CD" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="cb_IS_END" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddl_MAIN_HR_CHG_NO" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

