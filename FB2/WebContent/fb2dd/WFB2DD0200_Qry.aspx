<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dd/WFB2DD0200_Qry.aspx.cs" Inherits="WebContent_fb2dd_WFB2DD0200_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            $('.ym').mask('9999/99');
            iniForm();
        });
        function iniForm() {
            $('.ym').mask('9999/99');
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date1").datepicker({ dateFormat: 'yy/mm' });
            $("#txt_MANAGER_YM_S").mask("9999/99");
            $("#txt_MANAGER_YM_E").mask("9999/99");           
            
            gridviewScroll();
            $.unblockUI();


            $('#txt_NEW_EMP_ID').change(function () {

                //ajax 取得員工基本資料
                $.ajax({
                    url: "WFB2DD0100_GetEmpData.ashx",
                    data: {
                        EMP_ID: $('#txt_NEW_EMP_ID').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "")
                            alert(JData.errMsg);
                        else {
                            $('#lb_NEW_EMP_NAME').text(JData.EMP_NAME);
                            $('#lb_NEW_DEPT_NAME').text(JData.DEPT_NAME);
                            $('#lb_NEW_PLANT_CD').text(JData.PLANT_NAME);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });

            });
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"
                //,freezesize: 3
            });
          

        }

        //清空畫面
        function ClearAll() {            
            $("#txt_MANAGER_YM").val("");
            $("#txt_MANAGER_YM_S").val("");
            $("#txt_MANAGER_YM_E").val("");   
                      
        }

        function CheckSearch() {

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return false;
        }

        //儲存前檢查
        function saveCheck() {
            var processed = true;
            //alert($("#hid_Valid_Flag").val());

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;

            if (!processed) {
                $.unblockUI();
                return;
            }
            return processed;
        }
       
        
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function CheckYM(source, arguments) {
            if ($("#txt_MANAGER_YM_S").val() != "" && $("#txt_MANAGER_YM_E").val() != "") {
                if ($("#txt_MANAGER_YM_E").val() < $("#txt_MANAGER_YM_S").val())
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }

        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
               
                <colgroup>
                    <COL width="15%" />
					<COL width="35%" />
					<COL width="10%" />
					<COL width="40%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <td align="left" class="Body_label" colspan="5">
                            <fieldset style="padding:5px">
                                <legend class="Body_label">
                                    <asp:Label ID="lb_TRANS_CALCULATE" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_TRANS_CALCULATE%>"></asp:Label>
                                </legend>
                                <table align="left">
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lb_MANAGER_YM" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_MANAGER_YM%>"></asp:Label>	
                                            <asp:TextBox ID="txt_MANAGER_YM" CssClass="MandatoryField ym date1" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static"></asp:TextBox>	
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dd_required_MANAGER_YM_S%>"
                                                ControlToValidate="txt_MANAGER_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                           
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2dd_error_MANAGER_YM%>" ControlToValidate="txt_MANAGER_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                        </td>                                       
                                    </tr>                                   
                                </table>
                                <table align="right">
                                    <tr>                                        
                                        <td>
                                          <aces:Btn ID="WFB2DD0200Execute" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0200Execute%>" OnClick="WFB2DD0200Execute_Click" ValidationGroup="GroupA" OnClientClick="return saveCheck();"/>				
                                          
                                            <%--<asp:Button ID="WFB2DD0200Execute" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0200Execute%>" OnClick="WFB2DD0200Execute_Click" ValidationGroup="GroupA" OnClientClick="return saveCheck();"/>--%>				
                                        </td>
                                    </tr>                                   
                                </table>

                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader" >                            
                            <asp:Label ID="lb_SEARCH_YM" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_SEARCH_YM%>"></asp:Label>	
                        </th>
						<td class="Body_label"> 
							<asp:TextBox ID="txt_MANAGER_YM_S" runat="server" MaxLength="7" Width="81px" CssClass="MandatoryField date1" ClientIDMode="Static" ></asp:TextBox>
                                    ~  
                            <asp:TextBox ID="txt_MANAGER_YM_E" runat="server" MaxLength="7" Width="81px" CssClass="MandatoryField date1" ClientIDMode="Static" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dd_required_MANAGER_YM_S%>"
                                            ControlToValidate="txt_MANAGER_YM_S" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2dd_error_MANAGER_YM_S%>" ControlToValidate="txt_MANAGER_YM_S" ForeColor="Red" ValidationGroup="GroupB"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dd_required_MANAGER_YM_E%>"
                                            ControlToValidate="txt_MANAGER_YM_E" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2dd_error_MANAGER_YM_E%>" ControlToValidate="txt_MANAGER_YM_E" ForeColor="Red" ValidationGroup="GroupB"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dd_greaterthan_MANAGER_YM%>" ClientValidationFunction="CheckYM" ForeColor="Red"
                                            ControlToValidate="txt_MANAGER_YM_E" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>											
						</td> 						
                    </tr>                  
                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <aces:Btn ID="WFB2DD0200Search" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0200Search%>" OnClick="WFB2DD0200Search_Click" ValidationGroup="GroupB"/>

                                <%--<asp:Button ID="WFB2DD0200Search" runat="server" Text="<%$Resources:Resource,wfb2dd_WFB2DD0200Search%>" OnClick="WFB2DD0200Search_Click" ValidationGroup="GroupB"/>--%>
                                
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dd_btn_clear%>" onclick="ClearAll();"/>                               
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr>
                        </td>
                    </tr>
                   
                    <!-- END: Create a line -->
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DD0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_MANAGER_YM_S"
                        Name="manager_ym_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_MANAGER_YM_E" DefaultValue=""
                        Name="manager_ym_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />                   
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px" 
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1" OnRowCommand="gv_result_RowCommand">
                <Columns>                   
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2dd_RowNumber%>" HeaderStyle-Width="40px"/>
                    <asp:BoundField DataField="MANAGER_YM" HeaderText="<%$Resources:Resource,wfb2dd_lb_MANAGER_YM%>" SortExpression="MANAGER_YM" HeaderStyle-Width="250px" />
                    <asp:BoundField DataField="TAKE_OUT_DT" HeaderText="<%$Resources:Resource,wfb2dd_lb_TAKE_OUT_DT%>" SortExpression="TAKE_OUT_DT" HeaderStyle-Width="250px" />
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dd_lb_FUNC%>" HeaderStyle-Width="250px">
                        <ItemTemplate>
                            <asp:Button ID="btn_FUNC" runat="server"
                                CommandName="SentToSalary"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2dd_btn_FUNC%>" OnClientClick="BlockUI();"/>                            
                        </ItemTemplate>                       
                    </asp:TemplateField>                    
                    <asp:BoundField DataField="REMARK" HeaderText="<%$Resources:Resource,wfb2dd_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="250px"/>
                   
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