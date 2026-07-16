<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2200_Detail1.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2200_Detail1" %>

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
            <!-- START: Required tag library for TMAP Standard (background process) -->
            <input type="hidden" name="operation" value="initiate"><input type="hidden" name="mode" value="add"><input type="hidden" name="maxPage" value="2"><input type="hidden" name="currentPage" value="1">
            <asp:HiddenField ID="hid_SALARY_TYPE_search" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hid_SALARY_YM_search" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hid_EMP_ID_search" ClientIDMode="Static" runat="server" />

            <!-- END: Required tag library for TMAP Standard (background process) -->

            <table cellspacing="3" cellpadding="0" width="1020">
                <tr>
                    <td colspan="3">
                        <table cellspacing="1" cellpadding="1" width="100%">
                            <!-- START: Search Criteria Field -->
                            <!--
					<tr>
						<td>
							<TABLE cellSpacing="1" cellPadding="1" width="100%" border="0" class="Body_Label">
								<COLGROUP>
                                 	<COL width="10%"/>
									<COL width="18%"/>
									<COL width="10%"/>
                                    <COL width="18%"/>
									<COL width="10%"/>
									<COL width="15%"/>
									<COL width="23%"/>
								</COLGROUP>
								<TBODY>

                                	<tr>
                                		<th align="left"  class="Body_TableHeader">部門代號:</th>
                                		<td align="left" class="Body_label" >
											<input type="text" name="prodDt" disabled="disabled" maxlength="15" size="10" tabindex="4" value="" 
											onKeyDown="autoMoveByEnter();" onKeyUp="autoMoveByMax(this, window.event);" >
										</td>

                                		<th align="left"  class="Body_TableHeader">部門名稱:</th>
                                		<td align="left" class="Body_label" >
											<input type="text" name="prodDt" disabled="disabled" maxlength="15" size="20" tabindex="4" value="" 
											onKeyDown="autoMoveByEnter();" onKeyUp="autoMoveByMax(this, window.event);">
										</td>
										<td align="left" class="Body_label" ></td>
										<td align="left" class="Body_label" ></td>
										<td align="left" class="Body_label" ></td>
                                	</tr>


									<tr>
										<th align="left" class="Body_TableHeader" >部門層級:</th>
                                		<td align="left" class="Body_label" >
											<input type="text" name="userNameQuery2" disabled="disabled" maxlength="20" size="10" tabindex="2" value="<All>" onkeydown="autoMoveByEnter();" onkeyup="autoMoveByMax(this, window.event);">
										</td>

										<th align="left" class="Body_TableHeader" >組織類型:</th>
                                		<td align="left" class="Body_label" >
											<input type="text" name="userNameQuery2" disabled="disabled" maxlength="20" size="10" tabindex="2" value="<All>" onkeydown="autoMoveByEnter();" onkeyup="autoMoveByMax(this, window.event);">
										</td>

										<th align="left" class="Body_TableHeader" >組織撤編:</th>
                                		<td align="left" class="Body_label" >
											<input type="text" name="userNameQuery2" disabled="disabled" maxlength="20" size="10" tabindex="2" value="<All>" onkeydown="autoMoveByEnter();" onkeyup="autoMoveByMax(this, window.event);">
										</td>
										<td align="left" class="Body_label" ></td>
									</tr>

								</TBODY>
							</Table>
						</td>
					</tr>
					



					<tr>
						<td align="center" height="1" > <hr> </td>
					</tr>
					-->

                            <!-- START: Create button for body field. -->
                            <tr>
                                <td align="right">
                                    <div id="mrm" style="DISPLAY: inline; OVERFLOW: auto; WIDTH: 974px;">
                                        <table cellspacing="1" cellpadding="1" width="100%" border="0"
                                            class="Body_Label">
                                            <colgroup>
                                                <col width="100%" />
                                            </colgroup>
                                            <tbody>
                                                <tr>
                                                    <td align="right" class="Body_label"></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <!-- END: Create button for body field. -->



                            <!--======================================================================================-->
                            <tr>
                                <td align="right">
                                    <div id="Div1" style="DISPLAY: inline; OVERFLOW: auto; WIDTH: 974px;">
                                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                                            <colgroup>
                                                <col width="12%" />
                                                <col width="10%" />
                                                <col width="12%" />
                                                <col width="10%" />
                                                <col width="15%" />
                                                <col width="10%" />
                                                <col width="15%" />

                                            </colgroup>
                                            <tbody>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_YM%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_SALARY_YM" disabled="disabled" MaxLength="15" size="10" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_SALARY_DT" disabled="disabled" MaxLength="15" size="10" />
                                                    </td>
                                                    <td align="left" class="Body_label"></td>
                                                    <td align="left" class="Body_label"></td>
                                                    <td align="left" class="Body_label"></td>
                                                    <td align="left" class="Body_label"></td>
                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_ID%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_EMP_ID" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_NAME%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_EMP_NAME" disabled="disabled" MaxLength="15" size="20" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_RELATIVES" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_RELATIVES%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_RELATIVES" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <td align="left" class="Body_label"></td>
                                                    <td align="left" class="Body_label"></td>
                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_SALARY_ACCOUNT_NO" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_ACCOUNT_NO%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_SALARY_ACCOUNT_NO" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_SALARY_EMAIL" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_EMAIL%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label" colspan="4">
                                                        <asp:TextBox runat="server" ID="txt_SALARY_EMAIL" disabled="disabled" size="76" TabIndex="4" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_FAMILY_BIRTH_DT_1" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_FAMILY_BIRTH_DT_1%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_FAMILY_BIRTH_DT_1" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_FAMILY_BIRTH_DT_2" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_FAMILY_BIRTH_DT_2%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_FAMILY_BIRTH_DT_2" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_IS_ALLOWANCE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_IS_ALLOWANCE%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_IS_ALLOWANCE" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td align="center" height="1" colspan="10">
                                                        <hr>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                                            <colgroup>
                                                <col width="12%" />
                                                <col width="10%" />
                                                <col width="12%" />
                                                <col width="10%" />
                                                <col width="15%" />
                                                <col width="10%" />
                                                <col width="15%" />

                                            </colgroup>
                                            <tbody>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_DEPT_NO%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label" colspan="5">
                                                        <asp:TextBox runat="server" ID="txt_DEPT_NO" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                        <asp:TextBox runat="server" ID="txt_DEPT_DESC" disabled="disabled" MaxLength="15" size="60" TabIndex="4" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_COMPANY_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_COMPANY_CD" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PLANT_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_PLANT_CD" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_NATION_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_NATION_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_NATION_CD" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <td align="left" class="Body_label"></td>
                                                    <td align="left" class="Body_label"></td>
                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_CD%>"></asp:Label>: </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_EMP_CD" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PJOB_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_PJOB_CD" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_WS_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_WS_CD" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <td align="left" class="Body_label"></td>
                                                    <td align="left" class="Body_label"></td>
                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_LEVEL_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_LEVEL_CD" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_GRADE_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_GRADE_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_GRADE_CD" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_JPN_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_JPN_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_JPN_CD" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <td align="left" class="Body_label"></td>
                                                    <td align="left" class="Body_label"></td>
                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_ICT_COMPANY" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_ICT_COMPANY%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_ICT_COMPANY" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <td align="left" class="Body_label"></td>
                                                    <td align="left" class="Body_label"></td>
                                                    <td align="left" class="Body_label"></td>
                                                    <td align="left" class="Body_label"></td>
                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_WORK_SHIFT_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_WORK_SHIFT_CD" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_ACC_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_ACC_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_ACC_CD" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_INCOME_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_INCOME_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_INCOME_CD" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_HOURLY_WAGE" runat="server" Text="<%$Resources:Resource,wfb2sc_HOURLY_WAGE%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                         <asp:TextBox runat="server" ID="txt_HOURLY_WAGE" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_LEVEL_PAY" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_LEVEL_PAY%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_LEVEL_PAY" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_ABILITY_PAY" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_ABILITY_PAY%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_ABILITY_PAY" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_PJOB_PAY" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PJOB_PAY%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_PJOB_PAY" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_PROFESSION_PAY" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PROFESSION_PAY%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_PROFESSION_PAY" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <td align="left" class="Body_label"></td>
                                                </tr>
                                                <tr>
                                                    <td align="center" height="1" colspan="10">
                                                        <hr>
                                                    </td>
                                                </tr>

                                            </tbody>
                                        </table>
                                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                                            <colgroup>
                                                <col width="12%" />
                                                <col width="10%" />
                                                <col width="12%" />
                                                <col width="10%" />
                                                <col width="15%" />
                                                <col width="10%" />
                                                <col width="15%" />

                                            </colgroup>
                                            <tbody>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_EMP_CHG_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_CHG_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label" colspan="6">
                                                        <asp:TextBox runat="server" ID="txt_EMP_CHG_CD" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                        <asp:Label ID="lb_EMP_CHG_CD_DESC" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_CHG_CD_DESC%>"></asp:Label>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_EMP_CHG_DATE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_CHG_DATE%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_EMP_CHG_DATE" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_JOIN_DT%>"></asp:Label>: </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_JOIN_DT" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_LEAVE_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_LEAVE_DT%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_LEAVE_DT" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <td align="left" class="Body_label"></td>
                                                    <td align="left" class="Body_label"></td>
                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_LEAVE_REASON" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_LEAVE_REASON%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_LEAVE_REASON" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_PLAN_RETENTION_EDT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PLAN_RETENTION_EDT%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_PLAN_RETENTION_EDT" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_RETENTION_EDT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_RETENTION_EDT%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_RETENTION_EDT" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <td align="left" class="Body_label"></td>
                                                    <td align="left" class="Body_label"></td>
                                                </tr>

                                                <%--                                		<tr>
                               				<th align="left" class="Body_TableHeader"><asp:Label ID="lb_GCC_SDT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_GCC_SDT%>"></asp:Label>:</th>
                                			<td align="left" class="Body_label" >
												<asp:TextBox runat="server" id="txt_GCC_SDT" disabled="disabled" maxlength="15" size="10" tabindex="4" />
											</td>
                               				<th align="left" class="Body_TableHeader"><asp:Label ID="lb_PLAN_GCC_EDT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PLAN_GCC_EDT%>"></asp:Label>:</th>
                                			<td align="left" class="Body_label" >
												<asp:TextBox runat="server" id="txt_PLAN_GCC_EDT" disabled="disabled" maxlength="15" size="10" tabindex="4" />
											</td>
											<th align="left" class="Body_TableHeader"><asp:Label ID="lb_GCC_EDT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_GCC_EDT%>"></asp:Label>:</th>
                                			<td align="left" class="Body_label" >
												<asp:TextBox runat="server" id="txt_GCC_EDT" disabled="disabled" maxlength="15" size="10" tabindex="4" />
											</td>
											
                                		</tr>
                                		<tr>
                               				<th align="left" class="Body_TableHeader"><asp:Label ID="lb_ICT_SDT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_ICT_SDT%>"></asp:Label>:</th>
                                			<td align="left" class="Body_label" >
												<asp:TextBox runat="server" id="txt_ICT_SDT" disabled="disabled" maxlength="15" size="10" tabindex="4" />
											</td>
                               				<th align="left" class="Body_TableHeader"><asp:Label ID="lb_PLAN_ICT_EDT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PLAN_ICT_EDT%>"></asp:Label>:</th>
                                			<td align="left" class="Body_label" >
												<asp:TextBox runat="server" id="txt_PLAN_ICT_EDT" disabled="disabled" maxlength="15" size="10" tabindex="4" />
											</td>
											<th align="left" class="Body_TableHeader"><asp:Label ID="lb_ICT_EDT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_ICT_EDT%>"></asp:Label>:</th>
                                			<td align="left" class="Body_label" >
												<asp:TextBox runat="server" id="txt_ICT_EDT" disabled="disabled" maxlength="15" size="10" tabindex="4" />
											</td>
											
                                		</tr>
                                		<tr>
                               				<th align="left" class="Body_TableHeader"><asp:Label ID="lb_SUPPORT_SDT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SUPPORT_SDT%>"></asp:Label>:</th>
                                			<td align="left" class="Body_label" >
												<asp:TextBox runat="server" id="txt_SUPPORT_SDT" disabled="disabled" maxlength="15" size="10" tabindex="4" />
											</td>
                               				<th align="left" class="Body_TableHeader"><asp:Label ID="lb_PLAN_SUPPORT_EDT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PLAN_SUPPORT_EDT%>"></asp:Label>:</th>
                                			<td align="left" class="Body_label" >
												<asp:TextBox runat="server" id="txt_PLAN_SUPPORT_EDT" disabled="disabled" maxlength="15" size="10" tabindex="4" />
											</td>
											<th align="left" class="Body_TableHeader"><asp:Label ID="lb_SUPPORT_EDT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SUPPORT_EDT%>"></asp:Label>:</th>
                                			<td align="left" class="Body_label" >
												<asp:TextBox runat="server" id="txt_SUPPORT_EDT" disabled="disabled" maxlength="15" size="10" tabindex="4" />
											</td>
											
                                		</tr>--%>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_BACK_SCHOOL_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_BACK_SCHOOL_DT%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_BACK_SCHOOL_DT" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_BACK_PLANT_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_BACK_PLANT_DT%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_BACK_PLANT_DT" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_BE_CONTRACT_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_BE_CONTRACT_DT%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_BE_CONTRACT_DT" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_BE_DESPATCH_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_BE_DESPATCH_DT%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_BE_DESPATCH_DT" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_BE_EMP_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_BE_EMP_DT%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_BE_EMP_DT" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_label"></th>
                                                    <td align="left" class="Body_label"></td>

                                                </tr>

                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_RECENT_LEVEL_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_RECENT_LEVEL_DT%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_RECENT_LEVEL_DT" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_RECENT_PJOB_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_RECENT_PJOB_DT%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_RECENT_PJOB_DT" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_RECENT_DEPT_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_RECENT_DEPT_DT%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_RECENT_DEPT_DT" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_RECENT_DIV_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_RECENT_DIV_DT%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_RECENT_DIV_DT" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_RECENT_LEVEL_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_RECENT_LEVEL_WORK_DAYS%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_RECENT_LEVEL_WORK_DAYS" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_RECENT_PJOB_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_RECENT_PJOB_WORK_DAYS%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_RECENT_PJOB_WORK_DAYS" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_RECENT_DEPT_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_RECENT_DEPT_WORK_DAYS%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_RECENT_DEPT_WORK_DAYS" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_RECENT_DIV_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_RECENT_DIV_WORK_DAYS%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_RECENT_DIV_WORK_DAYS" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_WORK_DAYS_MONTH" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_WORK_DAYS_MONTH%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_WORK_DAYS_MONTH" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_WORK_DAYS%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_WORK_DAYS" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_SERVICE_DAYS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SERVICE_DAYS%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_SERVICE_DAYS" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_CAL_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_CAL_WORK_DAYS%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox runat="server" ID="txt_CAL_WORK_DAYS" disabled="disabled" MaxLength="15" size="10" TabIndex="4" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>

                                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                                            <colgroup>
                                                <col width="5%" />
                                                <col width="25%" />
                                                <col width="20%" />
                                                <col width="23%" />
                                            </colgroup>
                                            <tbody>

                                                <tr>
                                                    <td align="right" class="Body_label" colspan="10">
                                                        <div id="init">
                                                            <%--<input id="btn_back" type="button" value="<%$Resources:Resource,wfb2sc_btn_back%>" onclick="location.href = ('WFB2SC2200_Qry.aspx');" runat="server" />--%>
                                                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_back%>" OnClick="btn_back_Click" />
                                                        </div>
                                                    </td>
                                                </tr>





                                            </tbody>
                                        </table>
                                    </div>
                                </td>
                            </tr>






                            <!--======================================================================================-->


                        </table>
                    </td>
                </tr>


                <!-- START: Show only Save and Cancel button instead for Add and Edit operation -->



                <!-- END: Show only Save and Cancel button instead for Add and Edit operation -->
            </table>
        </ContentTemplate>

    </asp:UpdatePanel>
    <asp:ValidationSummary ID="vs1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>


