<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0100_Dtl.aspx.cs" Inherits="WebContent_WFB2HB_WFB2HB0100_Dtl" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $.unblockUI();
            $(".textWidth").css("font-family", "Times New Roman, Times, serif").css("font-size", "14px");
            $(':text').keydown(function(){return false});  //所有的text檔位
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#tabs").tabs();

            $("#ddl_JPN_CD").change(function () {
                checkJPN_CD();
            });
            checkJPN_CD();
        }
        function ChangeTab(tab) {
            $("#tabs").tabs({ active: tab });
        }


        //家庭儲存前檢查
        function saveFamCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupFam")) {
                if ($("#txt_FAMILY_LICENSE_ID").val() != undefined) {
                    if (checkLicenseID($("#txt_FAMILY_LICENSE_ID").val()))
                        BlockUI();
                    else {
                        alert("眷屬身份證號檢查錯誤");
                        processed = false;
                    }
                }
                else
                    BlockUI();

            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }
        //教育儲存前檢查
        function saveEduCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupEdu")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }

        //教育儲存前檢查
        function saveExpCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupExp")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }
        //總儲存檢查
        function saveCheck() {
            var processed = true;


            if (Page_ClientValidate("GroupA")) {
                if ($("#txt_LICENSE_ID").val() != undefined) {
                    if (checkLicenseID($("#txt_LICENSE_ID").val()))
                        BlockUI();
                    else {
                        alert("身份證號檢查錯誤");
                        processed = false;
                    }
                }
                else
                    BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }

        function checkJPN_CD() {
            if ($("#ddl_JPN_CD").val() == "-1") {
                $("#txt_START_DT").attr("disabled", "disabled");
                $("#txt_END_DT").attr("disabled", "disabled");
                $("#ddl_RENT_SUBSIDY").attr("disabled", "disabled");
            }
            else {
                $("#txt_START_DT").removeAttr("disabled");
                $("#txt_END_DT").removeAttr("disabled");
                $("#ddl_RENT_SUBSIDY").removeAttr("disabled");
            }

        }

        //兼任的資料
        function openHRItem() {
            window.showModalDialog("WFB2HB0100_OtherPjob.aspx?emp_id=" + $("#txt_EMP_ID").val(), self, 'dialogWidth=600px;dialogHeight=400px;scroll=no');
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
                                <col width="16%" />
                                <col width="20%" />
                                <col width="12%" />
                                <col width="21%" />
                                <col width="9%" />
                                <col width="22%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                        <asp:HiddenField ID="hid_EMP_NAME" runat="server" />
                                    </td>
                                     <%-- 編輯照片 --%>
                                    <th class="Body_TH3">
                                        <asp:Label ID="lb_EMP_PHOTO" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_PHOTO%>"></asp:Label>:
                                    </th>
                                </tr>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_NATION_JPN_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_NATION_JPN_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_NATION_CD" runat="server" Font-Bold="false"></asp:Label>

                                        <asp:Label ID="lb_JPN_CD" runat="server" ClientIDMode="Static"></asp:Label>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BIRTH_BLOOD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BIRTH_BLOOD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_BIRTH_DT" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                        <%--<asp:TextBox ID="txt_BLOOD_TYPE" runat="server" BorderWidth="0" ReadOnly="true"></asp:TextBox>--%>
                                    </td>
                                     <%-- 照片位置 --%>
                                    <td rowspan="3" style="border-right: #cccc99 1px solid; border-top: #cccc99 1px solid; border-left: #cccc99 1px solid; border-bottom: #cccc99 1px solid;" align="center">
                                        <asp:Image ID="EmpPhoto" runat="server" Width="120px" Height="120px" ImageUrl="" />
                                    </td>
                                </tr>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SEX_ARMY" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SEX_ARMY%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SEX_CD" runat="server" BorderWidth="0" ReadOnly="true"></asp:TextBox>


                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HEIGHT_WEIGHT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_HEIGHT_WEIGHT%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_HEIGHT" runat="server" MaxLength="3" Width="35px" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                        <asp:TextBox ID="txt_WEIGHT" runat="server" MaxLength="3" Width="35px" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LICENSE_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LICENSE_ID2%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LICENSE_ID" runat="server" MaxLength="30" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                        <asp:HiddenField ID="hid_LICENSE_ID" runat="server" />

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BIRTHPLACE" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BIRTHPLACE%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_BIRTHPLACE" runat="server" MaxLength="30" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PASSPORT_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PASSPORT_ID%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PASSPORT_ID" runat="server" MaxLength="30" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ACCOUNT_BANK" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SALARY_ACCOUNT_BANK%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_BANK" runat="server" MaxLength="3" Width="30px" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>

                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_BANK_NAME" runat="server" ClientIDMode="Static" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ACCOUNT_BANK" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_ACCOUNT_BANK%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_BRANCH" runat="server" MaxLength="4" ClientIDMode="Static" Width="40px" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ACCOUNT_NO" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SALARY_ACCOUNT_NO%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <!--
                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_NO1" runat="server" MaxLength="3" ClientIDMode="Static" Width="22px" BorderWidth="0" ReadOnly="true"></asp:TextBox>-
                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_NO2" runat="server" MaxLength="2" ClientIDMode="Static" Width="16px" BorderWidth="0" ReadOnly="true"></asp:TextBox>-
                                            -->
                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_NO3" runat="server" MaxLength="14" ClientIDMode="Static" Width="120px" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_REMARK%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:TextBox ID="txt_REMARK" runat="server" MaxLength="120" Width="700px" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>

                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="10">
                        <asp:Button ID="WFB2HB0100Cancel" runat="server" Text="<%$Resources:Resource,wfb2hb_btn_back%>" Visible="true" OnClick="WFB2HB0100Cancel_Click" />
                    </td>
                </tr>
            </table>


            <div id="tabs" style="width: 1020px">
                <ul>
                    <li><a href="#tabs-1">【任職資料一】</a></li>
                    <li><a href="#tabs-2">【任職資料二】</a></li>
                    <li><a href="#tabs-3">【戶籍/現居/Mail】</a></li>
                    <li><a href="#tabs-4">【緊急連絡】</a></li>
                    <li><a href="#tabs-5">【扶養&所得稅】</a></li>
                    <li><a href="#tabs-6">【家庭成員】</a></li>
                    <li><a href="#tabs-7">【學歷】</a></li>
                    <li><a href="#tabs-8">【經歷】</a></li>
                    <li><a href="#tabs-9">【外籍赴任】</a></li>
                </ul>
                <div id="tabs-1">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                        <colgroup>
                           <col width="11%" />
                            <col width="25%" />
                            <col width="12%" />
                            <col width="15%" />
                            <col width="14%" />
                            <col width="23%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_JOIN_DT2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_JOIN_DT" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_EXAM_EXPIRE_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EXAM_EXPIRE_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_EXAM_EXPIRE_DT" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_IS_MASTER" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_IS_MASTER%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_IS_MASTER" runat="server" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_COMPANY_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_COMPANY_CD" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PLANT_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PLANT_CD" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_IS_UPD_HEAD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_IS_UPD_HEAD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_IS_UPD_HEAD" runat="server" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>                               
                               <%-- 部門代號 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DEPT_NO%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="3">
                                    <asp:TextBox ID="txt_DEPT_NO" runat="server" Width="400px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                    <%--<asp:TextBox ID="txt_DEPT_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>--%>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_DIRECT_HEAD_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DIRECT_HEAD_EMP_ID%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_DIRECT_HEAD_EMP_ID" runat="server" Width="100px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                    <%--<asp:TextBox ID="txt_DIRECT_HEAD_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>--%>
                                </td>
                            </tr>
                            <tr>
                               <th class="Body_TH3">
                                    <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WS_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_WS_CD" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                 <%-- 特休生成日 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_DL_GEN_DT" runat="server" Text="特休生成日"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_DL_GEN_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_OVERTIME_HEALTH_YEAR" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_OVERTIME_HEALTH_YEAR%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_OVERTIME_CTL_CD" runat="server" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                    <%--<asp:TextBox ID="txt_HEALTH_YEAR" runat="server" MaxLength="8" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>--%>
                                </td>
                            </tr>
                            <tr>
                                 <th class="Body_TH3">
                                    <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_EMP_CD" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PLAN_DESPATCH_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PLAN_DESPATCH_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PLAN_DESPATCH_DT" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_IS_DUTY_CHECK" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_IS_DUTY_CHECK%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_IS_DUTY_CHECK" runat="server" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>
                                 <th class="Body_TH3">
                                    <asp:Label ID="lb_LEVEL_GRADE" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LEVEL_GRADE%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_LEVEL_CD" runat="server" MaxLength="4" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                    <asp:TextBox ID="txt_GRADE_CD" runat="server" MaxLength="4" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                               
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_BE_DESPATCH_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BE_DESPATCH_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_BE_DESPATCH_DT" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_MODEL_YEAR" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_MODEL_YEAR%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_MODEL_YEAR" runat="server" MaxLength="4" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                 <th class="Body_TH3">
                                    <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PJOB_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PJOB_CD" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                    <input type="button" name="button1" value="兼任" onclick="openHRItem()">
                                </td>                                
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_KEEP_DESPATCH_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_KEEP_DESPATCH_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_KEEP_DESPATCH_DT" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_HONOR_YEAR" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_HONOR_YEAR%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_HONOR_YEAR" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_GRAGE" runat="server" Text="年級"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_GRAGE" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_BE_CONTRACT_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BE_CONTRACT_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_BE_CONTRACT_DT" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_UNION_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_UNION_PJOB_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_UNION_PJOB_CD" runat="server" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WORK_SHIFT_CD2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_WORK_SHIFT_CD" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_BE_EMP_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BE_EMP_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_BE_EMP_DT" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_WORK_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WORK_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_WORK_CD" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_CALENDAR_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_CALENDAR_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_CALENDAR_CD" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="Body_TD3"></td>
                                <td class="Body_TD3"></td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_ACC_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_ACC_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_ACC_CD" runat="server" MaxLength="8" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="tabs-2">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                        <colgroup>
                            <col width="12%" />
                            <col width="13%" />
                            <col width="14%" />
                            <col width="11%" />
                            <col width="14%" />
                            <col width="11%" />
                            <col width="12%" />
                            <col width="13%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_LEVEL_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_LEVEL_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_LEVEL_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                </td>

                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_PJOB_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_PJOB_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_PJOB_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_DEPT_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_DEPT_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_DEPT_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_DIV_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_DIV_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_DIV_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                </td>
                            </tr>

                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_LEVEL_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_LEVEL_WORK_DAYS2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_LEVEL_WORK_DAYS" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                </td>

                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_PJOB_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_PJOB_WORK_DAYS%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_PJOB_WORK_DAYS" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"  CssClass="textWidth"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_DEPT_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_DEPT_WORK_DAYS%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_DEPT_WORK_DAYS" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_DIV_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_DIV_WORK_DAYS%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_DIV_WORK_DAYS" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_STUDENT_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_STUDENT_WORK_DAYS%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_STUDENT_WORK_DAYS" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_K_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_K_WORK_DAYS%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_K_WORK_DAYS" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_T_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_T_WORK_DAYS%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_T_WORK_DAYS" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_WORK_YEARS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WORK_YEARS2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_WORK_YEARS" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_SERVICE_YEARS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SERVICE_YEARS%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="7">
                                    <asp:TextBox ID="txt_SERVICE_YEARS" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_LEAVE_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LEAVE_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_LEAVE_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_LEAVE_REASON_DESC" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LEAVE_REASON_DESC%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_LEAVE_REASON_DESC" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PLAN_RETENTION_EDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PLAN_RETENTION_EDT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PLAN_RETENTION_EDT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RETENTION_EDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RETENTION_EDT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RETENTION_EDT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_TRANSFER_SDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRANSFER_SDT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_TRANSFER_SDT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_TRANSFER_REASON_DESC" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRANSFER_REASON_DESC%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_TRANSFER_REASON_DESC" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PLAN_TRANSFER_EDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PLAN_TRANSFER_EDT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PLAN_TRANSFER_EDT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_TRANSFER_EDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRANSFER_EDT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_TRANSFER_EDT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_BACK_SCHOOL_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BACK_SCHOOL_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_BACK_SCHOOL_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_BACK_PLANT_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BACK_PLANT_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="5">
                                    <asp:TextBox ID="txt_BACK_PLANT_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static"  CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>

                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="tabs-3">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                        <colgroup>
                            <col width="12%" />
                            <col width="33%" />
                            <col width="12%" />
                            <col width="13%" />
                            <col width="12%" />
                            <col width="18%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <td align="left" class="Body_label" colspan="6">【戶籍】
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_ZIP" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_ZIP%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" colspan="5">
                                    <asp:TextBox ID="txt_REGISTER_ZIP_CD" runat="server" MaxLength="5" Width="50px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txt_REGISTER_COUNTY" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txt_REGISTER_REGION" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_REGISTER_ADD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_REGISTER_ADD%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="3">
                                    <asp:TextBox ID="txt_REGISTER_ADDR" runat="server" MaxLength="255" Width="500px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_REGISTER_TEL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_REGISTER_TEL2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_REGISTER_TEL" runat="server" MaxLength="10" Width="140px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static" CssClass="textWidth"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td align="left" class="Body_label" colspan="6">【通訊】
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_CONTACT_ZIP" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_CONTACT_ZIP%>"></asp:Label>:</th>
                                <td align="left" class="Body_TD3" colspan="5">
                                    <asp:TextBox ID="txt_CONTACT_ZIP_CD" runat="server" MaxLength="5" Width="50px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txt_CONTACT_COUNTY" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txt_CONTACT_REGION" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                                </td>
                            </tr>

                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_CONTACT_ADDR" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_CONTACT_ADDR2%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="3">
                                    <asp:TextBox ID="txt_CONTACT_ADDR" runat="server" MaxLength="255" Width="630px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_CONTACT_TEL2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_CONTACT_TEL2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_CONTACT_TEL" runat="server" MaxLength="10" Width="140px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PERSONAL_EMAIL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PERSONAL_EMAIL%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PERSONAL_EMAIL" runat="server" MaxLength="50" Width="280px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static" CssClass="textWidth"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_MOBILE_TEL_1" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_MOBILE_TEL_1%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_MOBILE_TEL_1" runat="server" MaxLength="10" Width="140px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_MOBILE_TEL_2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_MOBILE_TEL_2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_MOBILE_TEL_2" runat="server" MaxLength="10" Width="140px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static" CssClass="textWidth"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_SALARY" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SALARY%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="5">
                                    <asp:CheckBox ID="cb_SALARY" runat="server" />
                                </td>
                            </tr>

                            <tr>
                                <td align="left" class="Body_label" colspan="6">【公司】
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_COMPANY_EMAIL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_COMPANY_EMAIL%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="5">
                                    <asp:TextBox ID="txt_COMPANY_EMAIL" runat="server" MaxLength="50" Width="280px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static" CssClass="textWidth"></asp:TextBox>
                                </td>
                                <%--                            <th class="Body_TH3">
                                <asp:Label ID="lb_COMPANY_EXT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_COMPANY_EXT%>"></asp:Label>:</th>
                            <td class="Body_TD3">
                                <asp:TextBox ID="txt_COMPANY_EXT" runat="server" MaxLength="10" Width="140px" ClientIDMode="Static"></asp:TextBox>
                            </td>--%>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_SALARY_2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SALARY%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="5">
                                    <asp:CheckBox ID="cb_SALARY_2" runat="server" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="tabs-4">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                        <colgroup>
                            <col width="15%" />
                            <col width="85%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_URGENT_CONTACT_NAME" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_URGENT_CONTACT_NAME2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_URGENT_CONTACT_NAME" runat="server" MaxLength="30" Width="100px" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_URGENT_CONTACT_TEL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_URGENT_CONTACT_TEL%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_URGENT_CONTACT_TEL" runat="server" MaxLength="255" Width="630px" BorderWidth="0" ReadOnly="true" CssClass="textWidth" ClientIDMode="Static"></asp:TextBox>
                                </td>  
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_URGENT_CONTACT_RELATION" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_URGENT_CONTACT_RELATION%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_URGENT_CONTACT_RELATION" runat="server" Enabled="false" Visible="false"></asp:DropDownList>
                                    <asp:TextBox ID="txt_URGENT_CONTACT_RELATION" runat="server" MaxLength="14" Width="700px" ClientIDMode="Static" BorderWidth="0" ReadOnly="true" Visible="false"></asp:TextBox>
                                    <!--緊急聯絡人關係-->
                                    <asp:TextBox ID="txt_URGENT_CONTACT_RELATION_DESC" runat="server" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="tabs-5">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                        <colgroup>
                            <col width="15%" />
                            <col width="85%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RELATIVES" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RELATIVES%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RELATIVES" runat="server" MaxLength="5" Width="70px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_INCOME_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_INCOME_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_INCOME_CD" runat="server" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="tabs-6">

                    <asp:GridView ID="gv_result" runat="server" AllowSorting="true" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="980px">
                        <Columns>
                            <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </ItemTemplate>


                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_NATION_CD%>" SortExpression="FAMILY_NATION_CD">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_NATION_CD" runat="server" Text='<%#Bind("FAMILY_NATION_DESC")%>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_SEX_CD%>" SortExpression="FAMILY_SEX_CD" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_SEX_CD" runat="server" Text='<%#Bind("FAMILY_SEX_DESC")%>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_LICENSE_ID%>" SortExpression="FAMILY_LICENSE_ID">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_LICENSE_ID" runat="server" Text='<%#Bind("FAMILY_LICENSE_ID")%>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_PASSPORT_ID%>" SortExpression="FAMILY_PASSPORT_ID">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_PASSPORT_ID" runat="server" Text='<%#Bind("FAMILY_PASSPORT_ID")%>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_NAME%>" SortExpression="FAMILY_NAME">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_NAME" runat="server" Text='<%#Bind("FAMILY_NAME")%>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_RELATION%>" SortExpression="FAMILY_RELATION">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_RELATION" runat="server" Text='<%#Bind("FAMILY_RELATION_DESC")%>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_BIRTH_DT%>" SortExpression="FAMILY_BIRTH_DT">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_BIRTH_DT" runat="server" BorderWidth="0" ReadOnly="true" Text='<%#Bind("FAMILY_BIRTH_DT")%>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_FAMILY_WORK_DESC%>" SortExpression="FAMILY_WORK_DESC">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_WORK_DESC" runat="server" Text='<%#Bind("FAMILY_WORK_DESC")%>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_ALLOWANCE%>" SortExpression="IS_ALLOWANCE">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_IS_ALLOWANCE" runat="server" Enabled="false" />
                                    <asp:HiddenField ID="hid_IS_ALLOWANCE" runat="server" Value='<%#Bind("IS_ALLOWANCE")%>' />
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_BENEFICIARY%>" SortExpression="BENEFICIARY">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_BENEFICIARY" runat="server" Enabled="false" />
                                    <asp:HiddenField ID="hid_BENEFICIARY" runat="server" Value='<%#Bind("BENEFICIARY")%>' />
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_VENDOR_ID%>" SortExpression="VENDOR_ID">
                                <ItemTemplate>
                                    <asp:Label ID="lb_VENDOR_ID" runat="server" Text='<%#Bind("VENDOR_ID")%>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_VALID%>" SortExpression="IS_VALID">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_IS_VALID" runat="server" Enabled="false" />
                                    <asp:HiddenField ID="hid_IS_VALID" runat="server" Value='<%#Bind("IS_VALID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EditRowStyle CssClass="normal" />
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <FooterStyle CssClass="GridviewScrollPager" />
                    </asp:GridView>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupFam" ShowSummary="false" />
                </div>
                <div id="tabs-7">


                    <asp:GridView ID="gv_result2" runat="server" AllowSorting="true" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result2_Sorting"
                        OnRowDataBound="gv_result2_RowDataBound" OnRowCreated="gv_result2_RowCreated" Width="1020px">
                        <Columns>

                            <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </ItemTemplate>


                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_SCHOOL_NATION_CD%>" SortExpression="SCHOOL_NATION_CD" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_SCHOOL_NATION_DESC" runat="server" Text='<%#Bind("SCHOOL_NATION_DESC")%>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_EDUCATION_CD%>" SortExpression="EDUCATION_CD" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_EDUCATION_CD" runat="server" Text='<%#Bind("EDUCATION_DESC")%>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_SCHOOL_NAME%>" SortExpression="SCHOOL_NAME" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_SCHOOL_NAME" runat="server" Text='<%#Bind("SCHOOL_NAME")%>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_DEPARTMENT_NAME%>" SortExpression="DEPARTMENT_NAME" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_DEPARTMENT_NAME" runat="server" Text='<%#Bind("DEPARTMENT_NAME")%>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_GRADUATION_YEAR%>" SortExpression="GRADUATION_YEAR" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_GRADUATION_YEAR" runat="server" Text='<%#Bind("GRADUATION_YEAR")%>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_SALARY_SCHOOL%>" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_IS_SALARY_SCHOOL" runat="server" Enabled="false" />
                                    <asp:HiddenField ID="hid_IS_SALARY_SCHOOL" runat="server" Value='<%#Bind("IS_SALARY_SCHOOL")%>' />
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_VIRTUAL_SCHOOL%>" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_IS_VIRTUAL_SCHOOL" runat="server" Enabled="false" />
                                    <asp:HiddenField ID="hid_IS_VIRTUAL_SCHOOL" runat="server" Value='<%#Bind("IS_VIRTUAL_SCHOOL")%>' />
                                </ItemTemplate>

                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>

                            <table class="grid-view">
                                <tr class="header">
                                    <td></td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2hb_RowNumber%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2hb_SCHOOL_NATION_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2hb_EDUCATION_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2hb_SCHOOL_NAME%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DEPARTMENT_NAME%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_GRADUATION_YEAR%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_IS_SALARY_SCHOOL%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_IS_VIRTUAL_SCHOOL%>"></asp:Label>
                                    </td>

                                </tr>
                                <tr class="normal">
                                    <td></td>
                                    <td>
                                        <asp:Label ID="lbl_EmptyRowNumber" runat="server" Text="1"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_SCHOOL_NATION_CD" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_EDUCATION_CD" runat="server" CssClass="MandatoryField">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EDUCATION_CD%>"
                                            ControlToValidate="ddl_EDUCATION_CD" ForeColor="Red" ValidationGroup="GroupEdu" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_SCHOOL_NAME" runat="server" MaxLength="20" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_SCHOOL_NAME%>"
                                            ControlToValidate="txt_SCHOOL_NAME" ForeColor="Red" ValidationGroup="GroupEdu" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_DEPARTMENT_NAME" runat="server" MaxLength="20" Width="140px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_GRADUATION_YEAR" runat="server" MaxLength="30" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_GRADUATION_YEAR%>"
                                            ControlToValidate="txt_GRADUATION_YEAR" ForeColor="Red" ValidationGroup="GroupEdu" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_IS_SALARY_SCHOOL" runat="server" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_IS_VIRTUAL_SCHOOL" runat="server" />
                                    </td>

                                </tr>
                            </table>

                        </EmptyDataTemplate>
                        <EditRowStyle CssClass="normal" />
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <FooterStyle CssClass="GridviewScrollPager" />
                    </asp:GridView>
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupEdu" ShowSummary="false" />
                </div>
                <div id="tabs-8">

                    <asp:GridView ID="gv_result3" runat="server" AllowSorting="true" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result3_Sorting"
                        OnRowDataBound="gv_result3_RowDataBound" Width="1020px">
                        <Columns>

                            <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </FooterTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_EXP_COMPANY_NAME%>" SortExpression="EXP_COMPANY_NAME" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_EXP_COMPANY_NAME" runat="server" Text='<%#Bind("EXP_COMPANY_NAME")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_EXP_COMPANY_NAME" runat="server" Text='<%#Bind("EXP_COMPANY_NAME")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_EXP_COMPANY_NAME" runat="server" MaxLength="20" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EXP_COMPANY_NAME%>"
                                        ControlToValidate="txt_EXP_COMPANY_NAME" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_EXP_TITLE_DESC%>" SortExpression="EXP_TITLE_DESC" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_EXP_TITLE_DESC" runat="server" Text='<%#Bind("EXP_TITLE_DESC")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_EXP_TITLE_DESC" runat="server" MaxLength="20" Width="140px" Text='<%#Bind("EXP_TITLE_DESC")%>' CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EXP_TITLE_DESC%>"
                                        ControlToValidate="txt_EXP_TITLE_DESC" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_EXP_TITLE_DESC" runat="server" MaxLength="20" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EXP_TITLE_DESC%>"
                                        ControlToValidate="txt_EXP_TITLE_DESC" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_START_YEAR%>" SortExpression="START_YEAR" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_START_YEAR" runat="server" Text='<%#Bind("START_YEAR")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_START_YEAR" runat="server" MaxLength="20" Width="140px" Text='<%#Bind("START_YEAR")%>' CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_START_YEAR%>"
                                        ControlToValidate="txt_START_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_START_YEAR" runat="server" MaxLength="20" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_START_YEAR%>"
                                        ControlToValidate="txt_START_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_END_YEAR%>" SortExpression="END_YEAR" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_END_YEAR" runat="server" Text='<%#Bind("END_YEAR")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_END_YEAR" runat="server" MaxLength="20" Width="140px" Text='<%#Bind("END_YEAR")%>' CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_END_YEAR%>"
                                        ControlToValidate="txt_END_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_END_YEAR" runat="server" MaxLength="20" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_END_YEAR%>"
                                        ControlToValidate="txt_END_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_APPROVE_WORK_YEARS%>" SortExpression="APPROVE_WORK_YEARS" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_APPROVE_WORK_YEARS" runat="server" Text='<%#Bind("APPROVE_WORK_YEARS")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_APPROVE_WORK_YEARS" runat="server" MaxLength="20" Width="140px" Text='<%#Bind("APPROVE_WORK_YEARS")%>' CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_APPROVE_WORK_YEARS%>"
                                        ControlToValidate="txt_APPROVE_WORK_YEARS" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_APPROVE_WORK_YEARS" runat="server" MaxLength="20" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_APPROVE_WORK_YEARS%>"
                                        ControlToValidate="txt_APPROVE_WORK_YEARS" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EditRowStyle CssClass="normal" />
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <FooterStyle CssClass="GridviewScrollPager" />
                    </asp:GridView>
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupExp" ShowSummary="false" />
                </div>
                <div id="tabs-9">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                        <colgroup>
                            <col width="15%" />
                            <col width="35%" />
                            <col width="15%" />
                            <col width="35%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DUR_START_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_START_DT" runat="server" MaxLength="8" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_END_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DUR_END_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_END_DT" runat="server" MaxLength="8" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RENT_SUBSIDY" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RENT_SUBSIDY%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="3">
                                    <asp:TextBox ID="txt_RENT_SUBSIDY" runat="server" ClientIDMode="Static" BorderWidth="0" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <asp:HiddenField ID="hid_IS_DURATION" runat="server" />
                        </tbody>
                    </table>

                </div>
            </div>
            <asp:ValidationSummary ID="ValidationSummary4" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
