<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2hc/WFB2HC0401_Dtl3.aspx.cs" Inherits="WebContent_fb2hc_WFB2HC0401_Dtl3" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date2").mask('9999/99');
            $.unblockUI();
        }

        function DH0700() {
            var year = $("#txt_PAY_YM").val().substring(0, 4);
            var month = $("#txt_PAY_YM").val().substring(5, 7);
            var day =  new Date(year, month++,0).getDate();
            window.open("../fb2dh/WFB2DH0700_Qry.aspx?parentFuncId=FB2HC040&fn=FB2HC040&emp_id=" + $("#hid_EMP_ID_search").val() + "&apply_leave_sdt=" + $("#txt_START_DT").val() + "&apply_leave_edt=" + $("#txt_PAY_YM").val() + "/" + day, 'DH0700');
            return false;
        }

        function HD0100() {
            var year = $("#txt_PAY_YM").val().substring(0, 4);
            var month = $("#txt_PAY_YM").val().substring(5, 7);
            var day = new Date(year, month++, 0).getDate();
            window.open("../fb2hd/WFB2HD0100_Qry.aspx?parentFuncId=FB2HC040&fn=FB2HC040&emp_id=" + $("#hid_EMP_ID_search").val() + "&start_dt_s=" + $("#txt_START_DT").val() + "&start_dt_e=" + $("#txt_PAY_YM").val() + "/" + day, 'HD0100');
            return false;
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>            
            <table width="1020" cellspacing="3" cellpadding="0">
		        <tr>
	                <td colspan="10">
				        <table cellspacing="1" cellpadding="1" width="100%">
					        <!-- START: Search Criteria Field -->
					        <tr>
						        <td>
							        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">                            
                                        <colgroup>
									        <col width="12%"/>
                                            <col width="10%"/>
									        <col width="12%"/>
                                            <col width="18%"/>
									        <col width="12%"/>
									        <col width="13%"/>
									        <col width="23%"/>
                                        </colgroup>
                                        <tbody>
                                            <tr>
                                                <td height="10px;"></td>
                                            </tr>
								            <tr>
                                                <%--發放年月--%>
				          			            <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_PAY_YM" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_PAY_YM%>"></asp:Label>:
				          			            </th>
				          			            <td align="left" class="Body_label" > 
				          				            <asp:TextBox ID="txt_PAY_YM" runat="server" MaxLength="6" size="5" style="border:none" readonly="true" CssClass="date2" ClientIDMode="Static"></asp:TextBox>
				          			            </td>
                                                <%--發薪日期--%>
									            <th align="left"  class="Body_TableHeader">
                                                    <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_SALARY_DT%>"></asp:Label>:
									            </th>
									            <td align="left" class="Body_label">  
										            <asp:TextBox ID="txt_SALARY_DT" runat="server" MaxLength="10" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox> 
									            </td>
									            <td align="left" class="Body_label"></td>
									            <td align="left" class="Body_label"></td>
									            <td align="left" class="Body_label"></td>
								            </tr>
                                            <tr>
                                                <%--聘用單位--%>
                               		            <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_COMPANY_CD_DESC" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_COMPANY_CD%>"></asp:Label>:
                               		            </th>
                                	            <td align="left" class="Body_label" >
										            <asp:TextBox ID="txt_COMPANY_CD_DESC" runat="server" size="8" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
									            </td>
                                                <%--工號--%>
                               		            <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_EMP_ID%>"></asp:Label>:
                               		            </th>
                                	            <td align="left" class="Body_label" >
										            <asp:TextBox ID="txt_EMP_ID" runat="server" size="8" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
									            </td>
                                                <%--部門代號--%>
									            <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_ORI_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_ORI_DEPT_NO%>"></asp:Label>:
									            </th>
                                	            <td align="left" class="Body_label" colspan="3">
										            <asp:TextBox ID="txt_ORI_DEPT_NO" runat="server" size="50" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
									            </td>
                                            </tr>									
                                            <tr>
                                                <td align="center" height="1" colspan=8> <hr> </td>
                                                <td align="right" class="Body_label">
                                                    <asp:Button ID="WFB2HC0401LeaveQry" runat="server" Text="<%$Resources:Resource,wfb2hc_WFB2HC0401LeaveQry%>" OnClientClick="return DH0700();" OnClick="WFB2HC0401LeaveQry_Click" />
                                                </td>
                                                <td align="right" class="Body_label" >
                                                    <asp:Button ID="WFB2HC0401RewardQry" runat="server" Text="<%$Resources:Resource,wfb2hc_WFB2HC0401RewardQry%>" OnClientClick="return HD0100();" OnClick="WFB2HC0401RewardQry_Click" />
                                                </td>
                                                <td align="right" class="Body_label" >
                                                    <asp:Button ID="WFB2HC0401BackPage" runat="server" Text="<%$Resources:Resource,wfb2hc_btn_backPage%>" OnClick="WFB2HC0401BackPage_Click" />                                    
                                                    <div id="init">
                                                        <asp:HiddenField ID="hid_PAY_YM_search" ClientIDMode="Static" runat="server" />
                                                        <asp:HiddenField ID="hid_SALARY_DT_search" ClientIDMode="Static" runat="server" />
                                                        <asp:HiddenField ID="hid_COMPANY_CD_search" ClientIDMode="Static" runat="server" />
                                                        <asp:HiddenField ID="hid_COMPANY_CD_DESC_search" ClientIDMode="Static" runat="server" />
                                                        <asp:HiddenField ID="hid_BONUS_TYPE_search" ClientIDMode="Static" runat="server" />
                                                        <asp:HiddenField ID="hid_BONUS_TYPE_DESC_search" ClientIDMode="Static" runat="server" />
                                                        <asp:HiddenField ID="hid_MEMBER_CNT_search" ClientIDMode="Static" runat="server" />
                                                        <asp:HiddenField ID="hid_AMT_CNT_search" ClientIDMode="Static" runat="server" />
                                                        <asp:HiddenField ID="hid_EMP_ID_search" ClientIDMode="Static" runat="server" />
                                                        <asp:HiddenField ID="hid_ORI_DEPT_NO_search" ClientIDMode="Static" runat="server" />
                                                        <asp:HiddenField ID="hid_START_DT_search" ClientIDMode="Static" runat="server" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
 						            <fieldset style="padding:5px; border-color: blue;">
                                        <legend class="Body_label">
								            <font color="black" face="Calibri"><b><asp:Label ID="lb_P1_DESC" runat="server"></asp:Label></b></font>
                                        </legend>
						                <div class='pos_top'>
							                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                                                <colgroup>
									                <col width="12%"/>
                                                    <col width="10%"/>
									                <col width="12%"/>
                                                    <col width="15%"/>
									                <col width="12%"/>
									                <col width="13%"/>
									                <col width="12%"/>
                                                    <col width="14%"/>
                                                </colgroup>
                                                <tbody>
									                <tr>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_BOUNS_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_BOUNS_WORK_DAYS%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_BOUNS_WORK_DAYS" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_START_DT%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_START_DT" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_END_DT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_END_DT%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_END_DT" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_WORK_DAYS%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_WORK_DAYS" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
                                	                </tr>
                                                </tbody>
                                            </table>
						                </div>
						            </fieldset>
 
							        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
								        <tr><td><br></td></tr>
							        </table>
 
 						            <fieldset style="padding:5px; border-color: blue;">
                                        <legend class="Body_label">
								            <font color="black" face="Calibri"><b><asp:Label ID="lb_P2_DESC" runat="server"></asp:Label></b></font>
                                        </legend>									
						                <div class='pos_top'>
							                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                                                <colgroup>
									                <col width="12%"/>
                                                    <col width="10%"/>
									                <col width="12%"/>
                                                    <col width="15%"/>
									                <col width="12%"/>
									                <col width="13%"/>
									                <col width="12%"/>
                                                    <col width="14%"/>
                                                </colgroup>
                                                <tbody>
									                <tr>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_LEAVE_B_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_LEAVE_B_DAYS%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_LEAVE_B_DAYS" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_LEAVE_A_HRS" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_LEAVE_A_HRS%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_LEAVE_A_HRS" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_LEAVE_B_HRS" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_LEAVE_B_HRS%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_LEAVE_B_HRS" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>    
										                </td>
										                <td align="left" class="Body_label"></td>
										                <td align="left" class="Body_label"></td>
                                	                </tr>
                                                </tbody>
                                            </table>
						                </div>
						            </fieldset>
 
							        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
								        <tr><td><br></td></tr>
							        </table>
 
						            <fieldset style="padding:5px; border-color: blue;">
                                        <legend class="Body_label">
							                <font color="black" face="Calibri"><b><asp:Label ID="lb_P3_DESC" runat="server"></asp:Label></b></font>
                                        </legend>
						                <div class='pos_top'>
							                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                                                <colgroup>
									                <col width="12%"/>
                                                    <col width="10%"/>
									                <col width="12%"/>
                                                    <col width="15%"/>
									                <col width="12%"/>
									                <col width="13%"/>
									                <col width="12%"/>
                                                    <col width="14%"/>
                                                </colgroup>
                                                <tbody>
									                <tr>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_LEAVE_Q_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_LEAVE_Q_DAYS%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_LEAVE_Q_DAYS" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_LEAVE_Q_HRS" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_LEAVE_Q_HRS%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_LEAVE_Q_HRS" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <td align="left" class="Body_label"></td>
										                <td align="left" class="Body_label"></td>
										                <td align="left" class="Body_label"></td>
										                <td align="left" class="Body_label"></td>
                                	                </tr>
                                                </tbody>
                                            </table>
						                </div>
						            </fieldset>
 
							        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
								        <tr><td><br></td></tr>
							        </table>
 
						            <fieldset style="padding:5px; border-color: blue;">
                                        <legend class="Body_label">
									        <font color="black" face="Calibri"><b><asp:Label ID="lb_P4_DESC" runat="server"></asp:Label></b></font>
                                        </legend>
						                <div class='pos_top'>
							                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                                                <colgroup>
									                <col width="12%"/>
                                                    <col width="10%"/>
									                <col width="12%"/>
                                                    <col width="15%"/>
									                <col width="12%"/>
									                <col width="13%"/>
									                <col width="12%"/>
                                                    <col width="14%"/>
                                                </colgroup>
                                                <tbody>
									                <tr>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_JUDGEMENT_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_JUDGEMENT_DAYS%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_JUDGEMENT_DAYS" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_THIRD_CNT_REWARD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_THIRD_CNT_REWARD%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_THIRD_CNT_REWARD" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_SECOND_CNT_REWARD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_SECOND_CNT_REWARD%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_SECOND_CNT_REWARD" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_FIRST_CNT_REWARD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_FIRST_CNT_REWARD%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_FIRST_CNT_REWARD" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
									                </tr>
									                <tr>
										                <td align="left" class="Body_label"></td>
										                <td align="left" class="Body_label"></td>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_THIRD_CNT_PUNISH" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_THIRD_CNT_PUNISH%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_THIRD_CNT_PUNISH" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_SECOND_CNT_PUNISH" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_SECOND_CNT_PUNISH%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_SECOND_CNT_PUNISH" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_FIRST_CNT_PUNISH" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_FIRST_CNT_PUNISH%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_FIRST_CNT_PUNISH" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
                                	                </tr>
                                                </tbody>
                                            </table>
						                </div>
						            </fieldset>
 
							        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
								        <tr><td><br></td></tr>
							        </table>
 
						            <fieldset style="padding:5px; border-color: red;">
                                        <legend class="Body_label">
									        <font color="darkred" face="Calibri"><b><asp:Label ID="lb_PLAST_DESC" runat="server"></asp:Label></b></font>
                                        </legend>
						                <div class='pos_top'>
							                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                                                <colgroup>
									                <col width="12%"/>
                                                    <col width="10%"/>
									                <col width="12%"/>
                                                    <col width="15%"/>
									                <col width="12%"/>
									                <col width="13%"/>
									                <col width="12%"/>
                                                    <col width="14%"/>
                                                </colgroup>
                                                <tbody>
									                <tr>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_PLAN_BONUS_AMT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_PLAN_BONUS_AMT%>"></asp:Label>:(A)</th>
                                		                <td align="left" class="Body_label" >											
											                <asp:TextBox ID="txt_PLAN_BONUS_AMT" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_PLAN_BONUS_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_PLAN_BONUS_DAYS%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label" >											
											                <asp:TextBox ID="txt_PLAN_BONUS_DAYS" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_BASIC_SALARY" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_BASIC_SALARY%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label" >
											                <asp:TextBox ID="txt_BASIC_SALARY" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
                                	                </tr>
 
									                <tr>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_PAID_AMT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_PAID_AMT%>"></asp:Label>:(B)</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_PAID_AMT" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
                                                        <%--先發次數 --%>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_PAID_CNT%>"></asp:Label>:</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_PAID_CNT" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
										                <td align="left" class="Body_label"></td>
										                <td align="left" class="Body_label"></td>
										                <td align="left" class="Body_label"></td>
										                <td align="left" class="Body_label"></td>
                                	                </tr>
 
									                <tr>
										                <th align="left" class="Body_TableHeader"><asp:Label ID="lb_BONUS_AMT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_BONUS_AMT%>"></asp:Label>:(A-B)</th>
                                		                <td align="left" class="Body_label">											
											                <asp:TextBox ID="txt_BONUS_AMT" runat="server" size="10" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
										                </td>
                                	                </tr>
 
                                                </tbody>
                                            </table>
						                </div>
						            </fieldset>
                                </td>
                  	        </tr>
 				        </table>
			        </td>
		        </tr>		
		    <!-- START: Show only navigation buttons on search operation -->
	        </table>

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Detail1_Choice_Not_Equal_1_Message" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Detail2_Choice_Not_Equal_1_Message" />            
        </ContentTemplate>

    </asp:UpdatePanel>
    <asp:ValidationSummary ID="vs1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>


