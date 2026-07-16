<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hc/WFB2HC0100_Add_batch.aspx.cs" Inherits="WebContent_WFB2HC0100_Add_batch" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <style type="text/css">
      .gv_result_empty_td {
        padding-top: 3px;
        font-size: 14px;
        padding-left: 5px;
        padding-right: 5px; }      
    </style>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();            
        });

        function iniForm() {
            $.unblockUI();
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date2").mask('9999/99');            

            if ($("#txt_AFTER_DESC").length > 0)
                $("#txt_AFTER_DESC").attr("ReadOnly", true);            
                        
            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NAME").attr("ReadOnly", true);
            $("#txt_DEPT_NO").blur(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        cache: false,
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
            
            //職務代號取得職務名稱的ajax
            $("#txt_PJOB_DESC").attr("ReadOnly", true);
            $("#txt_PJOB_CD").blur(function () {
                if ($("#txt_PJOB_CD").val().length == 4) {
                    $.ajax({
                        url: "../commgeo/WFB2GetPjobData.ashx",
                        data: {
                            PJOB_CD: $('#txt_PJOB_CD').val(),
                            START_DT: $('#txt_START_DT').val()
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_PJOB_DESC').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_PJOB_DESC').val(JData.PJOB_DESC);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_PJOB_DESC').val("");
                }
            });
           
            //輪值表代碼取得輪值表名稱的ajax
            $("#txt_WORK_SHIFT_DESC").attr("ReadOnly", true);
            $("#txt_WORK_SHIFT_CD").blur(function () {
                if ($("#txt_WORK_SHIFT_CD").val().length == 2) {
                    $.ajax({
                        url: "../commgeo/WFB2GetWorkShiftData.ashx",
                        data: {
                            WORK_SHIFT_CD: $('#txt_WORK_SHIFT_CD').val()
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_WORK_SHIFT_DESC').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_WORK_SHIFT_DESC').val(JData.WORK_SHIFT_DESC);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_WORK_SHIFT_DESC').val("");
                }
            });

            gridviewScroll2();
           
        }

        function set_START_DT_AND_HR_CHG_CD() {            
            if ($("#txt_START_DT").val() != "" && $("#ddl_HR_CHG_CD").val() != "")
                $("#hid_START_DT_AND_HR_CHG_CD").val($("#txt_START_DT").val() + $("#ddl_HR_CHG_CD").val());
            else
                $("#hid_START_DT_AND_HR_CHG_CD").val("");
        };   

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
                    cache: false,
                    dataType: 'json',
                    async: false,
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
                    cache: false,
                    dataType: 'json',
                    async: false,
                    success: function (JData) {
                        if (JData.errMsg != "" && JData.errMsg != null) {
                            alert(JData.errMsg);
                        } else if (JData.bolSTATUS) {                            
                                                      
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
                cache: false,
                dataType: 'json',
                async: false,
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
                cache: false,
                dataType: 'json',
                async: false,
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

        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());            
        }

        function ShowRecord2(obj) {
            $("#HID_PageRow2").val($("#ddlPerPageRow2").val());
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1000",
                height: "500",
                barcolor: "#7F7F7F"                
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        function gridviewScroll2() {
            $('#<%=gv_result2.ClientID%>').gridviewScroll({
                width: "1000",
                height: "500",
                barcolor: "#7F7F7F",
                freezesize: 8
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        //清空畫面
        function ClearAll() {
            $("#txt_JOIN_SDT").val("");
            $("#txt_JOIN_EDT").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#txt_PJOB_CD").val("");
            $("#txt_PJOB_DESC").val("");
            $("#txt_WORK_SHIFT_CD").val("");
            $("#txt_WORK_SHIFT_DESC").val("");
            $("#txt_BACK_SCHOOL_DT").val("");
            $("#txt_BACK_PLANT_DT").val("");
            $("#txt_BE_CONTRACT_DT").val("");
            $("#txt_BE_DESPATCH_DT").val("");
            $("#txt_KEEP_DESPATCH_DT").val("");
            $("#ddl_COMPANY_CD").val("");
            $("#ddl_PLANT_CD").val("");
            $("#ddl_WORK_CD").val("");
         }
         
        function setDetail_btn(display) {
            if (display) {
                $("#gv_result_empty_add").hide();
            }
            else {
                $("#gv_result_empty_add").show();
            }
        }
                
        //◎若人事異動代碼是click查詢按鍵開啟<<人事異動代碼查詢視窗>>，
        function hr_chg_cd_btn_click() {
            //若 資料權限之「小分類」為Y(管理部主管)，
            //傳入(無)(畫面.人事異動代碼)(無, 'Y', 無, 'Y', 無)，選取其中之一取得人事異動代碼及說明，顯示於畫面上。
            if ($("#hid_SysCodeAtt").val() == "Y") {
                OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_CD_DESC', 'IS_FOR_BATCH=Y&IS_VALID=Y&FUNC_ID=FB2HC010_ADD_BATCH');
                txt_HR_CHG_CD_change();
            }
            //若 資料權限之「小分類」為N(管理部擔當)，
            //傳入(無)(畫面.人事異動代碼)(無, 'Y', 無, 'Y', 登入者帳號)，選取其中之一取得人事異動代碼及說明，顯示於畫面上；
            if ($("#hid_SysCodeAtt").val() == "N") {
                OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_CD_DESC', 'IS_FOR_BATCH=Y&IS_VALID=Y&EMP_ID=' + $("#hid_LOGIN_ID").val() + "&FUNC_ID=FB2HC010_ADD_BATCH");
                txt_HR_CHG_CD_change();
            }
            //若 資料權限之「小分類」為W(各單位擔當)，
            //傳入(無)(畫面.人事異動代碼)('D', 'Y', 無, 'Y', 無)選取其中之一取得人事異動代碼及說明，顯示於畫面上；
            if ($("#hid_SysCodeAtt").val() == "W") {
                OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_CD_DESC', 'UPD_RIGHT_CD=D&IS_FOR_BATCH=Y&IS_VALID=Y&FUNC_ID=FB2HC010_ADD_BATCH');
                txt_HR_CHG_CD_change();
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
                cache: false,
                dataType: 'json',
                async: false,
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
        
        function ddl_HR_CHG_ITEM_change() {
            $("#txt_AFTER_CD").val("");
            $("#txt_AFTER_DESC").val("");
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
                    get_Add_batch_HR_CHG_ITEM_07_AFTER_DESC();
                else if ($("#ddl_HR_CHG_ITEM").val() == "08")
                    get_Add_batch_HR_CHG_ITEM_08_AFTER_DESC();
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
                    EMP_ID: ""
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
        function get_Add_batch_HR_CHG_ITEM_07_AFTER_DESC() {
            $.ajax({
                url: "WFB2HC0100_DataProc.ashx",
                data: {
                    DATA_TYPE: "Get_Add_batch_HR_CHG_ITEM_07_AFTER",
                    GRADE_CD: $('#txt_AFTER_CD').val()                    
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
        function get_Add_batch_HR_CHG_ITEM_08_AFTER_DESC() {
            $.ajax({
                url: "WFB2HC0100_DataProc.ashx",
                data: {
                    DATA_TYPE: "Get_Add_batch_HR_CHG_ITEM_08_AFTER",
                    PJOB_CD: $('#txt_AFTER_CD').val()
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
                OpenSearch('CommCode_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'MAIN_CD=PLANT_CD&SUB_CD=' + $("#txt_AFTER_CD").val());
            } else if ($("#ddl_HR_CHG_ITEM").val() == "03") {
                //(3.3)若CLICK異動後代碼欄的BUTTON，開啟<<共用代碼查詢視窗>>傳入('HB','WS_CD')(明細畫面.異動後代碼)(無)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																
                OpenSearch('CommCode_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'MAIN_CD=WS_CD&SUB_CD=' + $("#txt_AFTER_CD").val());
            } else if ($("#ddl_HR_CHG_ITEM").val() == "04") {
                //(4.3)若CLICK異動後代碼欄的BUTTON，開啟<<共用代碼查詢視窗>>傳入('HB','EMP_CD')(明細畫面.異動後代碼)(無)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																	
                OpenSearch('CommCode_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'MAIN_CD=EMP_CD&SUB_CD=' + $("#txt_AFTER_CD").val());
            } else if ($("#ddl_HR_CHG_ITEM").val() == "05") {
                //(5.3)若CLICK異動後代碼欄的BUTTON，開啟<<部門查詢視窗-清單>>傳入(無)(明細畫面.異動後代碼)(明細畫面.異動生效日)，選取其一之後取得代號及部門名稱，顯示於畫面上。																																																																																							
                OpenSearch('DeptGrid_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'DEPT_NO=' + $("#txt_AFTER_CD").val() + '&START_DT=' + $("#txt_START_DT").val(),"Y");
            } else if ($("#ddl_HR_CHG_ITEM").val() == "06") {
                //(6.3)若CLICK異動後代碼欄的BUTTON，開啟<<資格查詢視窗>>傳入(無)(明細畫面.異動後代碼)(明細畫面.異動生效日)，選取其一之後取得代碼及說明，顯示於畫面上。																																																																																		
                OpenSearch('EmpLevel_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'LEVEL_CD=' + $("#txt_AFTER_CD").val() + '&START_DT=' + $("#txt_START_DT").val());
            } else if ($("#ddl_HR_CHG_ITEM").val() == "07") {
                //(7.3)若CLICK異動後代碼欄的BUTTON，開啟<<資格級數查詢視窗>>傳入(無)(異動項目:'06-資格' 之異動後代碼 或 E.資格代號,明細畫面.異動後代碼)(明細畫面.異動生效日)，																																																																																					
                //選取其一之後取得代碼及說明，顯示於畫面上。
                OpenSearch('EmpGrade_Search.aspx', 'txt_AFTER_CD', 'txt_AFTER_DESC', 'GRADE_CD=' + $("#txt_AFTER_CD").val() + '&IS_VALID=Y&LEVEL_CD=' + $("#hid_LEVEL_CD_AFTER").val());
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
                        <fieldset style="padding:5px; border-color: red;">
                            <legend class="Body_label"><font color="red"  size="+0.5"><asp:Label runat="server" Text="<%$Resources:Resource,wfb2hc_Content_personnel_changes%>"></asp:Label></font></legend>
		                   <div class='pos_top'>
							    <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
								    <colgroup>
								        <col width="14%" />
                                        <col width="14%" />
							            <col width="14%" />
							            <col width="14%" />
							            <col width="14%" />
							            <col width="30%" />
							        </colgroup>
								    <tbody>
                                        <tr>
									        <%--人事異動編號--%>
                               	            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_HR_CHG_NO" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_HR_CHG_NO%>"></asp:Label>:
                               	            </th>
                                	        <td align="left" class="Body_label" >
                                                <asp:TextBox ID="txt_HR_CHG_NO" runat="server" size="10" ClientIDMode="Static" style="border:none"></asp:TextBox>									    
									        </td>
										    <%--異動生效日--%>
									        <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_START_DT1%>"></asp:Label>:
									        </th>
                                	        <td align="left" class="Body_label" >
                                                <asp:TextBox ID="txt_START_DT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" CssClass="MandatoryField date"  OnTextChanged="txt_START_DT_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfv_START_DT" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_START_DT%>"
                                                ControlToValidate="txt_START_DT" ForeColor="Red" Display="None" ValidationGroup="GroupA"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cv_START_DT" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2hc_START_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_START_DT%>"
                                                ControlToValidate="txt_START_DT" ForeColor="Red" Display="None" ValidationGroup="GroupB"></asp:RequiredFieldValidator>--%>
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2hc_START_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_START_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>

                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_START_DT%>"
                                                ControlToValidate="txt_START_DT" ForeColor="Red" Display="None" ValidationGroup="GroupC"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2hc_START_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_START_DT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
									        </td>
                                            <%--人事異動代碼--%>
                               		        <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_HR_CHG_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_HR_CHG_CD%>"></asp:Label>:
                               		        </th>
                                	        <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_HR_CHG_CD" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="txt_HR_CHG_CD_TextChanged" AutoPostBack="true" CssClass="MandatoryField"></asp:DropDownList>
                                                <%-- 
										        <asp:TextBox ID="txt_HR_CHG_CD" runat="server" MaxLength="3" size="5" ClientIDMode="Static" CssClass="MandatoryField" OnTextChanged="txt_HR_CHG_CD_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <input type="button" id="btn_HR_CHG_CD" onclick="hr_chg_cd_btn_click();" value="..." />                                        
                                                <asp:TextBox ID="txt_HR_CHG_CD_DESC" runat="server" size="20" ClientIDMode="Static" style="border:none;"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_HR_CHG_CD%>"
                                                ControlToValidate="txt_HR_CHG_CD" ForeColor="Red" Display="None" ValidationGroup="GroupC"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfv_HR_CHG_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_HR_CHG_CD%>"
                                                ControlToValidate="txt_HR_CHG_CD" ForeColor="Red" Display="None" ValidationGroup="GroupA"></asp:RequiredFieldValidator>
                                                 --%>

                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_HR_CHG_CD%>"
                                                ControlToValidate="txt_HR_CHG_CD" ForeColor="Red" Display="None" ValidationGroup="GroupB"></asp:RequiredFieldValidator>--%>
                                                <asp:RequiredFieldValidator ID="rv_START_DT_AND_HR_CHG_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_required_START_DT_AND_HR_CHG_CD%>"
                                                ControlToValidate="hid_START_DT_AND_HR_CHG_CD" ForeColor="Red" Display="None" ValidationGroup="GroupB"></asp:RequiredFieldValidator>

                                              
									        </td>
									    </tr>
									    <tr>
										    <%--保險預計處理日--%>
									        <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_INS_PLAN_PROC_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_INS_PLAN_PROC_DT%>"></asp:Label>:
									        </th>
                                	        <td align="left" class="Body_label" >
                                                <asp:TextBox ID="txt_INS_PLAN_PROC_DT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" CssClass="date" OnTextChanged="txt_INS_PLAN_PROC_DT_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <asp:CustomValidator ID="cv_INS_PLAN_PROC_DT" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2hc_INS_PLAN_PROC_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_INS_PLAN_PROC_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                                
                                                <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2hc_INS_PLAN_PROC_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_INS_PLAN_PROC_DT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
									        </td>
                                            <%--狀態預計結束日--%>
									        <th align="left" class="Body_TableHeader">                                            
                                                <asp:Label ID="lb_PLAN_END_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_PLAN_END_DT%>"></asp:Label>:
									        </th>
                                	        <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_PLAN_END_DT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" CssClass="date" OnTextChanged="txt_PLAN_END_DT_TextChanged" AutoPostBack="true"></asp:TextBox>											
                                                <asp:CustomValidator ID="cv_PLAN_END_DT" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2hc_PLAN_END_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_PLAN_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                                                <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2hc_PLAN_END_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_PLAN_END_DT" ValidationGroup="GroupC" Display="None"></asp:CustomValidator>
									        </td>
										    <%--狀態結束--%>
									        <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_IS_END" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_IS_END%>"></asp:Label>:
									        </th>
                                	        <td align="left" class="Body_label">
                                                <asp:CheckBox ID="cb_IS_END" runat="server" ClientIDMode="Static" />
									        </td>
									    </tr>
									    <tr>
										    <td align="center" height="1" colspan="6"> <hr> </td>
									    </tr>     
									    <tr>										
                                		    <td align="left" class="Body_label" >
                                                <asp:Label runat="server" Text="<%$Resources:Resource,wfb2hc_Change_Item_Detail%>"></asp:Label>:
                                		    </td>
										    <td align="right" colspan="5">
											    <div id="init_add"  style="display:inline">
                                                    <aces:Btn ID="WFB2HC0101Detail_Add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="WFB2HC0101Detail_Add_Click" />

                                                    <%--<asp:Button ID="WFB2HC0101Detail_Add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="WFB2HC0101Detail_Add_Click" />--%>
											    </div>    
											    <div id="init_grid"   style="display:inline">
												    <aces:Btn ID="WFB2HC0101Detail_Delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClick="WFB2HC0101Detail_Delete_Click" />
                                                    <aces:Btn ID="WFB2HC0101Detail_Edit" runat="server" Text="<%$Resources:Resource,btn_edit%>" OnClick="WFB2HC0101Detail_Edit_Click" />

<%--												    <asp:Button ID="WFB2HC0101Detail_Delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClick="WFB2HC0101Detail_Delete_Click" />
                                                    <asp:Button ID="WFB2HC0101Detail_Edit" runat="server" Text="<%$Resources:Resource,btn_edit%>" OnClick="WFB2HC0101Detail_Edit_Click" />--%>
											    </div>
											    <div id="grid_add">
												    <aces:Btn ID="WFB2HC0101Detail_Save" runat="server" Text="<%$Resources:Resource,wfb2hc_btn_detail_ok%>" OnClick="WFB2HC0101Detail_Save_Click" ValidationGroup="GroupA" />

												    <%--<asp:Button ID="WFB2HC0101Detail_Save" runat="server" Text="<%$Resources:Resource,wfb2hc_btn_detail_ok%>" OnClick="WFB2HC0101Detail_Save_Click" ValidationGroup="GroupA" />--%>
                                                    
                                                    <asp:Button ID="WFB2HC0101Detail_Cancel" runat="server" Text="<%$Resources:Resource,btn_cancel%>" OnClick="WFB2HC0101Detail_Cancel_Click" />
											    </div>
                                                <asp:HiddenField ID="hid_EMP_ID_search" ClientIDMode="Static" runat="server" />
                                                <asp:HiddenField ID="hid_START_SDT_search" ClientIDMode="Static" runat="server" />
                                                <asp:HiddenField ID="hid_START_EDT_search" ClientIDMode="Static" runat="server" />
                                                <asp:HiddenField ID="hid_HR_CHG_CD_search" ClientIDMode="Static" runat="server" />
                                                <asp:HiddenField ID="hid_HR_CHG_PROC_STATUS_search" ClientIDMode="Static" runat="server" />
                                                <asp:TextBox ID="hid_START_DT_AND_HR_CHG_CD" ClientIDMode="Static" runat="server" style="display:none;" />
                                                <asp:HiddenField ID="hid_SysCodeAtt" ClientIDMode="Static" runat="server" />        <%--資料權限之「小分類」--%>
                                                <asp:HiddenField ID="hid_LOGIN_ID" ClientIDMode="Static" runat="server" />          <%--登入者帳號--%>
                                                <asp:HiddenField ID="hid_LEVEL_CD_AFTER" ClientIDMode="Static" runat="server" />    <%--明細06-資格之異動後代碼--%>
										    </td>
									    </tr>					                    
					                </tbody>
							    </table>
							</div> 
							<div id="editPage1">
								<asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                                    AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                                    OnRowDataBound="gv_result_RowDataBound" Width="1000px"
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
                                            <HeaderStyle Width="50px" />                                            
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
                                                        ControlToValidate="ddl_HR_CHG_ITEM" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                                </asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hid_HR_CHG_ITEM" ClientIDMode="Static" runat="server" Value='<%#Bind("HR_CHG_ITEM")%>' />
                                            </EditItemTemplate>
                                            <FooterTemplate >
                                                <asp:DropDownList ID="ddl_HR_CHG_ITEM" runat="server" Text='<%#Bind("HR_CHG_ITEM")%>' ClientIDMode="Static" CssClass="MandatoryField" />
                                                <asp:RequiredFieldValidator ID="rfv_HR_CHG_ITEM" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_HR_CHG_ITEM%>"
                                                        ControlToValidate="ddl_HR_CHG_ITEM" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                                </asp:RequiredFieldValidator>
                                            </FooterTemplate>                            
                                            <ItemStyle HorizontalAlign="Left" />
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle Width="225px" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lb_BEFORE_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_BEFORE_CD%>"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lb_BEFORE_CD" runat="server" Text='<%#Bind("BEFORE_CD")%>' ClientIDMode="Static"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_BEFORE_CD" size="10" runat="server" Text='<%#Bind("BEFORE_CD")%>' ClientIDMode="Static" style="border:none"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt_BEFORE_CD" size="10" runat="server" Text='<%#Bind("BEFORE_CD")%>' ClientIDMode="Static" style="border:none"></asp:TextBox>                              
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
                                                <asp:TextBox ID="txt_BEFORE_DESC" size="32" runat="server" Text='<%#Bind("BEFORE_DESC")%>' ClientIDMode="Static" style="border:none"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt_BEFORE_DESC" size="32" runat="server" Text='<%#Bind("BEFORE_DESC")%>' ClientIDMode="Static" style="border:none"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle Width="230px" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lb_AFTER_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_AFTER_CD%>"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lb_AFTER_CD" runat="server" Text='<%#Bind("AFTER_CD")%>' ClientIDMode="Static"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate >
                                                <asp:TextBox ID="txt_AFTER_CD" runat="server" size="10" MaxLength="30" Text='<%#Bind("AFTER_CD")%>' ClientIDMode="Static" CssClass="MandatoryField" />
                                                <asp:Button runat="server" ID="btn_AFTER_CD" Text="..." ClientIDMode="Static" />
                                                <asp:RequiredFieldValidator ID="rfv_AFTER_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_AFTER_CD%>"
                                                        ControlToValidate="txt_AFTER_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                                </asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt_AFTER_CD" runat="server" size="10" MaxLength="30" Text='<%#Bind("AFTER_CD")%>' ClientIDMode="Static" CssClass="MandatoryField" />
                                                <asp:Button runat="server" ID="btn_AFTER_CD" Text="..." ClientIDMode="Static" />
                                                <asp:RequiredFieldValidator ID="rfv_AFTER_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_AFTER_CD%>"
                                                        ControlToValidate="txt_AFTER_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                                </asp:RequiredFieldValidator>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle Width="225px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lb_AFTER_CD_DESC" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_AFTER_CD_DESC%>"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lb_AFTER_DESC" runat="server" Text='<%#Bind("AFTER_DESC")%>' ClientIDMode="Static"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_AFTER_DESC" size="32" runat="server" Text='<%#Bind("AFTER_DESC")%>' ClientIDMode="Static" style="border:none"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt_AFTER_DESC" size="32" runat="server" Text='<%#Bind("AFTER_DESC")%>' ClientIDMode="Static" style="border:none"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle Width="460px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="GridviewScrollPager" />
                                    <FooterStyle CssClass="GridviewScrollPager" />
                                    <EmptyDataTemplate>
                                        <table class="grid-view" width="997" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                                            <tr class="header">
                                                <td width="20px"></td>
                                                <td width="50px">
                                                    <asp:Label ID="lb_RowNumber" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_RowNumber%>"></asp:Label>
                                                </td>
                                                <td width="225px">
                                                    <asp:Label ID="lb_HR_CHG_ITEM" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_HR_CHG_ITEM%>"></asp:Label>
                                                </td>
                                                <%--<td width="150px">
                                                    <asp:Label ID="lb_BEFORE_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_BEFORE_CD%>"></asp:Label>
                                                </td>
                                                <td width="230px">
                                                    <asp:Label ID="lb_BEFORE_CD_DESC" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_BEFORE_CD_DESC%>"></asp:Label>
                                                </td>--%>
                                                <td width="225px">
                                                    <asp:Label ID="lb_AFTER_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_AFTER_CD%>"></asp:Label>
                                                </td>
                                                <td width="460px">
                                                    <asp:Label ID="lb_AFTER_CD_DESC" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_AFTER_CD_DESC%>"></asp:Label>
                                                </td>                          
                                            </tr>
                                            <tr id="gv_result_empty_add" class="normal" style="display:none;">
                                                <td class="gv_result_empty_td"></td>                            
                                                <td class="gv_result_empty_td"></td>
                                                <td class="gv_result_empty_td">
                                                    <div style="text-align:left;">
                                                        <asp:DropDownList ID="ddl_HR_CHG_ITEM" runat="server" Text='<%#Bind("HR_CHG_ITEM_DESC")%>' ClientIDMode="Static" CssClass="MandatoryField" />
                                                        <asp:RequiredFieldValidator ID="rfv_HR_CHG_ITEM" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_HR_CHG_ITEM%>"
                                                                ControlToValidate="ddl_HR_CHG_ITEM" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </td>
                                                <%--<td class="gv_result_empty_td">
                                                    <div style="text-align:left;">
                                                        <asp:TextBox ID="txt_BEFORE_CD" size="10" runat="server" Text='<%#Bind("BEFORE_CD")%>' ClientIDMode="Static" style="border:none"></asp:TextBox>
                                                    </div>
                                                </td>
                                                <td class="gv_result_empty_td">
                                                    <div style="text-align:left;">
                                                        <asp:TextBox ID="txt_BEFORE_DESC" size="32" runat="server" Text='<%#Bind("BEFORE_DESC")%>' ClientIDMode="Static" style="border:none"></asp:TextBox>
                                                    </div>
                                                </td>--%>
                                                <td class="gv_result_empty_td">
                                                    <div style="text-align:left;">
                                                        <asp:TextBox ID="txt_AFTER_CD" runat="server" size="10" MaxLength="30" Text='<%#Bind("AFTER_CD")%>' ClientIDMode="Static" CssClass="MandatoryField" />                                
                                                        <asp:Button runat="server" ID="btn_AFTER_CD" Text="..." ClientIDMode="Static" />
                                                        <asp:RequiredFieldValidator ID="rfv_AFTER_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hc_Required_AFTER_CD%>"
                                                                ControlToValidate="txt_AFTER_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </td>
                                                <td class="gv_result_empty_td">
                                                    <div style="text-align:left;">
                                                        <asp:TextBox ID="txt_AFTER_DESC" size="32" runat="server" Text='<%#Bind("AFTER_DESC")%>' ClientIDMode="Static" style="border:none"></asp:TextBox>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:GridView>
							</div>
					    </fieldset>
					</td>
				</tr>
			</table>
 		    <table  width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">					
		        <tr valign="top">
			        <td>
			            <fieldset style="padding:5px; border-color: blue;" >
                            <legend class="Body_label"><font color="blue"  size="+0.5"><asp:Label runat="server" Text="<%$Resources:Resource,wfb2hc_Change_Target%>"></asp:Label></font></legend>		
		                        <div class='pos_top'>
					                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
									    <colgroup>
									        <col width="9%" />
									        <col width="31%" />
									        <col width="9%" />
									        <col width="51%" />
								        </colgroup>
								        <tbody>
									        <tr>
                                                <%--入社日--%>
                                		        <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_JOIN_SDT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_JOIN_DT%>"></asp:Label>:
                                		        </th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_JOIN_SDT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" CssClass="date"></asp:TextBox>
<%--                                                    <asp:CompareValidator ID="cv_JOIN_SDT" runat="server" ControlToValidate="txt_JOIN_SDT" ErrorMessage="<%$Resources:Resource,wfb2hc_JOIN_SDT_Format_Error%>" Type="Date" Operator="DataTypeCheck"
                                                        Display="None"  ValidationGroup="GroupB" ForeColor="Red"></asp:CompareValidator>--%>
											        ～	
											        <asp:TextBox ID="txt_JOIN_EDT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" CssClass="date"></asp:TextBox>
<%--                                                    <asp:CompareValidator ID="cv_JOIN_EDT" runat="server" ControlToValidate="txt_JOIN_EDT" ErrorMessage="<%$Resources:Resource,wfb2hc_JOIN_EDT_Format_Error%>" Type="Date" Operator="DataTypeCheck"
                                                        Display="None"  ValidationGroup="GroupB" ForeColor="Red"></asp:CompareValidator>--%>

                                                        <asp:CustomValidator ID="CustomValidator5" runat="server" ValidateEmptyText="true"
                                                            ErrorMessage="<%$Resources:Resource,wfb2hc_JOIN_SDT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                            ControlToValidate="txt_JOIN_SDT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="CustomValidator6" runat="server" ValidateEmptyText="true"
                                                            ErrorMessage="<%$Resources:Resource,wfb2hc_JOIN_EDT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                            ControlToValidate="txt_JOIN_EDT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>

                                                    <asp:CompareValidator ID="cv_JOIN_DT" runat="server" ControlToCompare="txt_JOIN_SDT"
                                                    ControlToValidate="txt_JOIN_EDT" ErrorMessage="<%$Resources:Resource,wfb2hc_JOIN_DT_Compare_Error%>" Type="String" Operator="GreaterThanEqual"
                                                    Display="None"  ValidationGroup="GroupB" ForeColor="Red"></asp:CompareValidator>
										        </td>
                                                <%--部門代號--%>
                                		        <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_DEPT_NO%>"></asp:Label>:</th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" size="5" ClientIDMode="Static" />
                                                    <asp:Button runat="server" ID="btn_DEPT_NO" Text="..." ClientIDMode="Static" OnClientClick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N'); return false;" />
                                                    <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                                </td>
									        </tr>
									        <tr>
                                                <%--職務代號--%>
                                		        <th align="left"  class="Body_TableHeader">
                                                    <asp:Label ID="lb_PJOB_DESC" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_PJOB_CD%>"></asp:Label>:
                                		        </th>
                                		        <td align="left" class="Body_label">
											        <asp:TextBox ID="txt_PJOB_CD" runat="server" maxlength="4" size="5" ClientIDMode="Static"></asp:TextBox>
											        <asp:Button runat="server" ID="btn_PJOB_CD" Text="..." ClientIDMode="Static" OnClientClick="OpenSearch('Pjob_Search.aspx', 'txt_PJOB_CD', 'txt_PJOB_DESC', 'WS_CD=' + $('#hid_WS_CD').val() + '&LEVEL_CD=' +  $('#hid_LEVEL_CD').val() + '&PJOB_CD=' + $('#txt_PJOB_CD').val() + '&START_DT='+ $('#txt_START_DT').val())" />
                                                    <asp:TextBox ID="txt_PJOB_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
										        </td>
                                                <%--輪值表代碼--%>
                                		        <th align="left"  class="Body_TableHeader">
                                                    <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_WORK_SHIFT_CD%>"></asp:Label>:
                                		        </th>
                                		        <td align="left" class="Body_label">
											        <asp:TextBox ID="txt_WORK_SHIFT_CD" runat="server" maxlength="2" size="5" ClientIDMode="Static"></asp:TextBox>
											        <asp:Button runat="server" ID="btn_WORK_SHIFT_CD" Text="..." ClientIDMode="Static" OnClientClick="OpenSearch('WorkShift_Search.aspx', 'txt_WORK_SHIFT_CD', 'txt_WORK_SHIFT_DESC', 'WORK_SHIFT_CD=' + $('#txt_WORK_SHIFT_CD').val())" />
                                                    <asp:TextBox ID="txt_WORK_SHIFT_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
										        </td>
									        </tr>
 								        </tbody>
					                </table>
					                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
									    <colgroup>
									        <col width="9%" />
									        <col width="11%" />
									        <col width="9%" />
									        <col width="11%" />
									        <col width="9%" />
									        <col width="11%" />
									        <col width="9%" />
									        <col width="11%" />
									        <col width="9%" />
									        <col width="11%" />
								        </colgroup>
								        <tbody>
									        <tr>
                                                <%--返校日--%>
                                		        <th align="left"  class="Body_TableHeader">
                                                    <asp:Label ID="lb_BACK_SCHOOL_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_BACK_SCHOOL_DT%>"></asp:Label>:
                                		        </th>
                                		        <td align="left" class="Body_label" >
											        <asp:TextBox ID="txt_BACK_SCHOOL_DT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                                    <asp:CustomValidator ID="cv_BACK_SCHOOL_DT" runat="server" ValidateEmptyText="true"
                                                        ErrorMessage="<%$Resources:Resource,wfb2hc_BACK_SCHOOL_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                        ControlToValidate="txt_BACK_SCHOOL_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
										        </td>
                                                <%--返廠日--%>
                                		        <th align="left"  class="Body_TableHeader">
                                                    <asp:Label ID="lb_BACK_PLANT_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_BACK_PLANT_DT%>"></asp:Label>:
                                		        </th>
                                		        <td align="left" class="Body_label" >
											        <asp:TextBox ID="txt_BACK_PLANT_DT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                                    <asp:CustomValidator ID="cv_BACK_PLANT_DT" runat="server" ValidateEmptyText="true"
                                                        ErrorMessage="<%$Resources:Resource,wfb2hc_BACK_PLANT_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                        ControlToValidate="txt_BACK_PLANT_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
										        </td>
                                                <%--轉期間工日--%>
                                		        <th align="left"  class="Body_TableHeader">
                                                    <asp:Label ID="lb_BE_CONTRACT_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_BE_CONTRACT_DT%>"></asp:Label>:
                                		        </th>
                                		        <td align="left" class="Body_label" >
											        <asp:TextBox ID="txt_BE_CONTRACT_DT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                                    <asp:CustomValidator ID="cv_BE_CONTRACT_DT" runat="server" ValidateEmptyText="true"
                                                        ErrorMessage="<%$Resources:Resource,wfb2hc_BE_CONTRACT_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                        ControlToValidate="txt_BE_CONTRACT_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
										        </td>
                                                <%--轉派日--%>
                                		        <th align="left"  class="Body_TableHeader">
                                                    <asp:Label ID="lb_BE_DESPATCH_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_BE_DESPATCH_DT%>"></asp:Label>:
                                		        </th>
                                		        <td align="left" class="Body_label" >
											        <asp:TextBox ID="txt_BE_DESPATCH_DT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                                    <asp:CustomValidator ID="cv_BE_DESPATCH_DT" runat="server" ValidateEmptyText="true"
                                                        ErrorMessage="<%$Resources:Resource,wfb2hc_BE_DESPATCH_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                        ControlToValidate="txt_BE_DESPATCH_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
										        </td>
                                                <%--續派日--%>
                                		        <th align="left"  class="Body_TableHeader">
                                                    <asp:Label ID="lb_KEEP_DESPATCH_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_KEEP_DESPATCH_DT%>"></asp:Label>:
                                		        </th>
                                		        <td align="left" class="Body_label" >
											        <asp:TextBox ID="txt_KEEP_DESPATCH_DT" runat="server" MaxLength="10" size="8" ClientIDMode="Static" CssClass="date"></asp:TextBox>
											        <asp:CustomValidator ID="cv_KEEP_DESPATCH_DT" runat="server" ValidateEmptyText="true"
                                                        ErrorMessage="<%$Resources:Resource,wfb2hc_KEEP_DESPATCH_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                        ControlToValidate="txt_KEEP_DESPATCH_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
										        </td>										    
									        </tr>																
									        <tr>
                                                <%--聘用單位--%>
										        <th align="left"  class="Body_TableHeader">
                                                    <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_COMPANY_CD%>"></asp:Label>:
										        </th>
										        <td align="left" class="Body_label">
											        <asp:DropDownList ID="ddl_COMPANY_CD" runat="server" ClientIDMode="Static" />	
										        </td>				
                                                <%--工廠區分--%>
										        <th align="left"  class="Body_TableHeader">
                                                    <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_PLANT_CD%>"></asp:Label>:
										        </th>
										        <td align="left" class="Body_label">
											        <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static" />
										        </td>
                                                <%--工數區分--%>
                                		        <th align="left"  class="Body_TableHeader">
                                                    <asp:Label ID="lb_WORK_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_WORK_CD%>"></asp:Label>:
                                		        </th>
										        <td align="left" class="Body_label">
											        <asp:DropDownList ID="ddl_WORK_CD" runat="server" ClientIDMode="Static" />
										        </td>										    
									        </tr>						
									        <tr>
										        <td align="right"  class="Body_label"  colspan="10">
											        <div id="init">
												        <aces:Btn ID="WFB2HC0101Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClientClick="set_START_DT_AND_HR_CHG_CD();CheckValid();" OnClick="WFB2HC0101Search_Click" ValidationGroup="GroupB" />

												        <%--<asp:Button ID="WFB2HC0101Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClientClick="set_START_DT_AND_HR_CHG_CD();CheckValid();" OnClick="WFB2HC0101Search_Click" ValidationGroup="GroupB" />--%>
                                                        
                                                        <asp:Button ID="WFB2HC0101Clear" runat="server" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="ClearAll();return false;"  OnClick="WFB2HC0101Clear_Click" />
                                                        <asp:HiddenField ID="hid_START_DT_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_HR_CHG_CD_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_JOIN_SDT_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_JOIN_EDT_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_DEPT_NO_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_PJOB_CD_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_WORK_SHIFT_CD_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_BACK_SCHOOL_DT_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_BACK_PLANT_DT_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_BE_CONTRACT_DT_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_BE_DESPATCH_DT_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_KEEP_DESPATCH_DT_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_COMPANY_CD_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_PLANT_CD_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_WORK_CD_Add_batch" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hid_wfb2hc_Cancel_Confirm_Message" runat="server" ClientIDMode="Static" />                                                        
											        </div>
 										        </td>			
									        </tr>
									        <tr>
										        <td align="center" height="1" colspan="10"> <hr> </td>
									        </tr>  
									        <tr>	
                                                <%--員工清單--%>									
                                		        <td align="left" class="Body_label" >
                                                    <asp:Label ID="lb_EMP_List" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_EMP_List%>"></asp:Label>:
                                		        </td>
									        </tr>
								        </tbody>
					                </table>
		                        </div>		                        
		                        <div id="editPage2" style="margin-top: 10px">
                                    <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="getData_Add_batch"
                                        SelectCountMethod="getCount_Add_batch" TypeName="CFB2HC0100DAO" EnablePaging="True"
                                        SortParameterName="sortExpression" OnSelecting="obs2_Selecting"
                                        StartRowIndexParameterName="startRowIndex"
                                        OnSelected="ods2_Selected">
                                        <SelectParameters>
                                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            <asp:Parameter Name="maximumRows" Type="Int32" />
                                            <asp:ControlParameter ControlID="hid_START_DT_Add_batch"
                                                Name="start_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                            <asp:ControlParameter ControlID="hid_HR_CHG_CD_Add_batch"
                                                Name="hr_chg_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                            <asp:ControlParameter ControlID="hid_JOIN_SDT_Add_batch"
                                                Name="join_sdt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                            <asp:ControlParameter ControlID="hid_JOIN_EDT_Add_batch"
                                                Name="join_edt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                            <asp:ControlParameter ControlID="hid_DEPT_NO_Add_batch"
                                                Name="dept_no" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                            <asp:ControlParameter ControlID="hid_PJOB_CD_Add_batch"
                                                Name="pjob_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                            <asp:ControlParameter ControlID="hid_WORK_SHIFT_CD_Add_batch"
                                                Name="work_shift_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                            <asp:ControlParameter ControlID="hid_BACK_SCHOOL_DT_Add_batch"
                                                Name="back_school_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                            <asp:ControlParameter ControlID="hid_BACK_PLANT_DT_Add_batch"
                                                Name="back_plant_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                            <asp:ControlParameter ControlID="hid_BE_CONTRACT_DT_Add_batch"
                                                Name="be_contract_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                            <asp:ControlParameter ControlID="hid_BE_DESPATCH_DT_Add_batch"
                                                Name="be_despatch_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                            <asp:ControlParameter ControlID="hid_KEEP_DESPATCH_DT_Add_batch"
                                                Name="keep_despatch_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                            <asp:ControlParameter ControlID="hid_COMPANY_CD_Add_batch"
                                                Name="company_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                            <asp:ControlParameter ControlID="hid_PLANT_CD_Add_batch"
                                                Name="plant_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                            <asp:ControlParameter ControlID="hid_WORK_CD_Add_batch"
                                                Name="work_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>                                    
			                        <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                                    AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting2"
                                    OnRowDataBound="gv_result_RowDataBound2" Width="990px" 
                                    OnRowCreated="gv_result_RowCreated2" OnPageIndexChanging="gv_result_PageIndexChanging2">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="20px">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID='cb_all' runat='server' onclick='javascript:SelectAllCheckboxes(this);' ClientIDMode='Static' />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hc_hd_RowNumber%>" ItemStyle-Width="40px" />                                            
                                            <asp:BoundField DataField="EMP_CHG_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_EMP_CHG_DESC%>" ItemStyle-Width="70px" SortExpression="EMP_CHG_DESC" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="COMPANY_NAME" HeaderText="<%$Resources:Resource,wfb2hc_hd_COMPANY_NAME%>" ItemStyle-Width="70px" SortExpression="COMPANY_NAME" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="PLANT_NAME" HeaderText="<%$Resources:Resource,wfb2hc_hd_PLANT_NAME%>" ItemStyle-Width="70px" SortExpression="PLANT_NAME" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="DEPT_NAME" HeaderText="<%$Resources:Resource,wfb2hc_hd_DEPT_NAME2%>" ItemStyle-Width="230px" SortExpression="DEPT_NAME" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2hc_hd_EMP_ID%>" ItemStyle-Width="70px" SortExpression="EMP_ID" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2hc_hd_EMP_NAME%>" ItemStyle-Width="70px" SortExpression="EMP_NAME" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="JOIN_DT" HeaderText="<%$Resources:Resource,wfb2hc_lb_JOIN_DT%>" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-Width="100px" SortExpression="JOIN_DT" />
                                            <asp:BoundField DataField="PJOB_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_PJOB_DESC2%>" ItemStyle-Width="100px" SortExpression="PJOB_DESC" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="WORK_SHIFT_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_WORK_SHIFT_DESC%>" ItemStyle-Width="100px" SortExpression="WORK_SHIFT_DESC" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="WORK_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_WORK_DESC%>" ItemStyle-Width="100px" SortExpression="WORK_DESC" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="BACK_SCHOOL_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_BACK_SCHOOL_DT%>" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-Width="100px" SortExpression="BACK_SCHOOL_DT" />
                                            <asp:BoundField DataField="BACK_PLANT_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_BACK_PLANT_DT%>" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-Width="100px" SortExpression="BACK_PLANT_DT" />
                                            <asp:BoundField DataField="BE_CONTRACT_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_BE_CONTRACT_DT%>" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-Width="100px" SortExpression="BE_CONTRACT_DT" />
                                            <asp:BoundField DataField="BE_DESPATCH_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_BE_DESPATCH_DT%>" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-Width="100px" SortExpression="BE_DESPATCH_DT" />
                                            <asp:BoundField DataField="KEEP_DESPATCH_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_KEEP_DESPATCH_DT%>" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-Width="100px" SortExpression="KEEP_DESPATCH_DT" />
                                            <asp:BoundField DataField="MAIN_HR_CHG_NO" HeaderText="<%$Resources:Resource,wfb2hc_hd_MAIN_HR_CHG_NO%>" ItemStyle-Width="100px" SortExpression="MAIN_HR_CHG_NO" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <PagerStyle CssClass="GridviewScrollPager" />
                                        <FooterStyle CssClass="GridviewScrollPager" />
                                    </asp:GridView>
                                    <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
		                        </div>                                
                        </fieldset>
                        <div style="height:30px"></div>
                        <div style="width:1018px; text-align: right;">
                            <aces:Btn ID="WFB2HC0101Save" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2HC0101Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupC" />
                            
                            <%--<asp:Button ID="WFB2HC0101Save" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2HC0101Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupC" />--%>
                            
                            <asp:Button ID="WFB2HC0101Cancel" runat="server" Text="<%$Resources:Resource,btn_cancel%>" OnClientClick="return confirm($('#hid_wfb2hc_Cancel_Confirm_Message').val())" OnClick="WFB2HC0101Cancel_Click" />
                        </div>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
                        <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupC" ShowSummary="false" />
                    </td>
                </tr>
            </table>            
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="WFB2HC0101Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="WFB2HC0101Detail_Add" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="WFB2HC0101Detail_Delete" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="WFB2HC0101Detail_Edit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="WFB2HC0101Detail_Save" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="WFB2HC0101Detail_Cancel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="gv_result" EventName="DataBound" />
            <asp:AsyncPostBackTrigger ControlID="gv_result2" EventName="DataBound" />            
            <%--<asp:AsyncPostBackTrigger ControlID="txt_EMP_ID" EventName="TextChanged" />--%>
            <asp:AsyncPostBackTrigger ControlID="txt_START_DT" EventName="TextChanged" />
            <%--<asp:AsyncPostBackTrigger ControlID="txt_HR_CHG_CD" EventName="TextChanged" />--%>            
            <asp:AsyncPostBackTrigger ControlID="txt_INS_PLAN_PROC_DT" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txt_PLAN_END_DT" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

