<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2da/WFB2DA0200_Add.aspx.cs" Inherits="WebContent_fb2da_WFB2DA0200_Add" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Src="~/UserControl/UCCommCodeDropDwonList.ascx" TagPrefix="uc1" TagName="UCCommCodeDropDwonList" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function CheckSHIFT_CD(source, arguments) {
            var re = /^[0-9a-zA-Z_]+$/;
            if (!re.test($("#txtSHIFT_CD").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        function iniForm() {
            GetHours();
           // $.unblockUI();
        }
        function Valid() {
            //BlockUI();
            Page_IsValid = Page_ClientValidate('GroupA');;
            if (Page_IsValid) {
                if (WorkHoursValid()) {
                    if (confirm($("#hid_Save_Confirm").val())) {
                        BlockUI();
                        return true;
                    } else {
                        $.unblockUI();
                        return false;
                    }
                }
                else {
                    if (confirm("工作時數不為8，" + $("#hid_Save_Confirm").val())) {
                        BlockUI();
                        return true;
                    } else {
                        $.unblockUI();
                        return false;
                    }
                }
            }
        }

        function Already_Confim() {
            
            if ($("#hid_SHIFT_Already").val() != '') {
                if (confirm($("#hid_SHIFT_Already").val())) {
                    if ($('#hid_EMP_DAY_DUTY_Already').val() != "") {
                        if (confirm($("#hid_EMP_DAY_DUTY_Already").val())) {
                            $("#WFB2DA0200SaveIs_AlreadyConfirmAfter").click();
                            BlockUI();
                        }
                    }
                    else {
                        $("#WFB2DA0200SaveIs_AlreadyConfirmAfter").click();
                        BlockUI();
                    }
                }
            }
            else {
                $("#WFB2DA0200SaveIs_AlreadyConfirmAfter").click();
                BlockUI();
            }
        }

        function EMP_DAY_DUTY_Already_ConfimAfter(message) {
            BlockUI();
            if (confirm(message)) {
                $('#WFB2DA0200SaveIsEMP_DAY_DUTY_Already').click();
            }
        }

        //勤前休息、勤前用餐、勤中用餐、勤後用餐、休息時段(一)、休息時段(二)、休息時段(三)、勤後休息(一)及勤後休息(二)不可重疊
        function MealTimeAndResetTimeCrossValid(source, arguments) {
            var isCross = false;
            var TimeControls = [["ddlMealTime1Reset_HH_S", "ddlMealTime1Reset_MM_S", "ddlMealTime1Reset_HH_E", "ddlMealTime1Reset_MM_E"],//勤前休息
                                ["ddlMealTime1_HH_S", "ddlMealTime1_MM_S", "ddlMealTime1_HH_E", "ddlMealTime1_MM_E"],//勤前用餐
                                ["ddlMealTime2_HH_S", "ddlMealTime2_MM_S", "ddlMealTime2_HH_E", "ddlMealTime2_MM_E"],//勤中用餐
                                ["ddlMealTime3_HH_S", "ddlMealTime3_MM_S", "ddlMealTime3_HH_E", "ddlMealTime3_MM_E"],//勤後用餐
                                ["ddlMealTime2Reset1_HH_S", "ddlMealTime2Reset1_MM_S", "ddlMealTime2Reset1_HH_E", "ddlMealTime2Reset1_MM_E"],//休息時段(一)
                                ["ddlMealTime2Reset2_HH_S", "ddlMealTime2Reset2_MM_S", "ddlMealTime2Reset2_HH_E", "ddlMealTime2Reset2_MM_E"],//休息時段(二)
                                ["ddlMealTime2Reset3_HH_S", "ddlMealTime2Reset3_MM_S", "ddlMealTime2Reset3_HH_E", "ddlMealTime2Reset3_MM_E"],//休息時段(三)
                                ["ddlMealTime3Reset1_HH_S", "ddlMealTime3Reset1_MM_S", "ddlMealTime3Reset1_HH_E", "ddlMealTime3Reset1_MM_E"],//勤後休息(一)                              
                                ["ddlMealTime3Reset2_HH_S", "ddlMealTime3Reset2_MM_S", "ddlMealTime3Reset2_HH_E", "ddlMealTime3Reset2_MM_E"]];//勤後休息(二)

            for (var i = 0; i < TimeControls.length; i++) {
                for (var j = i + 1; j < TimeControls.length; j++) {
                    isCross |= CompTimeOnOtherTIME(TimeControls[i][0],
                                                   TimeControls[i][1],
                                                   TimeControls[i][2],
                                                   TimeControls[i][3],
                                                   TimeControls[j][0],
                                                   TimeControls[j][1],
                                                   TimeControls[j][2],
                                                   TimeControls[j][3], true)
                }
            }
            arguments.IsValid = !isCross;
        }

        //勤前用餐,須小於等於上班時段
        function ddlMealTime1LessDUTY_TIMEValid(source, arguments) {
            var WorkStart = getTimeMMs(0, "ddlDUTY_TIME_HH_S", "ddlDUTY_TIME_MM_S", true);
            var WorkEnd = getTimeMMs(0, "ddlDUTY_TIME_HH_E", "ddlDUTY_TIME_MM_E", false);
            var CmpStart = getTimeMMs(0, "ddlMealTime1_HH_S", "ddlMealTime1_MM_S", true);
            var CmpEnd = getTimeMMs(0, "ddlMealTime1_HH_E", "ddlMealTime1_MM_E", false);
            if (WorkStart == -1 || WorkEnd == -1 || CmpStart == -1 || CmpEnd == -1)
                arguments.IsValid = true;
            else {
                if (WorkStart > CmpStart && WorkStart >= CmpEnd && WorkEnd > CmpStart && WorkEnd > CmpEnd)
                    arguments.IsValid = true;
                else
                    arguments.IsValid = false;
            }
        }
        //勤後用餐,須大於等於上班時段
        function ddlMealTime3GreatDUTY_TIMEValid(source, arguments) {
            var WorkStart = getTimeMMs(0, "ddlDUTY_TIME_HH_S", "ddlDUTY_TIME_MM_S", true);
            var WorkEnd = getTimeMMs(0, "ddlDUTY_TIME_HH_E", "ddlDUTY_TIME_MM_E", false);
            var CmpStart = getTimeMMs(0, "ddlMealTime3_HH_S", "ddlMealTime3_MM_S", true);
            var CmpEnd = getTimeMMs(0, "ddlMealTime3_HH_E", "ddlMealTime3_MM_E", false);
            if (WorkStart == -1 || WorkEnd == -1 || CmpStart == -1 || CmpEnd == -1)
                arguments.IsValid = true;
            else {
                if (WorkStart < CmpStart && WorkStart < CmpEnd && WorkEnd <= CmpStart && WorkEnd < CmpEnd)
                    arguments.IsValid = true;
                else
                    arguments.IsValid = false;
            }
        }

        //勤後休息(一),須大於等於上班時段
        function ddlMealTime3Reset1GreatDUTY_TIMEValid(source, arguments) {
            var WorkStart = getTimeMMs(0, "ddlDUTY_TIME_HH_S", "ddlDUTY_TIME_MM_S", true);
            var WorkEnd = getTimeMMs(0, "ddlDUTY_TIME_HH_E", "ddlDUTY_TIME_MM_E", false);
            var CmpStart = getTimeMMs(0, "ddlMealTime3Reset1_HH_S", "ddlMealTime3Reset1_MM_S", true);
            var CmpEnd = getTimeMMs(0, "ddlMealTime3Reset1_HH_E", "ddlMealTime3Reset1_MM_E", false);
            if (WorkStart == -1 || WorkEnd == -1 || CmpStart == -1 || CmpEnd == -1)
                arguments.IsValid = true;
            else {
                if (WorkStart < CmpStart && WorkStart < CmpEnd && WorkEnd <= CmpStart && WorkEnd < CmpEnd)
                    arguments.IsValid = true;
                else
                    arguments.IsValid = false;
            }
        }

        //勤後休息(二),須大於等於上班時段
        function ddlMealTime3Reset2GreatDUTY_TIMEValid(source, arguments) {
            var WorkStart = getTimeMMs(0, "ddlDUTY_TIME_HH_S", "ddlDUTY_TIME_MM_S", true);
            var WorkEnd = getTimeMMs(0, "ddlDUTY_TIME_HH_E", "ddlDUTY_TIME_MM_E", false);
            var CmpStart = getTimeMMs(0, "ddlMealTime3Reset2_HH_S", "ddlMealTime3Reset2_MM_S", true);
            var CmpEnd = getTimeMMs(0, "ddlMealTime3Reset2_HH_E", "ddlMealTime3Reset2_MM_E", false);
            if (WorkStart == -1 || WorkEnd == -1 || CmpStart == -1 || CmpEnd == -1)
                arguments.IsValid = true;
            else {
                if (WorkStart < CmpStart && WorkStart < CmpEnd && WorkEnd <= CmpStart && WorkEnd < CmpEnd)
                    arguments.IsValid = true;
                else
                    arguments.IsValid = false;
            }
        }

        //勤後用餐,須大於等於上班時段
        function ddlMealTime3GreatDUTY_TIMEValid(source, arguments) {
            var WorkStart = getTimeMMs(0, "ddlDUTY_TIME_HH_S", "ddlDUTY_TIME_MM_S", true);
            var WorkEnd = getTimeMMs(0, "ddlDUTY_TIME_HH_E", "ddlDUTY_TIME_MM_E", false);
            var CmpStart = getTimeMMs(0, "ddlMealTime3_HH_S", "ddlMealTime3_MM_S", true);
            var CmpEnd = getTimeMMs(0, "ddlMealTime3_HH_E", "ddlMealTime3_MM_E", false);
            if (WorkStart == -1 || WorkEnd == -1 || CmpStart == -1 || CmpEnd == -1)
                arguments.IsValid = true;
            else {
                if (WorkStart < CmpStart && WorkStart < CmpEnd && WorkEnd <= CmpStart && WorkEnd < CmpEnd)
                    arguments.IsValid = true;
                else
                    arguments.IsValid = false;
            }
        }

        //勤前休息,須小於等於上班時段
        function MealTime1ResetLessDUTY_TIMEValid(source, arguments) {
            var WorkStart = getTimeMMs(0, "ddlDUTY_TIME_HH_S", "ddlDUTY_TIME_MM_S", true);
            var WorkEnd = getTimeMMs(0, "ddlDUTY_TIME_HH_E", "ddlDUTY_TIME_MM_E", false);
            var CmpStart = getTimeMMs(0, "ddlMealTime1Reset_HH_S", "ddlMealTime1Reset_MM_S", true);
            var CmpEnd = getTimeMMs(0, "ddlMealTime1Reset_HH_E", "ddlMealTime1Reset_MM_E", false);
            if (WorkStart == -1 || WorkEnd == -1 || CmpStart == -1 || CmpEnd == -1)
                arguments.IsValid = true;
            else {
                if (WorkStart > CmpStart && WorkStart >= CmpEnd && WorkEnd > CmpStart && WorkEnd > CmpEnd)
                    arguments.IsValid = true;
                else
                    arguments.IsValid = false;
            }
        }

        //勤後用餐、勤後休息(一)及勤後休息(二),大於上班時段
        function ddlMealTime2OnDUTY_TIMEValid(source, arguments) {
            if ($('#ddlDUTY_TIME_HH_S').val() <= $('#ddlDUTY_TIME_HH_E').val()) {
                arguments.IsValid = CompTimeOnOtherTIME("ddlDUTY_TIME_HH_S",
                                                        "ddlDUTY_TIME_MM_S",
                                                        "ddlDUTY_TIME_HH_E",
                                                        "ddlDUTY_TIME_MM_E",
                                                        "ddlMealTime2_HH_S",
                                                        "ddlMealTime2_MM_S",
                                                        "ddlMealTime2_HH_E",
                                                        "ddlMealTime2_MM_E", false);
            } else {
                arguments.IsValid = CompTimeOnOtherTIMEToNextDay("ddlDUTY_TIME_HH_S",
                                        "ddlDUTY_TIME_MM_S",
                                        "ddlDUTY_TIME_HH_E",
                                        "ddlDUTY_TIME_MM_E",
                                        "ddlMealTime2_HH_S",
                                        "ddlMealTime2_MM_S",
                                        "ddlMealTime2_HH_E",
                                        "ddlMealTime2_MM_E", false);
            }
        }

        function ddlMealTime2Reset1OnDUTY_TIMEValid(source, arguments) {
            if ($('#ddlDUTY_TIME_HH_S').val() <= $('#ddlDUTY_TIME_HH_E').val()) {
                arguments.IsValid = CompTimeOnOtherTIME("ddlDUTY_TIME_HH_S",
                                                        "ddlDUTY_TIME_MM_S",
                                                        "ddlDUTY_TIME_HH_E",
                                                        "ddlDUTY_TIME_MM_E",
                                                        "ddlMealTime2Reset1_HH_S",
                                                        "ddlMealTime2Reset1_MM_S",
                                                        "ddlMealTime2Reset1_HH_E",
                                                        "ddlMealTime2Reset1_MM_E", false);
            } else {
                arguments.IsValid = CompTimeOnOtherTIMEToNextDay("ddlDUTY_TIME_HH_S",
                                        "ddlDUTY_TIME_MM_S",
                                        "ddlDUTY_TIME_HH_E",
                                        "ddlDUTY_TIME_MM_E",
                                        "ddlMealTime2Reset1_HH_S",
                                        "ddlMealTime2Reset1_MM_S",
                                        "ddlMealTime2Reset1_HH_E",
                                        "ddlMealTime2Reset1_MM_E", false);
            }
        }

        function WorkHoursValid(source, arguments) {
            if ($("#txt_WorkHour").val() != "") {
                var arrWorkTime = $("#txt_WorkHour").val().split(':');
                var WorkTimeByMin = parseInt(arrWorkTime[0], 10) * 60 + parseInt(arrWorkTime[1], 10);
                if (WorkTimeByMin != 8 * 60)
                    return false;
                else
                    return true;

            }
        }

        function ddlMealTime2Reset2OnDUTY_TIMEValid(source, arguments) {
            if ($('#ddlDUTY_TIME_HH_S').val() <= $('#ddlDUTY_TIME_HH_E').val()) {
                arguments.IsValid = CompTimeOnOtherTIME("ddlDUTY_TIME_HH_S",
                                                    "ddlDUTY_TIME_MM_S",
                                                    "ddlDUTY_TIME_HH_E",
                                                    "ddlDUTY_TIME_MM_E",
                                                    "ddlMealTime2Reset2_HH_S",
                                                    "ddlMealTime2Reset2_MM_S",
                                                    "ddlMealTime2Reset2_HH_E",
                                                    "ddlMealTime2Reset2_MM_E", false);
            } else {
                arguments.IsValid = CompTimeOnOtherTIMEToNextDay("ddlDUTY_TIME_HH_S",
                                                    "ddlDUTY_TIME_MM_S",
                                                    "ddlDUTY_TIME_HH_E",
                                                    "ddlDUTY_TIME_MM_E",
                                                    "ddlMealTime2Reset2_HH_S",
                                                    "ddlMealTime2Reset2_MM_S",
                                                    "ddlMealTime2Reset2_HH_E",
                                                    "ddlMealTime2Reset2_MM_E", false);

            }

        }

        function ddlMealTime2Reset3OnDUTY_TIMEValid(source, arguments) {
            if ($('#ddlDUTY_TIME_HH_S').val() <= $('#ddlDUTY_TIME_HH_E').val()) {
                arguments.IsValid = CompTimeOnOtherTIME("ddlDUTY_TIME_HH_S",
                                                        "ddlDUTY_TIME_MM_S",
                                                        "ddlDUTY_TIME_HH_E",
                                                        "ddlDUTY_TIME_MM_E",
                                                        "ddlMealTime2Reset3_HH_S",
                                                        "ddlMealTime2Reset3_MM_S",
                                                        "ddlMealTime2Reset3_HH_E",
                                                        "ddlMealTime2Reset3_MM_E", false);
            }
            else {
                arguments.IsValid = CompTimeOnOtherTIMEToNextDay("ddlDUTY_TIME_HH_S",
                                        "ddlDUTY_TIME_MM_S",
                                        "ddlDUTY_TIME_HH_E",
                                        "ddlDUTY_TIME_MM_E",
                                        "ddlMealTime2Reset3_HH_S",
                                        "ddlMealTime2Reset3_MM_S",
                                        "ddlMealTime2Reset3_HH_E",
                                        "ddlMealTime2Reset3_MM_E", false);
            }

        }

        function MealTime2GreatMealTime1Valid(source, arguments) {
            arguments.IsValid = CompTimeGreatOtherTime("ddlMealTime2_HH_S",
                                                       "ddlMealTime2_MM_S",
                                                       "ddlMealTime2_HH_E",
                                                       "ddlMealTime2_MM_E",
                                                       "ddlMealTime1_HH_S",
                                                       "ddlMealTime1_MM_S",
                                                       "ddlMealTime1_HH_E",
                                                       "ddlMealTime1_MM_E");

        }

        function MealTime3GreatMealTime2Valid(source, arguments) {
            arguments.IsValid = CompTimeGreatOtherTime("ddlMealTime3_HH_S",
                                                       "ddlMealTime3_MM_S",
                                                       "ddlMealTime3_HH_E",
                                                       "ddlMealTime3_MM_E",
                                                       "ddlMealTime2_HH_S",
                                                       "ddlMealTime2_MM_S",
                                                       "ddlMealTime2_HH_E",
                                                       "ddlMealTime2_MM_E");

        }

        function MealTime3GreatMealTime1Valid(source, arguments) {
            arguments.IsValid = CompTimeGreatOtherTime("ddlMealTime3_HH_S",
                                                       "ddlMealTime3_MM_S",
                                                       "ddlMealTime3_HH_E",
                                                       "ddlMealTime3_MM_E",
                                                       "ddlMealTime1_HH_S",
                                                       "ddlMealTime1_MM_S",
                                                       "ddlMealTime1_HH_E",
                                                       "ddlMealTime1_MM_E");

        }

        function MealTime2Reset2GreatMealTime2Reset1Valid(source, arguments) {
            arguments.IsValid = CompTimeGreatOtherTime("ddlMealTime2Reset2_HH_S",
                                                       "ddlMealTime2Reset2_MM_S",
                                                       "ddlMealTime2Reset2_HH_E",
                                                       "ddlMealTime2Reset2_MM_E",
                                                       "ddlMealTime2Reset1_HH_S",
                                                       "ddlMealTime2Reset1_MM_S",
                                                       "ddlMealTime2Reset1_HH_E",
                                                       "ddlMealTime2Reset1_MM_E");

        }

        function MealTime2Reset3GreatMealTime2Reset2Valid(source, arguments) {
            arguments.IsValid = CompTimeGreatOtherTime("ddlMealTime2Reset3_HH_S",
                                                       "ddlMealTime2Reset3_MM_S",
                                                       "ddlMealTime2Reset3_HH_E",
                                                       "ddlMealTime2Reset3_MM_E",
                                                       "ddlMealTime2Reset2_HH_S",
                                                       "ddlMealTime2Reset2_MM_S",
                                                       "ddlMealTime2Reset2_HH_E",
                                                       "ddlMealTime2Reset2_MM_E");

        }

        function MealTime2Reset3GreatMealTime2Reset1Valid(source, arguments) {
            arguments.IsValid = CompTimeGreatOtherTime("ddlMealTime2Reset3_HH_S",
                                                       "ddlMealTime2Reset3_MM_S",
                                                       "ddlMealTime2Reset3_HH_E",
                                                       "ddlMealTime2Reset3_MM_E",
                                                       "ddlMealTime2Reset1_HH_S",
                                                       "ddlMealTime2Reset1_MM_S",
                                                       "ddlMealTime2Reset1_HH_E",
                                                       "ddlMealTime2Reset1_MM_E");

        }

        function CompTimeOnOtherTIME(HH_S_S, MM_S_S, HH_E_S, MM_E_S, HH_S_D, MM_S_D, HH_E_D, MM_E_D, ValidIsOn) {
            if ($("#" + HH_S_S).val() != "" && $("#" + MM_S_S).val() != "" && $("#" + HH_E_S).val() != "" && $("#" + MM_E_S).val() != "") {
                var S_TIME_Strat = getTimeMMs(0, HH_S_S, MM_S_S, true);
                var S_TIME_End = getTimeMMs(S_TIME_Strat, HH_E_S, MM_E_S, false);
                var D_TIME_Strat = getTimeMMs(0, HH_S_D, MM_S_D, true);
                var D_TIME_End = getTimeMMs(D_TIME_Strat, HH_E_D, MM_E_D, false);
                if (S_TIME_Strat != -1 && S_TIME_End != -1 && D_TIME_Strat != -1 && D_TIME_End != -1) {
                    if (ValidIsOn) {
                        if ((D_TIME_Strat >= S_TIME_Strat && D_TIME_Strat <= S_TIME_End) ||
                            (D_TIME_End >= S_TIME_Strat && D_TIME_End <= S_TIME_End))
                            return true;
                        else
                            return false;
                    }
                    else {
                        if ((D_TIME_Strat < S_TIME_Strat || D_TIME_Strat > S_TIME_End) ||
                            (D_TIME_End < S_TIME_Strat || D_TIME_End > S_TIME_End))
                            return false;
                        else
                            return true;
                    }
                }
                else {
                    if (ValidIsOn)
                        return false;
                    else
                        return true;
                }
            }
            else {
                if (ValidIsOn)
                    return false;
                else
                    return true;
            }
        }

        function CompTimeOnOtherTIMEToNextDay(HH_S_S, MM_S_S, HH_E_S, MM_E_S, HH_S_D, MM_S_D, HH_E_D, MM_E_D, ValidIsOn) {
            if ($("#" + HH_S_S).val() != "" && $("#" + MM_S_S).val() != "" && $("#" + HH_E_S).val() != "" && $("#" + MM_E_S).val() != "") {
                var S_TIME_Strat = getTimeMMs(0, HH_S_S, MM_S_S, true);
                var S_TIME_End = getTimeMMs(S_TIME_Strat, HH_E_S, MM_E_S, false);
                var D_TIME_Strat = getTimeMMs(0, HH_S_D, MM_S_D, true);
                if ($("#" + HH_E_S).val() >= $("#" + HH_S_D).val() && D_TIME_Strat != -1 && D_TIME_Strat / 60 < 24)
                    D_TIME_Strat += 24 * 60;
                var D_TIME_End = getTimeMMs(D_TIME_Strat, HH_E_D, MM_E_D, false);
                if ($("#" + HH_E_S).val() >= $("#" + HH_S_D).val() && D_TIME_End != -1 && D_TIME_End / 60 < 24)
                    D_TIME_End += 24 * 60;
                if (S_TIME_Strat != -1 && S_TIME_End != -1 && D_TIME_Strat != -1 && D_TIME_End != -1) {
                    if (ValidIsOn) {
                        if ((D_TIME_Strat >= S_TIME_Strat && D_TIME_Strat <= S_TIME_End) ||
                            (D_TIME_End >= S_TIME_Strat && D_TIME_End <= S_TIME_End))
                            return true;
                        else
                            return false;
                    }
                    else {
                        if ((D_TIME_Strat < S_TIME_Strat || D_TIME_Strat > S_TIME_End) ||
                            (D_TIME_End < S_TIME_Strat || D_TIME_End > S_TIME_End))
                            return false;
                        else
                            return true;
                    }
                }
                else {
                    if (ValidIsOn)
                        return false;
                    else
                        return true;
                }
            }
            else {
                if (ValidIsOn)
                    return false;
                else
                    return true;
            }
        }

        function CompTimeGreatOtherTime(HH_S_G, MM_S_G, HH_E_G, MM_E_G, HH_S_S, MM_S_S, HH_E_S, MM_E_S) {
            if ($("#" + HH_S_G).val() != "" && $("#" + MM_S_G).val() != "" && $("#" + HH_E_G).val() != "" && $("#" + MM_E_G).val() != "") {
                //較大時間起值
                var G_TIME_Strat = getTimeMMs(0, HH_S_G, MM_S_G, true);
                //較大時間迄值
                var G_TIME_End = getTimeMMs(G_TIME_Strat, HH_E_G, MM_E_G, false);
                //較小時間起值
                var S_TIME_Strat = getTimeMMs(0, HH_S_S, MM_S_S, true);
                //較小時間迄值
                var S_TIME_End = getTimeMMs(S_TIME_Strat, HH_E_S, MM_E_S, false);
                if (G_TIME_Strat != -1 && G_TIME_End != -1 && S_TIME_Strat != -1 && S_TIME_End != -1) {
                    if (G_TIME_Strat > S_TIME_Strat && G_TIME_Strat > S_TIME_End &&
                        G_TIME_End > S_TIME_Strat && G_TIME_End > S_TIME_End)
                        return true;
                    else
                        return false;
                }
                else
                    return true;
            }
            else
                return true;
        }

        function CompTimeGreatOtherTimeToNextDay(HH_S_G, MM_S_G, HH_E_G, MM_E_G, HH_S_S, MM_S_S, HH_E_S, MM_E_S) {
            if ($("#" + HH_S_G).val() != "" && $("#" + MM_S_G).val() != "" && $("#" + HH_E_G).val() != "" && $("#" + MM_E_G).val() != "") {
                //較大時間起值
                var G_TIME_Strat = getTimeMMs(0, HH_S_G, MM_S_G, true);
                //較大時間迄值
                var G_TIME_End = getTimeMMs(G_TIME_Strat, HH_E_G, MM_E_G, false);
                //較小時間起值
                var S_TIME_Strat = getTimeMMs(0, HH_S_S, MM_S_S, true);
                if ($("#" + HH_E_G).val() >= $("#" + HH_S_S).val() && S_TIME_Strat != -1 && S_TIME_Strat / 60 < 24)
                    S_TIME_Strat += 24 * 60;
                //較小時間迄值
                var S_TIME_End = getTimeMMs(S_TIME_Strat, HH_E_S, MM_E_S, false);
                if ($("#" + HH_E_G).val() >= $("#" + HH_E_S).val() && S_TIME_End != -1 && S_TIME_End / 60 < 24)
                    S_TIME_Strat += 24 * 60;

                if (G_TIME_Strat != -1 && G_TIME_End != -1 && S_TIME_Strat != -1 && S_TIME_End != -1) {
                    if (G_TIME_Strat > S_TIME_Strat && G_TIME_Strat > S_TIME_End &&
                        G_TIME_End > S_TIME_Strat && G_TIME_End > S_TIME_End)
                        return true;
                    else
                        return false;
                }
                else
                    return true;
            }
            else
                return true;
        }


        function GetHours() {
            getInCompanyHour();
            getWorkHours();
        }

        //公式:在廠時數 - (勤前用餐 + 用餐時段二 + 勤後用餐)-(休息時段(一) + 休息時段(二) + 休息時段(三)) 
        //SA 改成 工作時數=在廠時數-勤中用餐-[勤中休息(一)+勤中休息(二)+勤中休息(三)] ) 
        function getWorkHours() {
            if ($("#txt_InCompanyHour").val() != "") {
                var InCompanyTotalMM = getTimeStartToTimeEndMMs("ddlDUTY_TIME_HH_S", "ddlDUTY_TIME_MM_S", "ddlDUTY_TIME_HH_E", "ddlDUTY_TIME_MM_E");
                InCompanyTotalMM = (InCompanyTotalMM == -1 ? 0 : InCompanyTotalMM);
                //var MealTime1TotalMM = getTimeStartToTimeEndMMs("ddlMealTime1_HH_S", "ddlMealTime1_MM_S", "ddlMealTime1_HH_E", "ddlMealTime1_MM_E");
                //MealTime1TotalMM = (MealTime1TotalMM == -1 ? 0 : MealTime1TotalMM);
                var MealTime2TotalMM = getTimeStartToTimeEndMMs("ddlMealTime2_HH_S", "ddlMealTime2_MM_S", "ddlMealTime2_HH_E", "ddlMealTime2_MM_E");
                MealTime2TotalMM = (MealTime2TotalMM == -1 ? 0 : MealTime2TotalMM);
                //var MealTime3TotalMM = getTimeStartToTimeEndMMs("ddlMealTime3_HH_S", "ddlMealTime3_MM_S", "ddlMealTime3_HH_E", "ddlMealTime3_MM_E");
                //MealTime3TotalMM = (MealTime3TotalMM == -1 ? 0 : MealTime3TotalMM);
                var MealTime2Reset1TotalMM = getTimeStartToTimeEndMMs("ddlMealTime2Reset1_HH_S", "ddlMealTime2Reset1_MM_S", "ddlMealTime2Reset1_HH_E", "ddlMealTime2Reset1_MM_E");
                MealTime2Reset1TotalMM = (MealTime2Reset1TotalMM == -1 ? 0 : MealTime2Reset1TotalMM);
                var MealTime2Reset2TotalMM = getTimeStartToTimeEndMMs("ddlMealTime2Reset2_HH_S", "ddlMealTime2Reset2_MM_S", "ddlMealTime2Reset2_HH_E", "ddlMealTime2Reset2_MM_E");
                MealTime2Reset2TotalMM = (MealTime2Reset2TotalMM == -1 ? 0 : MealTime2Reset2TotalMM);
                var MealTime2Reset3TotalMM = getTimeStartToTimeEndMMs("ddlMealTime2Reset3_HH_S", "ddlMealTime2Reset3_MM_S", "ddlMealTime2Reset3_HH_E", "ddlMealTime2Reset3_MM_E");
                MealTime2Reset3TotalMM = (MealTime2Reset3TotalMM == -1 ? 0 : MealTime2Reset3TotalMM);

                //var TotalMealTimeMM = MealTime1TotalMM + MealTime2TotalMM + MealTime3TotalMM;
                var TotalMealTimeMM = MealTime2TotalMM;
                var TotalResetTimeMM = MealTime2Reset1TotalMM + MealTime2Reset2TotalMM + MealTime2Reset3TotalMM;
                var InCompanyRealTotalMM = InCompanyTotalMM - TotalMealTimeMM - TotalResetTimeMM;
                var InCompanyHH = parseInt(InCompanyRealTotalMM / 60, 10);
                var inCompanyMM = InCompanyRealTotalMM % 60;

                $("#txt_WorkHour").val(InCompanyHH + ":" + padLeft(inCompanyMM, 2));
                $("#hid_WorkHour").val(InCompanyHH + ":" + padLeft(inCompanyMM, 2));
            }
            else {
                $("#txt_WorkHour").val("");
                $("#hid_WorkHour").val("");
            }
        }

        function getTimeMMs(StartTimes, HH_Id, MM_Id, IsStart) {
            var NeedShfit24h = false;
            if (parseInt($("#ddlDUTY_TIME_HH_S").val(), 10) * 60 + parseInt($("#ddlDUTY_TIME_MM_S").val(), 10) >
                parseInt($("#ddlDUTY_TIME_HH_E").val(), 10) * 60 + parseInt($("#ddlDUTY_TIME_MM_E").val(), 10) &&
                HH_Id != "ddlMealTime1_HH_S" &&
                HH_Id != "ddlMealTime1_HH_E" &&
                HH_Id != "ddlMealTime1Reset_HH_S" &&
                HH_Id != "ddlMealTime1Reset_HH_E")
                NeedShfit24h = true;
            if (IsStart) {
                if ($("#" + HH_Id).val() != "" || $("#" + MM_Id).val() != "") {
                    if ($("#" + HH_Id).val() != "" && $("#" + MM_Id).val() != "") {
                        var returnValue = parseInt($("#" + HH_Id).val(), 10) * 60 + parseInt($("#" + MM_Id).val(), 10);
                        if (NeedShfit24h &&
                            returnValue < 24 * 60 &&
                            returnValue < parseInt($("#ddlDUTY_TIME_HH_S").val(), 10) * 60 + parseInt($("#ddlDUTY_TIME_MM_S").val(), 10))
                            return returnValue + 24 * 60;
                        else
                            return returnValue;
                    }
                    else
                        return -1;
                }
                else
                    return -1;
            }
            else {
                if ($("#" + HH_Id).val() != "" || $("#" + MM_Id).val() != "") {
                    if ($("#" + HH_Id).val() != "" && $("#" + MM_Id).val() != "") {
                        var End_TIME_E = parseInt($("#" + HH_Id).val(), 10) * 60 + parseInt($("#" + MM_Id).val(), 10);
                        if (End_TIME_E < StartTimes)
                            End_TIME_E += 24 * 60;
                        if (NeedShfit24h &&
                            End_TIME_E < 24 * 60 &&
                            End_TIME_E < parseInt($("#ddlDUTY_TIME_HH_S").val(), 10) * 60 + parseInt($("#ddlDUTY_TIME_MM_S").val(), 10))
                            return End_TIME_E + 24 * 60;
                        else
                            return End_TIME_E;
                    }
                    else
                        return -1;
                }
                else
                    return -1;
            }

        }

        function getTimeStartToTimeEndMMs(StartHH_Id, StartMM_Id, EndHH_Id, EndMM_Id) {
            if ($("#" + StartHH_Id).val() != "" || $("#" + StartMM_Id).val() != "" || $("#" + EndHH_Id).val() != "" || $("#" + EndMM_Id).val() != "") {
                if ($("#" + StartHH_Id).val() != "" && $("#" + StartMM_Id).val() != "" && $("#" + EndHH_Id).val() != "" && $("#" + EndMM_Id).val() != "") {
                    var Start_TIME = parseInt($("#" + StartHH_Id).val(), 10) * 60 + parseInt($("#" + StartMM_Id).val(), 10);
                    var End_TIME_E;
                    if ($("#" + EndHH_Id).val() == "00" && $("#" + EndHH_Id).val() != $("#" + StartHH_Id).val()) {
                        End_TIME_E = 24 * 60 + parseInt($("#" + EndMM_Id).val(), 10);
                    }
                    else
                        End_TIME_E = parseInt($("#" + EndHH_Id).val(), 10) * 60 + parseInt($("#" + EndMM_Id).val(), 10);

                    if (End_TIME_E < Start_TIME)
                        End_TIME_E = (parseInt($("#" + EndHH_Id).val(), 10) + 24) * 60 + parseInt($("#" + EndMM_Id).val(), 10);
                    return parseInt(End_TIME_E - Start_TIME, 10);
                }
                else
                    return -1;
            }
            else
                return -1;
        }

        function getInCompanyHour() {
            var InCompanyTotalMM = getTimeStartToTimeEndMMs("ddlDUTY_TIME_HH_S", "ddlDUTY_TIME_MM_S", "ddlDUTY_TIME_HH_E", "ddlDUTY_TIME_MM_E");
            if (InCompanyTotalMM > -1) {
                var InCompanyHH = parseInt(InCompanyTotalMM / 60, 10);
                var inCompanyMM = InCompanyTotalMM % 60;
                $("#txt_InCompanyHour").val(InCompanyHH + ":" + padLeft(inCompanyMM, 2));
                $("#hid_InCompanyHour").val(InCompanyHH + ":" + padLeft(inCompanyMM, 2));
            }
            else {
                $("#txt_InCompanyHour").val("");
                $("#hid_InCompanyHour").val("");
            }
        }

        function padLeft(str, len) {
            str = '' + str;
            if (str.length >= len) {
                return str;
            } else {
                return padLeft("0" + str, len);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table width="100%">
                            <tr>
                                <%--時段別區分--%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_SHIFT_TIME" runat="server" Text='<%$Resources:Resource,wf2da_lbSHIFT_TIME%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label">
                                    <uc1:UCCommCodeDropDwonList CssClass="MandatoryField" runat="server"
                                        ID="uc_SHIFT_TIME"
                                        DataTextField="SUB_CD,SUB_DESC"
                                        DataValueField="SUB_CD"
                                        SYS_CDs="DA"
                                        MAIN_CDs="SHIFT_TIME_CD"
                                        IS_VALID="True"
                                        DataTextFormatString="{0} - {1}"
                                        OrderSeq="ASC"
                                        FirstItem="<%$Resources:Resource,wfb2da_uc_SHIFT_TIME_PlaceChoice%>"
                                        ClientIDMode="Static"
                                        Require="true"
                                        ValidationGroup="GroupA"
                                        ValidatorErrMesg="<%$Resources:Resource,wfb2da_uc_SHIFT_TIME_NotNull%>" />
                                </td>
                                <%--複製班別代碼--%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_C_SHIFT_CD" runat="server" Text='複製班別代碼' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label" colspan="3">
                                    <asp:DropDownList ID="ddlC_SHIFT_CD" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddlC_SHIFT_CD_SelectedIndexChanged" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <%--班別代碼 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbSHIFT_CD" runat="server" Text='<%$Resources:Resource,wf2da_lbSHIFT_CD%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txtSHIFT_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static" Width="40px" MaxLength="2" style="TEXT-TRANSFORM:uppercase"  ValidationGroup="GroupA" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2da_SHIFT_CD_NotNull%>"
                                        ControlToValidate="txtSHIFT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <%--班別代碼說明 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbSHIFT_DESC" runat="server" Text='<%$Resources:Resource,wf2da_SHIFT_DESC%>'  ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label" colspan="3">
                                    <asp:TextBox ID="txtSHIFT_DESC" runat="server" CssClass="MandatoryField" ClientIDMode="Static" Width="98%" MaxLength="60" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2da_SHIFT_DESC_NotNull%>"
                                        ControlToValidate="txtSHIFT_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_WorkHour" runat="server" Text='<%$Resources:Resource,wfd2de_lb_WorkHour%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_WorkHour" runat="server" Enabled="false" CssClass="MandatoryField" ClientIDMode="Static" Width="40px" />
                                    <asp:HiddenField ID="hid_WorkHour" runat="server" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfd2de_lb_WorkHourNotNull%>"
                                        ControlToValidate="txt_WorkHour" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_InCompanyHour" runat="server" Text='<%$Resources:Resource,wfd2de_lb_InCompanyHour%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label" colspan="3">
                                    <asp:TextBox ID="txt_InCompanyHour" runat="server" Enabled="false" CssClass="MandatoryField" ClientIDMode="Static" Width="40px" />
                                    <asp:HiddenField ID="hid_InCompanyHour" runat="server" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfd2de_lb_InCompanyHourNotNull%>"
                                        ControlToValidate="txt_InCompanyHour" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <%--上班時段 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lblDUTY_TIME" runat="server" Text='<%$Resources:Resource,wfb2da_lb_DUTY_TIME_Add%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label" colspan="5">
                                    <asp:DropDownList ID="ddlDUTY_TIME_HH_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlDUTY_TIME_MM_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    ~
                                    <asp:DropDownList ID="ddlDUTY_TIME_HH_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlDUTY_TIME_MM_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                </td>
                            </tr>
                            <tr>
                                <%--勤前用餐 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbMealTime1" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime1%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlMealTime1_HH_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime1_MM_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    ~
                            <asp:DropDownList ID="ddlMealTime1_HH_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime1_MM_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbMealTime1Reset" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime1Reset%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label" colspan="3">
                                    <asp:DropDownList ID="ddlMealTime1Reset_HH_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime1Reset_MM_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime1Reset_HH_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime1Reset_MM_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                </td>
                            </tr>
                            <tr>
                                <%--勤中用餐 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbMealTime2" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime2%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label" colspan="5">
                                    <asp:DropDownList ID="ddlMealTime2_HH_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime2_MM_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime2_HH_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime2_MM_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbMealTime2Reset1" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime2Reset1%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlMealTime2Reset1_HH_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime2Reset1_MM_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime2Reset1_HH_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime2Reset1_MM_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbMealTime2Reset2" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime2Reset2%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlMealTime2Reset2_HH_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime2Reset2_MM_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime2Reset2_HH_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime2Reset2_MM_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbMealTime2Reset3" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime2Reset3%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlMealTime2Reset3_HH_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime2Reset3_MM_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime2Reset3_HH_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime2Reset3_MM_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="Label1" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime3%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlMealTime3_HH_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime3_MM_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime3_HH_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime3_MM_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="Label2" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime3Reset1%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlMealTime3Reset1_HH_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime3Reset1_MM_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime3Reset1_HH_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime3Reset1_MM_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="Label3" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime3Reset2%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlMealTime3Reset2_HH_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime3Reset2_MM_S" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime3Reset2_HH_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                    <asp:DropDownList ID="ddlMealTime3Reset2_MM_E" runat="server" ClientIDMode="Static" onchange="GetHours();" />
                                </td>

                            </tr>
                            <tr>
                                <%-- 輪班津貼 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_WORK_SHIFT_ALLOWANCE_TYPE" runat="server" Text='<%$Resources:Resource,wfb2da_lb_WORK_SHIFT_ALLOWANCE_TYPE%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label" colspan="5">
                                    <uc1:UCCommCodeDropDwonList runat="server"
                                        ID="uc_WORK_SHIFT_ALLOWANCE_TYPE"
                                        DataTextField="SUB_CD,SUB_DESC"
                                        DataValueField="SUB_CD"
                                        SYS_CDs="SC"
                                        MAIN_CDs="WORK_SHIFT_ALLOWANCE_TYPE"
                                        IS_VALID="True"
                                        DataTextFormatString="{0} - {1}"
                                        OrderSeq="ASC"
                                        FirstItem="<%$Resources:Resource,wfb2da_uc_WORK_SHIFT_ALLOWANCE_TYPE_PlaceChoice%>"
                                        ClientIDMode="Static" />
                                </td>
                            </tr>
                            <%-- 生效日期 --%>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_START_DT" runat="server" Text='<%$Resources:Resource,wfb2da_lb_START_DT%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label" colspan="5">
                                    <uc1:UCDateTimeRange
                                        ControlClientIDMode="Static"
                                        StartDateRequire="true"
                                        StartDateCssClass="MandatoryField"
                                        EndDateCssClass="MandatoryField"
                                        ValidateRequestMode="Enabled"
                                        StartDateRequireMsg="<%$Resources:Resource,wfb2da_START_DT_Start_NotNull%>"
                                        StartDateMaxLength="10"
                                        EndDateRequireMsg="<%$Resources:Resource,wfb2da_START_DT_End_NotNull%>"
                                        ValidationGroup="GroupA"
                                        EndDateRequire="true"
                                        ValidatorEndDateErrMesg="<%$Resources:Resource,wfb2da_START_DT_End_DateFormat_Error%>"
                                        ValidatorStartDateErrMesg="<%$Resources:Resource,wfb2da_START_DT_Start_DateFormat_Error%>"
                                        EndDateMaxLength="10"
                                        ClientValidationFunctionE="CheckDate"
                                        ClientValidationFunctionS="CheckDate"
                                        ClientIDMode="Static"
                                        CompareValidatorErrMesg="<%$Resources:Resource,wfb2da_START_DT_End_Need_Greater_Start%>"
                                        CompareValidatorOperator="GreaterThanEqual"
                                        runat="server"
                                        ID="UC_START_DT" />
                                </td>
                            </tr>
                            <tr>
                                <%--取代班別代碼--%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_R_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2db_lb_IR_SHIFT_CD%>" />
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlR_SHIFT_CD" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <%-- IFLOW顯示 --%>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_IS_IFLOW_SHOW" Text="<%$Resources:Resource,wfb2db_lb_IS_IFLOW_SHOW%>" />
                                </th>
                                <td>
                                    <asp:DropDownList ID="ddl_IS_IFLOW_SHOW" runat="server" CssClass="MandatoryField" Width="80px">
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2db_IS_IFLOW_SHOW_Y%>" Value="Y" />
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2db_IS_IFLOW_SHOW_N%>" Value="N" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_REMARK" runat="server" Text='<%$Resources:Resource,wfb2da_lb_REMARK%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label" colspan="5">
                                    <asp:TextBox ID="txt_REMARK" runat="server" ClientIDMode="Static" Width="98%" MaxLength="210" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="Body_label" colspan="6">
                                    <div id="init_grid" style="margin-bottom: 7px;">
                                        <%-- 確定 --%>
                                        <aces:Btn ID="WFB2DA0200Save" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0200Save%>" OnClick="WFB2DA0200Save_Click" ValidationGroup="GroupA" OnClientClick="return Valid();" ClientIDMode="Static" />
                                        <asp:Button ID="WFB2DA0200Cancel" runat="server" Text="<%$Resources:Resource,wfb2da_Cancel%>" ClientIDMode="Static" />
                                        <asp:Button ID="WFB2DA0200SaveIs_AlreadyConfirmAfter" runat="server" Text="" OnClick="WFB2DA0200SaveIs_AlreadyConfirmAfter_Click" Style="display: none;" ClientIDMode="Static" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            
            <asp:CustomValidator ID="CheckSHIFT_CDValidator" runat="server" ErrorMessage="<%$Resources:Resource,wfd2da_Error_SHIFT_CD%>" Display="None" ClientValidationFunction="CheckSHIFT_CD" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfd2da_MealTime2_NeedOnWorkTime%>" Display="None" ClientValidationFunction="ddlMealTime2OnDUTY_TIMEValid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTime1LessDUTY_TIMEValidator" runat="server" ErrorMessage="<%$Resources:Resource,wfd2da_MealTime1_NeedLessWorkTime%>" Display="None" ClientValidationFunction="ddlMealTime1LessDUTY_TIMEValid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTime3GreatDUTY_TIMEValidator" runat="server" ErrorMessage="<%$Resources:Resource,wfd2da_MealTime3GreatDUTY_TIME%>" Display="None" ClientValidationFunction="ddlMealTime3GreatDUTY_TIMEValid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTime3Reset1GreatDUTY_TIMEValidator" runat="server" ErrorMessage="<%$Resources:Resource,wfd2da_MealTime3Reset1GreatDUTY_TIME%>" Display="None" ClientValidationFunction="ddlMealTime3Reset1GreatDUTY_TIMEValid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTime3Reset2GreatDUTY_TIMEValidator" runat="server" ErrorMessage="<%$Resources:Resource,MealTime3Reset2GreatDUTY_TIME%>" Display="None" ClientValidationFunction="ddlMealTime3Reset2GreatDUTY_TIMEValid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTime1ResetLessDUTY_TIMEValidator" runat="server" ErrorMessage="<%$Resources:Resource,wfd2da_MealTime1Reset_NeedLessWorkTime%>" Display="None" ClientValidationFunction="MealTime1ResetLessDUTY_TIMEValid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTime2Reset1OnDUTY_TIMEValidator" runat="server" ErrorMessage="<%$Resources:Resource,wfd2da_MealTime2Reset1_NeedOnWorkTime%>" Display="None" ClientValidationFunction="ddlMealTime2Reset1OnDUTY_TIMEValid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTime2Reset2OnDUTY_TIMEValidator" runat="server" ErrorMessage="<%$Resources:Resource,wfd2da_MealTime2Reset2_NeedOnWorkTime%>" Display="None" ClientValidationFunction="ddlMealTime2Reset2OnDUTY_TIMEValid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTime2Reset3OnDUTY_TIMEValidator" runat="server" ErrorMessage="<%$Resources:Resource,wfd2da_MealTime2Reset3_NeedOnWorkTime%>" Display="None" ClientValidationFunction="ddlMealTime2Reset3OnDUTY_TIMEValid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTimeAndResetTimeCrossValidator" runat="server" ErrorMessage="<%$Resources:Resource,wfd2da_MealTimeAndResetTimeCantCross%>" Display="None" ClientValidationFunction="MealTimeAndResetTimeCrossValid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTime2GreatMealTime1Validator" runat="server" ErrorMessage="<%$Resources:Resource,MealTime2NeedGreatMealTime1%>" Display="None" ClientValidationFunction="MealTime2GreatMealTime1Valid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTime3GreatMealTime2Validator" runat="server" ErrorMessage="<%$Resources:Resource,MealTime3NeedGreatMealTime2%>" Display="None" ClientValidationFunction="MealTime3GreatMealTime2Valid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTime3GreatMealTime1Validator" runat="server" ErrorMessage="<%$Resources:Resource,MealTime3NeedGreatMealTime1%>" Display="None" ClientValidationFunction="MealTime3GreatMealTime1Valid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTime2Reset2GreatMealTime2Reset1Validator" runat="server" ErrorMessage="<%$Resources:Resource,MealTime2Reset2NeedGreatMealTime2Reset1%>" Display="None" ClientValidationFunction="MealTime2Reset2GreatMealTime2Reset1Valid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTime2Reset3GreatMealTime2Reset2Validator" runat="server" ErrorMessage="<%$Resources:Resource,MealTime2Reset3NeedGreatMealTime2Reset2%>" Display="None" ClientValidationFunction="MealTime2Reset3GreatMealTime2Reset2Valid" ValidationGroup="GroupA" />
            <asp:CustomValidator ID="MealTime2Reset3GreatMealTime2Reset1Validator" runat="server" ErrorMessage="<%$Resources:Resource,MealTime2Reset3NeedGreatMealTime2Reset1%>" Display="None" ClientValidationFunction="MealTime2Reset3GreatMealTime2Reset1Valid" ValidationGroup="GroupA" />
            <%--<asp:CustomValidator ID="TimeIntervalValidator" runat="server" ErrorMessage="各用餐時間與休息時間的間隔必須小於4個小時" Display="None" ClientValidationFunction="TimeIntervalValid" ValidationGroup="GroupA" />--%>
            <%--<asp:CustomValidator ID="WorkHoursValidator" runat="server" ErrorMessage="工作時數不為8" Display="None" ClientValidationFunction="WorkHoursValid" ValidationGroup="GroupA" />--%>
            <asp:ValidationSummary ID="ValidationSummary1" ClientIDMode="Static" EnableClientScript="true" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:HiddenField ID="hid_SHIFT_Already" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_EMP_DAY_DUTY_Already" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_Save_Confirm" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DA0200Save" />
            <asp:PostBackTrigger ControlID="WFB2DA0200SaveIs_AlreadyConfirmAfter" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
