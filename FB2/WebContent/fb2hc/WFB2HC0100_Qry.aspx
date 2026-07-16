<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hc/WFB2HC0100_Qry.aspx.cs" Inherits="WebContent_WFB2HC0100_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();                        
            //if ($("#gv_result").length == 0)
            //    setinit_grid(false);
        });

        function setReadOnly(val) {
            //$("#txt_EMP_ID_search").attr("ReadOnly", val);
            $("#txt_EMP_NAME_search").attr("ReadOnly", val);
            //$("#txt_HR_CHG_CD_search").attr("ReadOnly", val);
            $("#txt_HR_CHG_CD_DESC_search").attr("ReadOnly", val);
        }

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date2").mask('9999/99');

            $('#txt_EMP_ID_search').blur(function () {
                if ($('#txt_EMP_ID_search').val() != "") {
                    $.ajax({
                        url: "WFB2HC0100_DataProc.ashx",
                        data: {
                            DATA_TYPE: "Qry_GET_EMP_NAME",
                            EMP_ID: $('#txt_EMP_ID_search').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        async: false,
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "" && JData.errMsg != null) {
                                $('#txt_EMP_NAME_search').val("");                                
                                alert(JData.errMsg);
                            }
                            else if (JData.bolSTATUS) {
                                $('#txt_EMP_NAME_search').val(JData.strEMP_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME_search').val("");
                }
            });

            $('#txt_HR_CHG_CD_search').blur(function () {
                if ($('#txt_HR_CHG_CD_search').val() != "") {
                    $.ajax({
                        url: "WFB2HC0100_DataProc.ashx",
                        data: {
                            DATA_TYPE: "Qry_GET_HR_CHG_DESC",
                            HR_CHG_CD: $('#txt_HR_CHG_CD_search').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        async: false,
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "" && JData.errMsg != null) {
                                $('#txt_HR_CHG_CD_DESC_search').val("");
                                alert(JData.errMsg);
                            }
                            else if (JData.bolSTATUS) {
                                $('#txt_HR_CHG_CD_DESC_search').val(JData.strHR_CHG_DESC);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_HR_CHG_CD_DESC_search').val("");
                }
            });

            setReadOnly(true);
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
                freezesize: 5
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID_search").val("");
            $("#txt_EMP_NAME_search").val("");
            $("#txt_START_SDT_search").val("");
            $("#txt_START_EDT_search").val("");
            $("#ddl_HR_CHG_CD").val("");
            //$("#txt_HR_CHG_CD_search").val("");
            //$("#txt_HR_CHG_CD_DESC_search").val("");
            $("#rb_HR_CHG_PROC_STATUS_A_search").prop('checked', true);
            $("#hid_EMP_ID_search").val("");
            $("#hid_EMP_NAME_search").val("");
            $("#hid_START_SDT_search").val("");
            $("#hid_START_EDT_search").val("");
            $("#hid_HR_CHG_CD_search").val("");
            $("#hid_HR_CHG_CD_DESC_search").val("");
            $("#hid_HR_CHG_PROC_STATUS_search").val("");
        }

        function IsDelete() {
            var j = 0;
            elm = document.forms[0];
            for (i = 0; i <= elm.length - 1; i++) {

                if (elm[i].type == "checkbox" && elm[i].id.substr(elm[i].id.length - 8, 18) == 'cb_check') {

                    if (elm.elements[i].checked == true)
                        j++;
                }
            }
            if (j == 0) {
                alert($("#hid_wfb2hc_CheckBox_NotChoiceMessage").val());
                return false;
            }
                
            var answer = confirm($("#hid_wfb2hc_Delete_Confirm_Message").val());
            if (answer) {
                BlockUI();
                return true;
            }
            else
                return false;
        }
        function setinit_grid(display) {
            if (display)
                $("#init_grid").show();
            else
                $("#init_grid").hide();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">
                <tr valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
	                    <colgroup>                                 	
		                    <col width="11%" />
                            <col width="25%" />
		                    <col width="11%" />
		                    <col width="53%" />
	                    </colgroup>
                        <tbody>
                            <tr>
                                <td height="10px"></td>
                            </tr>                                	
		                    <tr>
                                <%--工號--%>		
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_EMP_ID%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label" >
									<asp:TextBox ID="txt_EMP_ID_search" runat="server" MaxLength="5" size="5" ClientIDMode="Static"></asp:TextBox>
                                    <asp:Button ID="btn_EMP_ID" runat="server" Text="..." />
                                    <asp:TextBox ID="txt_EMP_NAME_search" runat="server" size="10" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
								</td>
                                <%--異動生效日--%>
								<th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_START_DT1%>"></asp:Label>:
								</th>
                                <td align="left" class="Body_label" >
                                    <asp:TextBox ID="txt_START_SDT_search" runat="server" MaxLength="10" size="10" ClientIDMode="Static" CssClass="date"></asp:TextBox>
									～
                                    <asp:TextBox ID="txt_START_EDT_search" runat="server" MaxLength="10" size="10" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                    <asp:CustomValidator ID="cv_START_SDT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hc_START_SDT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_SDT_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cv_START_EDT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hc_START_EDT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_EDT_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    <asp:CompareValidator ID="cv_START_DT" runat="server" ControlToCompare="txt_START_SDT_search" ControlToValidate="txt_START_EDT_search" ErrorMessage="<%$Resources:Resource,wfb2hc_START_DT_Compare_Error%>" Type="Date" Operator="GreaterThanEqual"
                                        Display="None"  ValidationGroup="GroupA" ForeColor="Red"></asp:CompareValidator>									
								</td>
							</tr>
							<tr>
                                <%--人事異動代碼--%>
                            	<th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_HR_CHG_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_HR_CHG_CD%>"></asp:Label>:
                            	</th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddl_HR_CHG_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    <%--<asp:TextBox ID="txt_HR_CHG_CD_search" runat="server" MaxLength="3" size="5" ClientIDMode="Static"></asp:TextBox>                                    
                                    <asp:Button ID="btn_HR_CHG_CD" runat="server" Text="..." />
                                    <asp:TextBox ID="txt_HR_CHG_CD_DESC_search" runat="server" size="15" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>--%>
								</td>
                                <%--生效處理狀態--%>
								<th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_HR_CHG_PROC_STATUS" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_HR_CHG_PROC_STATUS%>"></asp:Label>:
								</th>
                                <td align="left" class="Body_label" colspan="3">
                                    <asp:RadioButton ID="rb_HR_CHG_PROC_STATUS_Y_search" name="rb_HR_CHG_PROC_STATUS_search" runat="server" GroupName="rbg_HR_CHG_PROC_STATUS" Text="<%$Resources:Resource,wfb2hc_lb_HR_CHG_PROC_STATUS_Y%>" ClientIDMode="Static" />
                                    <asp:RadioButton ID="rb_HR_CHG_PROC_STATUS_N_search" name="rb_HR_CHG_PROC_STATUS_search" runat="server" GroupName="rbg_HR_CHG_PROC_STATUS" Text="<%$Resources:Resource,wfb2hc_lb_HR_CHG_PROC_STATUS_N%>" ClientIDMode="Static" />
                                    <asp:RadioButton ID="rb_HR_CHG_PROC_STATUS_E_search" name="rb_HR_CHG_PROC_STATUS_search" runat="server" GroupName="rbg_HR_CHG_PROC_STATUS" Text="<%$Resources:Resource,wfb2hc_lb_HR_CHG_PROC_STATUS_E%>" ClientIDMode="Static" />
                                    <asp:RadioButton ID="rb_HR_CHG_PROC_STATUS_A_search" name="rb_HR_CHG_PROC_STATUS_search" runat="server" GroupName="rbg_HR_CHG_PROC_STATUS" Text="<%$Resources:Resource,wfb2hc_lb_HR_CHG_PROC_STATUS_A%>" Checked="true" ClientIDMode="Static" />
								</td>
							</tr>
 							<tr>
                                <td align="right" class="Body_label" colspan="10">
                                    <div id="init">
                                        <aces:Btn ID="WFB2HC0100Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="btn_search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                        
                                        <%--<asp:Button ID="WFB2HC0100Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="btn_search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                                        
                                        <asp:Button ID="WFB2HC0100Clear" runat="server" type="button" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="ClearAll();return false;" />
                                        
                                        <aces:Btn ID="WFB2HC0100EffectProc" runat="server" type="button" Text="<%$Resources:Resource,wfb2hc_WFB2HC0100EffectProc%>" OnClick="WFB2HC0100EffectProc_Click" />
                                        
                                        <%--<asp:Button ID="WFB2HC0100EffectProc" runat="server" type="button" Text="<%$Resources:Resource,wfb2hc_WFB2HC0100EffectProc%>" OnClick="WFB2HC0100EffectProc_Click" />--%>
                                        
                                        <asp:HiddenField ID="hid_EMP_ID_search" ClientIDMode="Static" runat="server" />
                                        <asp:HiddenField ID="hid_START_SDT_search" ClientIDMode="Static" runat="server" />
                                        <asp:HiddenField ID="hid_START_EDT_search" ClientIDMode="Static" runat="server" />
                                        <asp:HiddenField ID="hid_HR_CHG_CD_search" ClientIDMode="Static" runat="server" />
                                        <asp:HiddenField ID="hid_HR_CHG_PROC_STATUS_search" ClientIDMode="Static" runat="server" />                                        
                                        <asp:HiddenField ID="hid_wfb2hc_CheckBox_NotChoiceMessage" ClientIDMode="Static" runat="server" /> 
                                        <asp:HiddenField ID="hid_wfb2hc_Delete_Confirm_Message" ClientIDMode="Static" runat="server" /> 
                                    </div>
                                </td>
                            </tr>
 					        <tr>
						        <td align="center" height="1" colspan=10> <hr> </td>
					        </tr> 
					        <tr>
						        <td align="right" colspan="10">
							        <div id="init_add"  style="display:inline">
                                        <aces:Btn ID="WFB2HC0100Add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="btn_add_Click" />

                                        <%--<asp:Button ID="WFB2HC0100Add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="btn_add_Click" />--%>
						            </div>    
							        <div id="init_grid"   style="display:inline">
                                    <aces:Btn ID="WFB2HC0100Delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClientClick="return IsDelete();" OnClick="btn_delete_Click" />
                                        <aces:Btn ID="WFB2HC0100Edit" runat="server" Text="<%$Resources:Resource,btn_edit%>" OnClick="btn_edit_Click" />
								        <aces:Btn ID="WFB2HC0100Detail" runat="server" Text="<%$Resources:Resource,wfb2hc_WFB2HC0100Detail%>" OnClick="WFB2HC0100Detail_Click" />                                        

<%--                                        <asp:Button ID="WFB2HC0100Delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClientClick="return IsDelete();" OnClick="btn_delete_Click" />
                                        <asp:Button ID="WFB2HC0100Edit" runat="server" Text="<%$Resources:Resource,btn_edit%>" OnClick="btn_edit_Click" />
								        <asp:Button ID="WFB2HC0100Detail" runat="server" Text="<%$Resources:Resource,wfb2hc_WFB2HC0100Detail%>" OnClick="WFB2HC0100Detail_Click" />                                        --%>
							        </div>
                                    <aces:Btn ID="WFB2HC0100AddBatch" runat="server" Text="<%$Resources:Resource,wfb2hc_WFB2HC0100AddBatch%>" OnClick="WFB2HC0100AddBatch_Click" />

                                    <%--<asp:Button ID="WFB2HC0100AddBatch" runat="server" Text="<%$Resources:Resource,wfb2hc_WFB2HC0100AddBatch%>" OnClick="WFB2HC0100AddBatch_Click" />--%>
						        </td>
					        </tr>									
                        </tbody>
                        </table>
                    </td>
                </tr>                
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HC0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_EMP_ID_search"
                        Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_START_SDT_search"
                        Name="start_sdt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_START_EDT_search"
                        Name="start_edt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_HR_CHG_CD"
                        Name="hr_chg_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_HR_CHG_PROC_STATUS_search"
                        Name="hr_chg_proc_status" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
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
                            <asp:TextBox ID="hid_HR_CHG_PROC_STATUS" ClientIDMode="Static" runat="server" Text='<%#Bind("HR_CHG_PROC_STATUS")%>' style="display:none;" />
                            <asp:TextBox ID="hid_INS_CHG_PROC_STATUS" ClientIDMode="Static" runat="server" Text='<%#Bind("INS_CHG_PROC_STATUS")%>' style="display:none;" />                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hc_hd_RowNumber%>" ItemStyle-Width="40px" />
                    <asp:BoundField DataField="HR_CHG_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_HR_CHG_CD_DESC%>" ItemStyle-Width="150px" SortExpression="HR_CHG_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_START_DT1%>" ItemStyle-Width="100px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="START_DT" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2hc_hd_EMP_ID%>" ItemStyle-Width="70px" SortExpression="EMP_ID" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2hc_hd_EMP_NAME%>" ItemStyle-Width="80px" SortExpression="EMP_NAME" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="HR_CHG_PROC_STATUS_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_HR_CHG_PROC_STATUS_DESC%>" ItemStyle-Width="110px" SortExpression="HR_CHG_PROC_STATUS_DESC" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="AF_DEPT_NO_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_DEPT_NO_DESC%>" ItemStyle-Width="300px" SortExpression="AF_DEPT_NO_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_LEVEL_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_LEVEL_CD_DESC%>" ItemStyle-Width="40px" SortExpression="AF_LEVEL_CD_DESC" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="AF_GRADE_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_GRADE_CD_DESC%>" ItemStyle-Width="40px" SortExpression="AF_GRADE_CD_DESC" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="AF_PJOB_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_PJOB_CD_DESC%>" ItemStyle-Width="120px" SortExpression="AF_PJOB_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_EMP_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_EMP_CD_DESC%>" ItemStyle-Width="80px" SortExpression="AF_EMP_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_WS_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_WS_CD_DESC%>" ItemStyle-Width="40px" SortExpression="AF_WS_CD_DESC" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="AF_COMPANY_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_COMPANY_CD_DESC%>" ItemStyle-Width="100px" SortExpression="AF_COMPANY_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_PLANT_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_PLANT_CD_DESC%>" ItemStyle-Width="60px" SortExpression="AF_PLANT_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_WORK_SHIFT_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_WORK_SHIFT_CD_DESC%>" ItemStyle-Width="150px" SortExpression="AF_WORK_SHIFT_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="AF_WORK_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_AF_WORK_CD_DESC%>" ItemStyle-Width="60px" SortExpression="AF_WORK_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <%-- 人事異動編號 --%>
                    <asp:BoundField DataField="HR_CHG_NO" HeaderText="<%$Resources:Resource,wfb2hc_hd_HR_CHG_NO%>" ItemStyle-Width="130px" SortExpression="HR_CHG_NO" ItemStyle-HorizontalAlign="Left" />
                    <%-- IFLOW單號號 --%>
                    <asp:BoundField DataField="IFLOW_NO" HeaderText="<%$Resources:Resource,wfb2hc_lb_IFLOW_NO%>" ItemStyle-Width="160px" SortExpression="IFLOW_NO" ItemStyle-HorizontalAlign="Left" />
                    <%-- 入力人員 --%>
                    <asp:BoundField DataField="CREATED_NAME" HeaderText="<%$Resources:Resource,wfb2hc_hd_CREATED_NAME%>" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                    <%-- 入力日期 --%>
                    <asp:BoundField DataField="CREATED_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_CREATED_DT%>" ItemStyle-Width="100px" SortExpression="CREATED_DT" ItemStyle-HorizontalAlign="Center" />
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

