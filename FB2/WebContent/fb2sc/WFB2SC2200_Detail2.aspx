<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2200_Detail2.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2200_Detail2" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            gridviewScroll();
            $.unblockUI();
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            
        }
        function ClearAll() {
            $('#txt_SALARY_YM_search').val("");
            $('#txt_SALARY_DT_search').val("");
            $('#txt_EMP_ID_search').val("");
            $('#txt_EMP_NAME_search').val("");
            return false;
        }
        function CheckDetail1Action() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2sc_Detail1_Choice_Not_Equal_1_Message').val());
                return false;
            }
        }

        function CheckDetail2Action() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2sc_Detail2_Choice_Not_Equal_1_Message').val());
                return false;
            }
        }  

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function CheckORDER_SEQ(source, arguments) {
            var re = /^\+?[0-9][0-9]*$/;
            if (!re.test($("#txt_ORDER_SEQ_Add").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }
        function proc_data() {
            var txt_SALARY_YM_search = $("#txt_SALARY_YM_search").val();
            $("#tmp_SALARY_YM_search").val("");
            if (txt_SALARY_YM_search != "")
                $("#tmp_SALARY_YM_search").val(txt_SALARY_YM_search.substring(0, 4) + '/' + txt_SALARY_YM_search.substring(4, 6) + '/01');
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hid_SALARY_DT_search" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hid_EMP_ID_search" ClientIDMode="Static" runat="server" />
            <TABLE cellSpacing="1" cellPadding="1" width="1020" border="0" class="Body_Label">
								<COLGROUP>
									<COL width="15%" />
									<COL width="15%" />
									<COL width="15%" />
									<COL width="15%" />
									<COL width="40%" />
								</COLGROUP>
								<TBODY>
									<!-- START: 1st Line in Search Criteria area -->
									<tr>
										<th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_YM%>"></asp:Label>
                                            :</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_SALARY_YM" runat="server" disabled="disabled" maxlength="15" size="10" tabindex="4" />
                                        </td>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>"></asp:Label>
                                            :</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_SALARY_DT" runat="server" disabled="disabled" maxlength="15" size="10" tabindex="4" />
                                        </td>
                                        <td align="left" class="Body_label"></td>
                                        <td align="left" class="Body_label"></td>
                                        <td align="left" class="Body_label"></td>
                                        <td align="left" class="Body_label"></td>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_ID%>"></asp:Label>
                                            :</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_EMP_ID" runat="server" disabled="disabled" maxlength="15" size="10" tabindex="4" />
                                        </td>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_NAME%>"></asp:Label>
                                            :</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_EMP_NAME" runat="server" disabled="disabled" maxlength="15" size="20" tabindex="4" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_DUTY_SDT_TO_EDT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_DUTY_SDT_TO_EDT%>"></asp:Label>
                                            :</th>
                                        <td align="left" class="Body_label" colspan="3">
                                            <asp:TextBox ID="txt_DUTY_SDT" runat="server" disabled="disabled" maxlength="15" size="10" tabindex="4" />
                                            ~
                                            <asp:TextBox ID="txt_DUTY_EDT" runat="server" disabled="disabled" maxlength="15" size="10" tabindex="4" />
                                        </td>
                                        <td align="left" class="Body_label"></td>
                                        <td align="left" class="Body_label"></td>
                                        <td align="left" class="Body_label"></td>
                                        <td align="left" class="Body_label"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <hr></hr>
                                        </td>
                                    </tr>
                                    <!-- end: Create MODULE ID -->								
								</TBODY>
							</Table>

		</div>
		<table border="1">
			<tr>
				<td rowspan="2" valign='top'>
				<asp:Label ID="lb_OVERTIME_RESULT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_OVERTIME_RESULT%>"></asp:Label>:
					<div valign='top' id="gridList1" style="width:330px; ">
                        <asp:GridView ID="gv_result_overtime" runat="server" CssClass="grid-view"
                            AutoGenerateColumns="False" OnRowDataBound="gv_result_RowDataBound" Width="329px">
                            <Columns>                                
                                <asp:BoundField DataField="OVERTIME_PAY_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2sc_hd_OVERTIME_PAY_TYPE_DESC%>" ItemStyle-Width="60%" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="TOTAL_HOURS" HeaderText="<%$Resources:Resource,wfb2sc_hd_OVERTIME_PAY_TYPE_TOTAL_HOURS%>" ItemStyle-Width="40%" ItemStyle-HorizontalAlign="Right" />                                
                            </Columns>                
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle CssClass="GridviewScrollPager" />
                        </asp:GridView>
					</div>
				</td>
				<td rowspan="2" valign='top' >
				<asp:Label ID="lb_LEAVE_RESULT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_LEAVE_RESULT%>"></asp:Label>:
					<div valign='top' id="gridList2" style="width:330px; ">
                        <asp:GridView ID="gv_result_leave" runat="server" CssClass="grid-view"
                            AutoGenerateColumns="False" OnRowDataBound="gv_result_RowDataBound" Width="329px">
                            <Columns>                                
                                <asp:BoundField DataField="SUB_LEAVE_DESC" HeaderText="<%$Resources:Resource,wfb2sc_hd_SUB_LEAVE_DESC%>" ItemStyle-Width="60%" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="TOTAL_HOURS" HeaderText="<%$Resources:Resource,wfb2sc_hd_SUB_LEAVE_TOTAL_HOURS%>" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="LEAVE_CNT_UNIT_DESC" HeaderText="<%$Resources:Resource,wfb2sc_hd_LEAVE_CNT_UNIT_DESC%>" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left" />
                            </Columns>                
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle CssClass="GridviewScrollPager" />
                        </asp:GridView>
					</div>
				</td>
				<td valign='top'>
				<asp:Label ID="lb_WORK_SHIFT_ALLOWANCE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_WORK_SHIFT_ALLOWANCE%>"></asp:Label>:
					<div  id="gridList3" style="width:338px; ">
                        <asp:GridView ID="gv_result_work" runat="server" CssClass="grid-view"
                            AutoGenerateColumns="False" OnRowDataBound="gv_result_RowDataBound" Width="337px">
                            <Columns>                                
                                <asp:BoundField DataField="WORK_SHIFT_ALLOWANCE_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2sc_hd_WORK_SHIFT_ALLOWANCE_TYPE_DESC%>" ItemStyle-Width="60%" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="TOTAL_DAYS" HeaderText="<%$Resources:Resource,wfb2sc_hd_WORK_SHIFT_ALLOWANCE_TYPE_TOTAL_DAYS%>" ItemStyle-Width="40%" ItemStyle-HorizontalAlign="Right" />                                
                            </Columns>                
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle CssClass="GridviewScrollPager" />
                        </asp:GridView>
				</div>
				</td>
			</tr>
			<%--<tr>
				<td>
				環境津貼明細:
					<div id="gridList4" style="width:300px; "></div>
				</td>
			</tr>--%>
			<tr>
				<td valign='top'>
				<asp:Label ID="lb_AVAILABLE_LEAVE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_AVAILABLE_LEAVE%>"></asp:Label>:
					<div id="gridList5" style="width:338px; ">
                        <asp:GridView ID="gv_result_available" runat="server" CssClass="grid-view"
                            AutoGenerateColumns="False" OnRowDataBound="gv_result_RowDataBound" Width="337px">
                            <Columns>                                
                                <asp:BoundField DataField="DATA_YEAR" HeaderText="<%$Resources:Resource,wfb2sc_hd_DATA_YEAR%>" ItemStyle-Width="15%" />
                                <asp:BoundField DataField="LEAVE_ALLOWANCE_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2sc_hd_LEAVE_ALLOWANCE_TYPE_DESC%>" ItemStyle-Width="41%" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="TOTAL_HOURS" HeaderText="<%$Resources:Resource,wfb2sc_hd_LEAVE_ALLOWANCE_TYPE_TOTAL_HOURS%>" ItemStyle-Width="22%" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="IS_YN" HeaderText="<%$Resources:Resource,wfb2sc_hd_IS_YN%>" ItemStyle-Width="22%" ItemStyle-HorizontalAlign="Left" />
                            </Columns>                
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle CssClass="GridviewScrollPager" />
                        </asp:GridView>
					</div>
				</td>
			</tr>
			
		</table>
		<div align="right" style="width:1020px;">
			<%--<input id="btn_back" type="button" value="<%$Resources:Resource,wfb2sc_btn_back%>" onclick="location.href = ('WFB2SC2200_Qry.aspx');" runat="server" />--%>
            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_back%>" OnClick="btn_back_Click" />
		</div>
        </ContentTemplate>

    </asp:UpdatePanel>
    <asp:ValidationSummary ID="vs1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>


