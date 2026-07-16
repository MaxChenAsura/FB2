<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hc/WFB2HC0100_EffectProc.aspx.cs" Inherits="WebContent_WFB2HC0100_EffectProc" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();                        
            if ($("#gv_result").length == 0)
                setinit_grid(false);
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date2").mask('9999/99');            

            gridviewScroll();
            $.unblockUI();
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F",
                headerrowcount: 2,
                freezesize: 8
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }


        function setinit_grid(display) {
            if (display)
                $("#init_grid").show();
            else
                $("#init_grid").hide();
        }

        //一括更新的檢查
        function checkConfirm() {
            var processed = false;
            if (LookUpCheckboxs() == 0) {
                alert("請選擇資料");
                return false;
            }
            var result = confirm("確定要執行?");
            if (result) {
                BlockUI();
                processed=true;
            }
            if (!processed)
                $.unblockUI();
            return processed;
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hid_EMP_ID_search" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hid_START_SDT_search" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hid_START_EDT_search" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hid_HR_CHG_CD_search" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hid_HR_CHG_PROC_STATUS_search" ClientIDMode="Static" runat="server" />
            <div style="height:10px"></div>
            <div><asp:Label ID="lb_topic" runat="server"></asp:Label></div>
             <div style="height:10px"></div>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData_EffectProc"
                SelectCountMethod="getCount_EffectProc" TypeName="CFB2HC0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />                    
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1020px"
                OnRowCreated="gv_result_RowCreated"
                OnPageIndexChanging="gv_result_PageIndexChanging">
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
                    <asp:BoundField DataField="HR_CHG_NO" HeaderText="<%$Resources:Resource,wfb2hc_hd_HR_CHG_NO%>" ItemStyle-Width="130px" SortExpression="HR_CHG_NO" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="HR_CHG_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_HR_CHG_CD_DESC%>" ItemStyle-Width="150px" SortExpression="HR_CHG_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_START_DT1%>" ItemStyle-Width="100px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="START_DT" />
                    <asp:BoundField DataField="INS_PLAN_PROC_DT" HeaderText="<%$Resources:Resource,wfb2hc_lb_INS_PLAN_PROC_DT%>" ItemStyle-Width="120px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="INS_PLAN_PROC_DT" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2hc_hd_EMP_ID%>" ItemStyle-Width="70px" SortExpression="EMP_ID" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2hc_hd_EMP_NAME%>" ItemStyle-Width="80px" SortExpression="EMP_NAME" ItemStyle-HorizontalAlign="Left" />                    
                    <asp:BoundField DataField="AF_DEPT_NO_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_DEPT_NO_DESC%>" ItemStyle-Width="300px" SortExpression="AF_DEPT_NO_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_LEVEL_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_LEVEL_CD_DESC%>" ItemStyle-Width="40px" SortExpression="AF_LEVEL_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_GRADE_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_GRADE_CD_DESC%>" ItemStyle-Width="40px" SortExpression="AF_GRADE_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_PJOB_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_PJOB_CD_DESC%>" ItemStyle-Width="120px" SortExpression="AF_PJOB_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_EMP_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_EMP_CD_DESC%>" ItemStyle-Width="80px" SortExpression="AF_EMP_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_WS_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_WS_CD_DESC%>" ItemStyle-Width="40px" SortExpression="AF_WS_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_COMPANY_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_COMPANY_CD_DESC%>" ItemStyle-Width="60px" SortExpression="AF_COMPANY_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_PLANT_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_PLANT_CD_DESC%>" ItemStyle-Width="60px" SortExpression="AF_PLANT_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_WORK_SHIFT_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_WORK_SHIFT_CD_DESC%>" ItemStyle-Width="150px" SortExpression="AF_WORK_SHIFT_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_WORK_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_WORK_CD_DESC%>" ItemStyle-Width="60px" SortExpression="AF_WORK_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <div style="width:1018px; text-align: right;">
                <aces:Btn ID="WFB2HC0102Save" runat="server" Text="<%$Resources:Resource,btn_Execute%>" OnClientClick="return checkConfirm();" OnClick="WFB2HC0102Save_Click" />

                <%--<asp:Button ID="WFB2HC0102Save" runat="server" Text="<%$Resources:Resource,btn_Execute%>" OnClientClick="BlockUI();" OnClick="WFB2HC0102Save_Click" />--%>
                
                <asp:Button ID="WFB2HC0102Cancel" runat="server" Text="<%$Resources:Resource,btn_cancel%>"  OnClick="WFB2HC0102Cancel_Click" />
            </div>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

