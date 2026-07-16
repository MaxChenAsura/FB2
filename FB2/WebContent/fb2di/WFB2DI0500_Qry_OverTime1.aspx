<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0500_Qry_OverTime1.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0500_Qry_OverTime1" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        <title></title>
    <script type="text/javascript">

 </script>
    <style type="text/css">
        .auto-style1 {
            height: 35px;
        }
        </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <div class='pos_top'>


            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="12%" />
                    <col width="12%" />
                    <col width="11%" />
                    <col width="11%" />
                    <col width="11%" />
                    <col width="11%" />
                    <col width="11%" />
                    <col width="23%" />
                </colgroup>
                <tbody style="height: 25px;">
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_YEARMONTH" runat="server" Text="<%$Resources:Resource,wfb2di_lb_YearMonth%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_YEARMONTH" runat="server" MaxLength="10" Width="80px" BorderWidth="0"></asp:Label>
                            <!-- <input class='MandatoryField' type="text" name="workName" maxlength="10" size="10" tabindex="10"  value="201310" >  -->
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_EMP_ID" runat="server" BorderWidth="0" MaxLength="10" Width="100px" ClientIDMode="Static"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_NAME%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_EMP_NAME" runat="server" MaxLength="15" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_DEPT_NO" runat="server" BorderWidth="0" MaxLength="15" Width="140px" ClientIDMode="Static"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td align="right" class="Body_label" colspan="8">
                            <div id="init">
                                <!--<a href="#" tabindex="91" onclick="" name="search" id="search"><img src="images/tw/search.gif" style="border=0" alt="search" ></a>
													<a href="#" tabindex="91" onclick="" name="clear" id="clear"><img src="images/tw/clear.gif" style="border=0" alt="clear" ></a>  -->
<%--                                <a href='WFB2DI0500_Qry.html'>--%>
                                    <input ID="WFB2DI0500Cancel" runat="server" type="button" value="<%$Resources:Resource,wfb2hb_WFB2DI0500Cancel%>" Visible="true" OnClick="history.back();"  />
                            </div>
                        </td>
                    </tr>

                </tbody>
            </table>

            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="12%" />
                    <col width="11%" />
                    <col width="12%" />
                    <col width="11%" />
                    <col width="11%" />
                    <col width="11%" />
                    <col width="11%" />
                    <col width="12%" />
                    <col width="13%" />

                </colgroup>

                <tbody style="height: 25px;">
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_OVERTIME_CTL_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_CTL_CD_2%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_OVERTIME_CTL_CD" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:Label>
                        </td>

                        <td align="right" class="auto-style1">
                            <asp:Label ID="lb_ActualTotalTime" runat="server" Text="<%$Resources:Resource,wfb2di_lb_ActualTotalTime%>"></asp:Label>:</td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ACTUAL_WEEKDAYS_OVERTIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_Weekdays_Overtime%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_ACTUAL_WEEKDAYS_OVERTIME" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ACTUAL_EXCHANGED" runat="server" Text="<%$Resources:Resource,wfb2di_lb_Exchanged%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_ACTUAL_EXCHANGED" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_label"></th>
                            <td align="left" class="Body_label">
                            </td>
                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_OVERTIME_GRAND_TOTAL" runat="server" Text="<%$Resources:Resource,wfb2di_lb_Overtime_Grand_Total%>"></asp:Label></th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_OVERTIME_GRAND_TOTAL" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:Label>
                        </td>
                        <td align="right" class="Body_label"></td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ACTUAL_HOLIDAY_OVERTIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_Holiday_Overtime%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_ACTUAL_HOLIDAY_OVERTIME" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ACTUAL_APPLIED" runat="server" Text="<%$Resources:Resource,wfb2di_lb_Applied%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_ACTUAL_APPLIED" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ACTUAL_OVERTIME_MANAGE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_Overtime_manage%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_ACTUAL_OVERTIME_MANAGE" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:Label></td>
                    </tr>

                    <tr>
                        <td align="left" class="Body_label"></td>
                        <td align="left" class="Body_label"></td>
                        <td align="right" class="Body_label">
                            <asp:Label ID="lb_APPLINGTOTALTIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_ApplyingTotaltime%>"></asp:Label>:</td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLING_WEEKDAYS_OVERTIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_Weekdays_Overtime%>"></asp:Label>:</th>

                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_APPLING_WEEKDAYS_OVERTIME" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLING_EXCHANGED" runat="server" Text="<%$Resources:Resource,wfb2di_lb_Exchanged%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_APPLING_EXCHANGED" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:Label>
                        </td>
                        <td align="left" class="Body_label"></td>
                        <td align="left" class="Body_label"></td>
                    </tr>

                    <tr>
                        <td align="left" class="Body_label"></td>
                        <td align="left" class="Body_label"></td>
                        <td align="right" class="Body_label"></td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLYING_HOLIDAY_OVERTIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_Holiday_Overtime%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_APPLYING_HOLIDAY_OVERTIME" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:Label>
                        </td>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLING_APPLIED" runat="server" Text="<%$Resources:Resource,wfb2di_lb_Applied%>"></asp:Label>:</th>

                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_APPLING_APPLIED" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLING_OVERTIME_MANAGE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_Overtime_manage%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                            <asp:Label ID="txt_APPLING_OVERTIME_MANAGE" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:Label>
                            </td>
                    </tr>

                </tbody>
            </table>
        </div>
        <div align="right" class='pos_top'>
        </div>
        <span>
            <br/>
        </span>
        <div id="editPage1">
            <div id="gridList1" style="width: 965px;">
            </div>
        </div>
</asp:Content>


