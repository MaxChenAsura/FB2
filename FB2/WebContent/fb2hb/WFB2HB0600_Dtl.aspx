<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0600_Dtl.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0600_Dtl" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $("#tabs").tabs();
            $(".textWidth").css("font-family", "Times New Roman, Times, serif").css("font-size", "14px");
        }
        function ChangeTab(tab) {
            $("#tabs").tabs({ active: tab });
        }
    </script>

    <style type="text/css">
        .auto-style1 {
            width: 143px;
            font-family: "Times New Roman", Times, serif;
            font-size: 14px;
            font-weight: bold;
            color: #FFFFFF;
            background-color: #7F7F7F;
        }

        .auto-style2 {
            width: 112px;
            font-family: "Times New Roman", Times, serif;
            font-size: 14px;
            font-weight: bold;
            color: #FFFFFF;
            background-color: #7F7F7F;
        }

        .auto-style3 {
            width: 87px;
            font-family: "Times New Roman", Times, serif;
            font-size: 14px;
            font-weight: bold;
            color: #FFFFFF;
            background-color: #7F7F7F;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0">

                <tr>
                    <th class="Body_TH1" style="width: 120px;">
                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th style="width: 50px;"></th>
                    <th class="Body_TH1" style="width: 120px;">
                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th style="width: 100px;"></th>
                    <th class="Body_TH1" style="width: 150px;">
                        <asp:Label ID="lb_EMP_CHG_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_CHG_CD%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_EMP_CHG_CD" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th class="Body_TR1" style="width: 50px;"></th>
                    <!--<td class="Body_TR1" style="width:20%;">   -->
                    <td align="center" rowspan="5">
                        <asp:Image ID="EmpPhoto" runat="server" Width="120px" Height="120px" />

                    </td>
                </tr>
                <tr>
                    <th class="Body_TH1" style="width: 120px;">
                        <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_LEVEL_CD%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_LEVEL_CD" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th style="width: 50px;"></th>
                    <th class="Body_TH1" style="width: 120px;">
                        <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_PJOB_CD%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;" colspan="4">
                        <asp:TextBox ID="txt_PJOB_CD" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th style="width: 100px;"></th>

                </tr>
                <tr>
                    <th class="Body_TH1" style="width: 120px;">
                        <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_PLANT_CD%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_PLANT_CD" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th style="width: 50px;"></th>
                    <%--部門 --%>
                    <th class="Body_TH1" style="width: 120px;">
                        <asp:Label ID="lb_DEPT_NAME" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DEPT_NAME%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;" colspan="4">
                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" MaxLength="10" Width="300px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th style="width: 100px;"></th>
                </tr>
                <tr>
                    <th class="Body_TH1" style="width: 120px;">
                        <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_JOIN_DT%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_JOIN_DT" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th style="width: 50px;"></th>
                    <th class="Body_TH1" style="width: 120px;">
                        <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WS_CD%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_WS_CD" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th style="width: 100px;"></th>
                    <th class="Body_TH1" style="width: 150px;">
                        <asp:Label ID="lb_SEX_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SEX_CD%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_SEX_CD" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th class="Body_TH1" style="width: 120px;">
                        <asp:Label ID="lb_WORK_YEARS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WORK_YEARS%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_WORK_YEARS" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th style="width: 50px;"></th>
                    <th class="Body_TH1" style="width: 130px;">
                        <asp:Label ID="lb_MODEL_YEAR" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_MODEL_YEAR%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_MODEL_YEAR" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th style="width: 100px;"></th>
                    <th class="Body_TH1" style="width: 150px;">
                        <asp:Label ID="lb_BIRTH_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BIRTH_DT%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_BIRTH_DT" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <%-- 資格年資--%>
                    <th class="Body_TH1" style="width: 120px;">
                        <asp:Label ID="lb_RECENT_LEVEL_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_LEVEL_WORK_DAYS%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_RECENT_LEVEL_WORK_DAYS" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th style="width: 50px;"></th>
                    <th class="Body_TH1" style="width: 120px;">
                        <asp:Label ID="lb_BASE_SALARY" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BASE_SALARY%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_BASE_SALARY" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th style="width: 100px;"></th>
                    <th class="Body_TH1" style="width: 150px;">
                        <asp:Label ID="lb_AGE" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_AGE%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;" colspan="4">
                        <asp:TextBox ID="txt_AGE" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <%-- 日文--%>
                    <th class="Body_TH1" style="width: 120px;">
                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_total_score_jpn%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_TOTAL_SCORE_JPN" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th style="width: 50px;"></th>
                    <%-- 英文TOEIC--%>
                    <th class="Body_TH1" style="width: 120px;">
                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_total_score_toeic%>"></asp:Label>:</th>
                    <td class="Body_TR1" style="width: 100px;">
                        <asp:TextBox ID="txt_TOTAL_SCORE_TOEIC" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                    </td>
                    <th style="width: 100px;"></th>
                    <th style="width: 150px;"></th>
                    <td style="width: 100px;" colspan="4"></td>
                </tr>
                <tr>
                    <td align="right" colspan="10">
                        <asp:Button ID="WFB2HB0600Cancel" runat="server" Text="<%$Resources:Resource,wfb2hb_btn_back%>" Visible="true" OnClick="WFB2HB0600Cancel_Click" />
                    </td>
                </tr>
            </table>


            <div id="tabs" style="width: 1020px">
                <ul>
                    <li id="tabs1"><a href="#tabs-1">【考核】</a></li>
                    <li id="tabs2"><a href="#tabs-2">【人事異動履歷】</a></li>
                    <li id="tabs3"><a href="#tabs-3">【外調履歷】</a></li>
                    <li id="tabs4"><a href="#tabs-4">【國外研修】</a></li>
                    <li id="tabs5"><a href="#tabs-5">【兼任】</a></li>
                    <li id="tabs6"><a href="#tabs-6">【學歷】</a></li>
                    <li id="tabs7"><a href="#tabs-7">【家庭情況】</a></li>
                    <li id="tabs8"><a href="#tabs-8">【連絡資料】</a></li>
                </ul>
                <div id="tabs-1">
                    <iframe id="iframe1" name="iframe" frameborder="0" border="0" cellspacing="0" width="1020px" height="280px" src="" runat="server" style="border: none"></iframe>
                </div>

                <div id="tabs-2">
                    <iframe id="iframe2" name="iframe" frameborder="0" border="0" cellspacing="0" width="1020px" height="280px" src="" runat="server" style="border: none"></iframe>
                </div>

                <div id="tabs-3">
                    <iframe id="iframe3" name="iframe" frameborder="0" border="0" cellspacing="0" width="1020px" height="300px" src="" runat="server" style="border: none"></iframe>
                </div>

                <div id="tabs-4">
                    <iframe id="iframe4" name="iframe" frameborder="0" border="0" cellspacing="0" width="1020px" height="300px" src="" runat="server" style="border: none"></iframe>
                </div>

                <div id="tabs-5">
                    <iframe id="iframe5" name="iframe" frameborder="0" border="0" cellspacing="0" width="1020px" height="300px" src="" runat="server" style="border: none"></iframe>
                </div>

                <div id="tabs-6">
                    <iframe id="iframe6" name="iframe" frameborder="0" border="0" cellspacing="0" width="1020px" height="300px" src="" runat="server" style="border: none"></iframe>
                </div>

                <div id="tabs-7">
                    <iframe id="iframe7" name="iframe" frameborder="0" border="0" cellspacing="0" width="1020px" height="300px" src="" runat="server" style="border: none"></iframe>
                </div>

                <div id="tabs-8">
                    <table cellspacing="1" cellpadding="1" width="1020" border="0">
                        <tr>
                            <th align="left" class="auto-style1">
                                <asp:Label ID="lb_URGENT_CONTACT_NAME" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_URGENT_CONTACT_NAME%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="5">
                                <asp:TextBox ID="txt_URGENT_CONTACT_NAME" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true" CssClass="textWidth"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="auto-style1">
                                <asp:Label ID="lb_URGENT_CONTACT_TEL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_URGENT_CONTACT_TEL%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="5">
                                <asp:TextBox ID="txt_URGENT_CONTACT_TEL" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true" CssClass="textWidth"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="auto-style1">
                                <asp:Label ID="lb_URGENT_CONTACT_RELATION" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_URGENT_CONTACT_RELATION%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="5">
                                <asp:TextBox ID="txt_URGENT_CONTACT_RELATION" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true" CssClass="textWidth"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <br>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="auto-style1">
                                <asp:Label ID="lb_REGISTER_ADDR" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_REGISTER_ADDR%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:TextBox ID="txt_REGISTER_ADDR" runat="server" MaxLength="10" Width="400px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true" CssClass="textWidth"></asp:TextBox>

                            </td>
                            <th align="left" class="auto-style3">
                                <asp:Label ID="lb_REGISTER_TEL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_REGISTER_TEL%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_REGISTER_TEL" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true" CssClass="textWidth"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <th align="left" class="auto-style1">
                                <asp:Label ID="lb_CONTACT_ADDR" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_CONTACT_ADDR%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:TextBox ID="txt_CONTACT_ADDR" runat="server" MaxLength="10" Width="400px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true" CssClass="textWidth"></asp:TextBox>
                            </td>
                            <th align="left" class="auto-style3">
                                <asp:Label ID="lb_CONTACT_TEL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_CONTACT_TEL%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_CONTACT_TEL" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true" CssClass="textWidth"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="auto-style1">
                                <asp:Label ID="lb_MOBILE_TEL_1" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_MOBILE_TEL_1%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_MOBILE_TEL_1" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true" CssClass="textWidth"></asp:TextBox>
                            </td>
                            <th align="left" class="auto-style2">
                                <asp:Label ID="lb_MOBILE_TEL_2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_MOBILE_TEL_2%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_MOBILE_TEL_2" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true" CssClass="textWidth"></asp:TextBox>
                            </td>
                            <%--                            <th align="left" class="auto-style3">
                                <asp:Label ID="lb_COMPANY_EXT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_COMPANY_EXT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_COMPANY_EXT" runat="server" MaxLength="10" Width="120px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                            </td>--%>
                        </tr>

                        <tr>
                            <th align="left" class="auto-style1">
                                <asp:Label ID="lb_PERSONAL_EMAIL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PERSONAL_EMAIL%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_PERSONAL_EMAIL" runat="server" MaxLength="10" Width="150px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true" CssClass="textWidth"></asp:TextBox>
                            </td>
                            <th align="left" class="auto-style2">
                                <asp:Label ID="lb_COMPANY_EMAIL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_COMPANY_EMAIL%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:TextBox ID="txt_COMPANY_EMAIL" runat="server" MaxLength="10" Width="150px" BorderWidth="0" ClientIDMode="Static" ReadOnly="true" CssClass="textWidth"></asp:TextBox>
                            </td>
                        </tr>

                    </table>
                </div>
            </div>

            <asp:HiddenField ID="hid_emp_id" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
