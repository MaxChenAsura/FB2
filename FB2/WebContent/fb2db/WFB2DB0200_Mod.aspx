<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2db/WFB2DB0200_Mod.aspx.cs" Inherits="WebContent_fb2da_WFB2DB0200_Mod" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Src="~/UserControl/UCCommCodeDropDwonList.ascx" TagPrefix="uc1" TagName="UCCommCodeDropDwonList" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $.unblockUI();
        }

        function Valid() {
            var Message = "";
            if ($("#ddl_SHIFT_CD").val() == "") {
                Message += $("#hid_wfb2db_SHIT_CDNotNull").val();
            }
            if (Message == "") {
                if (confirm($("#hid_Save_Confirm").val())) {
                    return true;
                } else {
                    $.unblockUI();
                    return false;
                }
            }
            else {
                alert(Message);
                $.unblockUI();
                return false;
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
                                <%-- 工號 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_EMP_ID" runat="server" Text='<%$Resources:Resource,wfb2db_EMP_ID%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:Label ID="lb_EMP_ID_Value" runat="server" ClientIDMode="Static" />
                                </td>
                                <%-- 姓名 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%$Resources:Resource,wfb2db_EMP_NAME%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label" colspan="3">
                                    <asp:Label ID="lb_EMP_NAME_Value" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <%-- 工廠區分 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_PLANT" runat="server" Text='<%$Resources:Resource,wfb2db_PLANT%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:Label ID="lb_PLANT_Value" runat="server" ClientIDMode="Static" />
                                </td>
                                <%-- 部門 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%$Resources:Resource,wfb2db_DEPT_NO%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label" colspan="3">
                                    <asp:Label ID="lb_DEPT_NO_Value" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <%-- 勤務日期 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_CALENDAR_DT" runat="server" Text='<%$Resources:Resource,wfb2db_CALENDAR_DT%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:Label ID="lb_CALENDAR_DT_Value" runat="server" ClientIDMode="Static" />
                                </td>
                                 <%-- 出勤別 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_WORK_DAY" runat="server" Text='<%$Resources:Resource,wfb2db_WORK_DAY%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label">
                                     <asp:DropDownList ID="ddl_WORK_DAY_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                                </td>
                                <%-- 輪值表 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_WORK_SHIFT" runat="server" Text='<%$Resources:Resource,wfd2db_WORK_SHIFT%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:Label ID="lb_WORK_SHIFT_Value" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <%-- 班別 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_SHIFT_CD" runat="server" Text='<%$Resources:Resource,wfb2db_SHIFT_CD%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddl_SHIFT_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                                    <%--<asp:TextBox ID="txt_SHIFT_CD_Value" Width="30" AutoPostBack="true" OnTextChanged="txt_SHIFT_CD_Value_TextChanged" runat="server" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="2" />
                                    <asp:Button ID="btn_SHIFT_CD" runat="server" ClientIDMode="Static" Text="..." />
                                    <asp:TextBox ID="txt_SHIFT_DESC_Value" runat="server" ClientIDMode="Static" CssClass="MandatoryField" BorderWidth="0" Enabled="false" Style="background-color: white; color: black;" />--%>
                                </td>
                                <%-- 時段別區分 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_SHIFT_TIME" runat="server" Text='<%$Resources:Resource,wfb2db_SHIFT_TIME%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label" colspan="3">
                                    <uc1:UCCommCodeDropDwonList runat="server" Enabled="false"
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
                                        Require="true" />
                                </td>
                            </tr>
                            <tr>
                                <%-- 工作時數 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_WorkHour" runat="server" Text='<%$Resources:Resource,wfd2db_WorkHour%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:Label ID="lb_WorkHour_Value" runat="server" ClientIDMode="Static" />
                                </td>
                                <%-- 在廠時數 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_InCompanyHour" runat="server" Text='<%$Resources:Resource,wfd2db_InCompanyHour%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label" colspan="3">
                                    <asp:Label ID="lb_InCompanyHour_Value" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <%-- 上班時段 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lblDUTY_TIME" runat="server" Text='<%$Resources:Resource,wfb2da_lb_DUTY_TIME_Add%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label" colspan="5">
                                    <asp:DropDownList ID="ddlDUTY_TIME_HH_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlDUTY_TIME_MM_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    ~
                                    <asp:DropDownList ID="ddlDUTY_TIME_HH_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlDUTY_TIME_MM_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <%-- 勤前用餐 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbMealTime1" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime1%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlMealTime1_HH_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime1_MM_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    ~
                            <asp:DropDownList ID="ddlMealTime1_HH_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime1_MM_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                </td>
                                <%-- 勤前休息 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbMealTime1Reset" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime1Reset%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label" colspan="3">
                                    <asp:DropDownList ID="ddlMealTime1Reset_HH_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime1Reset_MM_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime1Reset_HH_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime1Reset_MM_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <%-- 勤中休息(二) --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbMealTime2" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime2%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label" colspan="5">
                                    <asp:DropDownList ID="ddlMealTime2_HH_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime2_MM_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime2_HH_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime2_MM_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <%-- 勤中休息(一) --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbMealTime2Reset1" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime2Reset1%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlMealTime2Reset1_HH_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime2Reset1_MM_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime2Reset1_HH_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime2Reset1_MM_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                </td>
                                <%-- 勤中休息(二) --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbMealTime2Reset2" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime2Reset2%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlMealTime2Reset2_HH_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime2Reset2_MM_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime2Reset2_HH_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime2Reset2_MM_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                </td>
                                <%-- 勤中休息(三) --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbMealTime2Reset3" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime2Reset3%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlMealTime2Reset3_HH_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime2Reset3_MM_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime2Reset3_HH_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime2Reset3_MM_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <%-- 勤後用餐 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="Label1" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime3%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlMealTime3_HH_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime3_MM_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime3_HH_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime3_MM_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                </td>
                                <%-- 勤後休息(一) --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="Label2" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime3Reset1%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlMealTime3Reset1_HH_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime3Reset1_MM_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime3Reset1_HH_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime3Reset1_MM_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                </td>
                                <%-- 勤後休息(二) --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="Label3" runat="server" Text='<%$Resources:Resource,wf2da_lbMealTime3Reset2%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddlMealTime3Reset2_HH_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime3Reset2_MM_S" runat="server" ClientIDMode="Static" Enabled="false" />
                                    ~
                                    <asp:DropDownList ID="ddlMealTime3Reset2_HH_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                    <asp:DropDownList ID="ddlMealTime3Reset2_MM_E" runat="server" ClientIDMode="Static" Enabled="false" />
                                </td>

                            </tr>
                            <tr>
                                <%-- 輪班津貼 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_WORK_SHIFT_ALLOWANCE_TYPE" runat="server" Text='<%$Resources:Resource,wfb2db_WORK_SHIFT_ALLOWANCE_TYPE%>' ClientIDMode="Static" />:
                                </th>
                                <td align="left" class="Body_label" colspan="5">
                                    <uc1:UCCommCodeDropDwonList runat="server" Enabled="false"
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
                            <tr>
                                <td align="right" class="Body_label" colspan="6">
                                    <div id="init_grid" style="margin-bottom: 7px;">
                                        <aces:Btn ID="WFB2DB0200Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Save%>" OnClick="WFB2DB0200Save_Click" OnClientClick="return Valid();" ClientIDMode="Static" />

                                        <%--儲存--%>
                                        <%--<asp:Button ID="WFB2DB0200Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Save%>" OnClick="WFB2DB0200Save_Click" OnClientClick="return Valid();" ClientIDMode="Static" />--%>
                                        <asp:Button ID="WFB2DB0200Cancel" runat="server" Text="<%$Resources:Resource,wfb2da_Cancel%>" ClientIDMode="Static" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hid_Save_Confirm" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2db_SHIT_CDNotNull" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfd2da_Error_SHIFT_CD" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfd2da_Error_SHIFT_CD%>" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DB0200Save" />
            <%--<asp:PostBackTrigger ControlID="txt_SHIFT_CD_Value" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
