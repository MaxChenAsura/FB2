<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2hc/WFB2HC0300_Qry.aspx.cs" Inherits="WebContent_fb2hc_WFB2HC0300_Qry" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
           
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date2").mask('9999/99')

            $("#txt_qry_DEPT_NAME").attr("readonly", true);
            $("#txt_qry_EMP_NAME").attr("readonly", true);

            gridviewScroll();
            gridviewScroll2();
            //部門代號取得部門名稱的ajax
            $("#txt_ORI_DEPT_NO_search").blur(function () {
                if ($("#txt_ORI_DEPT_NO_search").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_ORI_DEPT_NO_search').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_qry_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_qry_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_qry_DEPT_NAME').val("");
                }
            });

            //工號取得姓名的ajax
            $("#txt_EMP_ID_search").blur(function () {
                if ($("#txt_EMP_ID_search").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID_search').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_qry_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_qry_EMP_NAME').val(JData.EMP_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_qry_EMP_NAME').val("");
                }
            });

            $.unblockUI();
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());            
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function ShowRecord2(obj) {

            $("#HID_PageRow2").val($("#ddlPerPageRow2").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        function gridviewScroll2() {
            $('#<%=gv_result2.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        function ClearAll() {
            $('#txt_START_SYM_search').val("");
            $('#txt_START_EYM_search').val("");
            $('#txt_ORI_DEPT_NO_search').val("");
            $('#txt_qry_DEPT_NAME').val("");
            $('#txt_EMP_ID_search').val("");
            $('#txt_qry_EMP_NAME').val("");
            $('#ddl_COMPANY_CD_search').val("");
            $('#ddl_WS_CD_search').val("");
            $("#txt_pre_Master_Key").val("");
            $("#txt_Master_Key").val("");
            $("#hid_EMP_ID_2_search").val("");
            $("#hid_START_DT_2_search").val("");
            $("#txt_Detail_search").val("");
            return false;
        }        
        function proc_data() {
            var txt_START_SYM_search = $("#txt_START_SYM_search").val();
            $("#tmp_START_SYM_search").val("");
            if (txt_START_SYM_search != "")
                $("#tmp_START_SYM_search").val(txt_START_SYM_search.substring(0, 4) + '/' + txt_START_SYM_search.substring(4, 6) + '/01');

            var txt_START_EYM_search = $("#txt_START_EYM_search").val();
            $("#tmp_START_EYM_search").val("");
            if (txt_START_EYM_search != "")
                $("#tmp_START_EYM_search").val(txt_START_EYM_search.substring(0, 4) + '/' + txt_START_EYM_search.substring(4, 6) + '/01');
        }
        function setTxt_Master_Key(start_dt, ori_dept_info, emp_id) {
            $("#txt_Master_Key").val(start_dt + "," + ori_dept_info + "," + emp_id);
        }
        function setTxt_Detail_search(emp_id, start_dt) {
            $("#hid_EMP_ID_2_search").val(emp_id);
            $("#hid_START_DT_2_search").val(start_dt);            
            $("#txt_Detail_search").val(emp_id + "," + start_dt);
            form1.submit();
        }


        function CheckYM(source, arguments) {
            //console.log("ttt");
            if ($("#txt_START_SYM_search").val() != "" && $("#txt_START_EYM_search").val() != "") {
                //console.log("ttt2");
                var sym = $("#txt_START_SYM_search").val().replace("/", "");
                var eym = $("#txt_START_EYM_search").val().replace("/", "");
                //console.log(sym);
                //console.log(eym);
                if (eym < sym)
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }

        }

        //刪除,檢查是否有勾選
        function doDelete() {
            var processed = true;
            BlockUI();
            processed = confirm("確定要刪除?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">    
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:TextBox ID="txt_pre_Master_Key" runat="server" style="display:none;" ClientIDMode="Static" />
    <asp:TextBox ID="txt_Master_Key" runat="server" style="display:none;" ClientIDMode="Static" />
    <asp:TextBox ID="txt_Detail_search" runat="server" style="display:none;" ClientIDMode="Static" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>            
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>                                 	
									<col width="12%" />
                                    <col width="8%" />
									<col width="3%" />
									<col width="8%" />
									<col width="10%" />
									<col width="14%" />
									<col width="10%" />
                                    <col width="32%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <td height="10px"></td>
                                </tr>
                                <tr>
	                                <%--契約開始年月--%>										
	                                <th align="left" class="Body_TableHeader">
		                                <asp:Label ID="lb_START_YM_search" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_START_YM%>"></asp:Label>：
	                                </th>
                                    <td align="left" class="Body_label" >
		                                <asp:TextBox ID="txt_START_SYM_search" runat="server"  size="6" ClientIDMode="Static" CssClass="date2"></asp:TextBox>
                                        <asp:TextBox ID="tmp_START_SYM_search" runat="server" size="5" style="display:none;" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2hc_START_SYM_Format_Error%>" ControlToValidate="txt_START_SYM_search" ForeColor="Red"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                         <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="契約開始年月(迄不可大於起)" ClientValidationFunction="CheckYM" ForeColor="Red"
                                            ControlToValidate="txt_START_EYM_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>   
	                                </td>	
	                                <td>~</td>										
	                                <td align="left" class="Body_label" >
		                                <asp:TextBox ID="txt_START_EYM_search" runat="server"  size="6" ClientIDMode="Static" CssClass="date2"></asp:TextBox>
                                        <asp:TextBox ID="tmp_START_EYM_search" runat="server" size="5" style="display:none;" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2hc_START_EYM_Format_Error%>" ControlToValidate="txt_START_EYM_search" ForeColor="Red"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>                                   
	                                </td>
	                                <%--部門代號--%>
	                                <th align="left"  class="Body_TableHeader">
		                                <asp:Label ID="lb_ORI_DEPT_NO_search" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_ORI_DEPT_NO%>"></asp:Label>:
	                                </th>
                                    <td align="left" class="Body_label" colspan="3">											
		                                <asp:TextBox ID="txt_ORI_DEPT_NO_search" runat="server" MaxLength="7" size="10" ClientIDMode="Static"></asp:TextBox>
		                                <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_ORI_DEPT_NO_search', 'txt_qry_DEPT_NAME', 'N');" />
                                        <asp:TextBox ID="txt_qry_DEPT_NAME" runat="server"  BorderWidth="0" ClientIDMode="Static"/>
	                                </td>
                                </tr>									
                                <tr>	
	                                <%--工號--%>									
	                                <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_EMP_ID%>"></asp:Label>：
	                                </th>
	                                <td align="left" class="Body_label"  colspan="3">
                                        <asp:TextBox ID="txt_EMP_ID_search" runat="server" MaxLength="5" size="3" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_search', 'txt_qry_EMP_NAME', 'N');" />
                                        <asp:TextBox ID="txt_qry_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
	                                </td>
	                                <%--聘用單位--%>
	                                <th align="left"  class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_COMPANY_CD%>"></asp:Label>:
	                                </th>
	                                <td align="left" class="Body_label" >
		                                <asp:DropDownList id="ddl_COMPANY_CD_search" runat="server" ClientIDMode="Static"></asp:DropDownList>
	                                </td>
	                                <%--職種--%>
 	                                <th align="left"  class="Body_TableHeader">
                                         <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_WS_CD%>"></asp:Label>:
 	                                </th>
	                                <td align="left" class="Body_label" >
		                                <asp:DropDownList id="ddl_WS_CD_search" runat="server" ClientIDMode="Static"></asp:DropDownList>
	                                </td>
                                 </tr>
                                <tr>
                                    <td align="right" class="Body_label" colspan="10">
                                        <div id="init">
                                            <aces:Btn ID="WFB2HC0300Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2HC0300Search_Click" OnClientClick="proc_data();CheckValid();" ValidationGroup="GroupA" />
                                            
                                            <%--<asp:Button ID="WFB2HC0300Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2HC0300Search_Click" OnClientClick="proc_data();CheckValid();" ValidationGroup="GroupA" />--%>
                                            
                                            <asp:Button ID="WFB2HC0300Clear" runat="server" type="button" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="return ClearAll();" />
                                            <asp:HiddenField ID="hid_START_SYM_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_START_EYM_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_ORI_DEPT_NO_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_EMP_ID_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_COMPANY_CD_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_WS_CD_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_EMP_ID_2_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_START_DT_2_search" ClientIDMode="Static" runat="server" />
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
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData1"
                SelectCountMethod="getCount1" TypeName="CFB2HC0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_START_SYM_search"
                        Name="start_sym" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_START_EYM_search"
                        Name="start_eym" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_ORI_DEPT_NO_search"
                        Name="ori_dept_no" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_EMP_ID_search"
                        Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_COMPANY_CD_search"
                        Name="company_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_WS_CD_search"
                        Name="ws_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="40px">
                        <HeaderTemplate>
                            &nbsp;
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:RadioButton id="rb_check" runat="server" GroupName="rb_check" OnCheckedChanged="rb_check_CheckedChanged"/> 
                            <asp:TextBox ID="txt_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Style="display:none;"></asp:TextBox>
                            <asp:TextBox ID="txt_START_DT" runat="server" Text='<%#Bind("START_DT")%>' Style="display:none;"></asp:TextBox>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hc_hd_RowNumber%>" ItemStyle-Width="40px" />
                    <asp:BoundField DataField="COMPANY_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_COMPANY_CD_DESC%>" ItemStyle-Width="80px" SortExpression="COMPANY_CD_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ORI_DEPT_INFO" HeaderText="<%$Resources:Resource,wfb2hc_lb_ORI_DEPT_INFO%>" ItemStyle-Width="320px" ItemStyle-HorizontalAlign="Left" SortExpression="ORI_DEPT_INFO" />
                    <asp:BoundField DataField="WS_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_WS_CD%>" ItemStyle-Width="90px" SortExpression="WS_CD" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2hc_lb_EMP_ID%>" ItemStyle-Width="60px" SortExpression="EMP_ID" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2hc_lb_EMP_NAME%>" ItemStyle-Width="60px" SortExpression="EMP_NAME" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="BIRTH_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_BIRTH_DT%>" ItemStyle-Width="120px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="BIRTH_DT" />
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_START_DT%>" ItemStyle-Width="120px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="START_DT" />
                    <asp:BoundField DataField="PLAN_END_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_PLAN_END_DT%>" ItemStyle-Width="120px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="PLAN_END_DT" />
                    <asp:BoundField DataField="END_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_END_DT%>" ItemStyle-Width="120px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="END_DT" />
                    <asp:BoundField DataField="HR_CHG_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_HR_CHG_DESC%>" ItemStyle-Width="160px" SortExpression="HR_CHG_DESC" ItemStyle-HorizontalAlign="Left" />
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>            
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <asp:UpdatePanel id="updetail" runat="server" Visible="false">
                <ContentTemplate>                    
                    <asp:Label ID="lb_TOTAL_BONUS_AMT" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_TOTAL_BONUS_AMT%>"></asp:Label>：
                    <asp:Label ID="lb_TOTAL_BONUS_AMT_DATA" runat="server" Text=""></asp:Label>
                    <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="getData2"
                        SelectCountMethod="getCount2" TypeName="CFB2HC0300DAO" EnablePaging="True"
                        SortParameterName="sortExpression" OnSelecting="obs2_Selecting"
                        StartRowIndexParameterName="startRowIndex" OnSelected="ods2_Selected">
                        <SelectParameters>
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:ControlParameter ControlID="hid_EMP_ID_2_search"
                                Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                            <asp:ControlParameter ControlID="hid_START_DT_2_search"
                                Name="start_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                        </SelectParameters>
                    </asp:ObjectDataSource>

                    <%--meta:resourcekey="gv_resultResource2"--%>
                    <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting2"
                        OnRowDataBound="gv_result_RowDataBound2" OnRowCreated="gv_result_RowCreated2" Width="1020px"
                        OnPageIndexChanging="gv_result_PageIndexChanging2" meta:resourcekey="gv_resultResource2"
                        OnRowCommand="gv_result_RowCommand2"  OnDataBound="gv_result_DataBound2">
                        <Columns>                                
                            <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hc_hd_RowNumber%>" ItemStyle-Width="40px" />
                            <asp:BoundField DataField="BONUS_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2hc_hd_BONUS_TYPE_DESC%>" ItemStyle-Width="200px" SortExpression="BONUS_TYPE_DESC" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="PAY_YM" HeaderText="<%$Resources:Resource,wfb2hc_hd_PAY_YM%>" ItemStyle-Width="100px" SortExpression="PAY_YM" />
                            <asp:BoundField DataField="BONUS_AMT" HeaderText="<%$Resources:Resource,wfb2hc_hd_BONUS_AMT%>" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right" SortExpression="BONUS_AMT" />
                            <asp:BoundField DataField="SALARY_STATUS" HeaderText="<%$Resources:Resource,wfb2hc_hd_SALARY_STATUS%>" ItemStyle-Width="150px" SortExpression="SALARY_STATUS" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="SALARY_DT" HeaderText="<%$Resources:Resource,wfb2hc_hd_SALARY_DT%>" ItemStyle-Width="150px" SortExpression="SALARY_DT" />                    
                         <asp:TemplateField HeaderText="功能" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="WFB2HC0300Delete" runat="server"
                                    CommandName="ToDelete"
                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    Text="刪除"  OnClientClick="return doDelete();"/>
                            </ItemTemplate>
                         </asp:TemplateField>            
                        </Columns>
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <FooterStyle CssClass="GridviewScrollPager" />
                    </asp:GridView>                    
                    <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
                </ContentTemplate>                
            </asp:UpdatePanel>

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Detail1_Choice_Not_Equal_1_Message" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Detail2_Choice_Not_Equal_1_Message" />            
        </ContentTemplate>
        <%--<Triggers>            
            <asp:PostBackTrigger ControlID="txt_Detail_Key" /> 
            <asp:PostBackTrigger ControlID="WFB2HC0300Search" /> 
            <asp:PostBackTrigger ControlID="WFB2HC0300Clear" />
            <asp:PostBackTrigger ControlID="txt_ORI_DEPT_NO_search" /> 
            <asp:PostBackTrigger ControlID="txt_EMP_ID_search" />           
        </Triggers>--%>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="vs1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>


