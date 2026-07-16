<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2hc/WFB2HC0401_Dtl.aspx.cs" Inherits="WebContent_fb2hc_WFB2HC0401_Dtl" %>
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
                barcolor: "#7F7F7F"
            });
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
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
								<colgroup>                                 	
									<col width="12%" />
									<col width="10%" />
									<col width="12%" />
									<col width="15%" />
									<col width="12%" />
									<col width="10%" />
									<col width="12%" />
									<col width="17%" />
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
								    <td align="left" class="Body_label" ></td>
								    <td align="left" class="Body_label" ></td>
								    <td align="left" class="Body_label" ></td>
								    <td align="left" class="Body_label" ></td>
							    </tr>

							    <tr>
                                    <%--聘用單位--%>
								    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD_DESC" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_COMPANY_CD%>"></asp:Label>:
								    </th>
				          		    <td align="left" class="Body_label" > 
                                        <asp:TextBox ID="txt_COMPANY_CD_DESC" runat="server" size="8" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>				          			    
				          		    </td>
                                    <%--獎金類型--%>
								    <th align="left" class="Body_TableHeader">                                        
                                        <asp:Label ID="lb_BONUS_TYPE_DESC" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_BONUS_TYPE_DESC%>"></asp:Label>:
								    </th>
				          		    <td align="left" class="Body_label" > 
                                        <asp:TextBox ID="txt_BONUS_TYPE_DESC" runat="server" size="15" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
				          		    </td>
                                    <%--發放人數合計--%>
								    <th align="left"  class="Body_TableHeader">
                                        <asp:Label ID="lb_MEMBER_CNT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_TOTAL_REAL%>"></asp:Label>:
								    </th>
								    <td align="left" class="Body_label">  
                                        <asp:TextBox ID="txt_MEMBER_CNT" runat="server" size="8" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>
								    </td>
                                    <%--發放金額合計--%>
				          		    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_AMT_CNT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_TOTAL_AMT_REAL%>"></asp:Label>:
				          		    </th>
				          		    <td align="left" class="Body_label" > 
                                        <asp:TextBox ID="txt_AMT_CNT" runat="server" size="8" style="border:none" readonly="true" ClientIDMode="Static"></asp:TextBox>				          			    
				          		    </td>

							    </tr>																	
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <asp:Button ID="WFB2HC0401BackPage" runat="server" Text="<%$Resources:Resource,wfb2hc_btn_backPage%>" OnClick="WFB2HC0401BackPage_Click" />
                                            <asp:HiddenField ID="hid_PAY_YM_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_SALARY_DT_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_COMPANY_CD_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_COMPANY_CD_DESC_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_BONUS_TYPE_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_BONUS_TYPE_DESC_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_MEMBER_CNT_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_AMT_CNT_search" ClientIDMode="Static" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>                
                <tr>
                    <td style="height:10px;"></td>
                </tr>                    
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData2_d1"
                SelectCountMethod="getCount2_d1" TypeName="CFB2HC0400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_PAY_YM_search"
                        Name="pay_ym" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_COMPANY_CD_search"
                        Name="company_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_BONUS_TYPE_search"
                        Name="bonus_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />                    
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>                    
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hc_hd_RowNumber%>"  HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <asp:BoundField DataField="ORI_DEPT_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_ORI_DEPT_DESC%>" HeaderStyle-Width="360px" ItemStyle-Width="360px" ItemStyle-HorizontalAlign="Left" SortExpression="ORI_DEPT_DESC" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2hc_hd_EMP_ID%>" HeaderStyle-Width="60px" ItemStyle-Width="60px" SortExpression="EMP_ID" ItemStyle-HorizontalAlign="CENTER" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2hc_hd_EMP_NAME%>" HeaderStyle-Width="80px" ItemStyle-Width="80px" SortExpression="EMP_NAME" ItemStyle-HorizontalAlign="CENTER" />
                    <asp:BoundField DataField="JOIN_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_JOIN_DT%>"  HeaderStyle-Width="100px" ItemStyle-Width="100px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="JOIN_DT" />
                    <asp:BoundField DataField="LEAVE_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_LEAVE_DT%>"  HeaderStyle-Width="100px" ItemStyle-Width="100px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="LEAVE_DT" />
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_START_DT%>"  HeaderStyle-Width="100px" ItemStyle-Width="100px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="START_DT" />
                    <asp:BoundField DataField="BONUS_AMT" HeaderText="<%$Resources:Resource,wfb2hc_hd_BONUS_AMT1%>" HeaderStyle-Width="80px" ItemStyle-Width="80px" DataFormatString="{0:##,#}" ItemStyle-HorizontalAlign="Right" SortExpression="BONUS_AMT" />
                    <asp:TemplateField ItemStyle-Width="80px">
                        <HeaderTemplate><asp:Label ID="lb_function" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_function%>"></asp:Label></HeaderTemplate>
                        <ItemTemplate>
                        </ItemTemplate>                                
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                            <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                            <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Detail1_Choice_Not_Equal_1_Message" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Detail2_Choice_Not_Equal_1_Message" />            
        </ContentTemplate>

    </asp:UpdatePanel>
    <asp:ValidationSummary ID="vs1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>


