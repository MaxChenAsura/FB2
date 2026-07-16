<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ib/WFB2IB0500_Qry.aspx.cs" Inherits="WebContent_fb2ib_WFB2IB0500_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            
            
            iniForm();
        });
        function iniForm() {
            $('.ym').mask('9999/99');
            $("#txt_SALARY_YM").mask("9999/99/99");            
            $("#txt_AFT_INS_TOTAL").mask("9999999999");            
            $('.date').datepicker({ dateFormat: 'yy/mm' });
            $('.ymd').mask('9999/99/99');
            $('.date1').datepicker({ dateFormat: 'yy/mm/dd' });
            $.unblockUI();
            $(".money").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });
            gridviewScroll();
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"
                //,freezesize: 3
            });


        }

        function CheckSearch() {

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return false;
        }

        function doBlock() {

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return false;
        }

        function doBlock2() {

            if (Page_ClientValidate("GroupC")) {
                BlockUI();
            }
            else
                return false;
        }

        //儲存前檢查
        function saveCheck() {
            var processed = true;
            //alert($("#hid_Valid_Flag").val());

            //檢查是否做過月度結算
            $.ajax({
                url: "WFB2IB0500_ChecData.ashx",
                data: {
                    txt_YM: $('#txt_YM').val()

                },
                type: "GET",
                dataType: 'text',
                async: false,
                success: function (result) {
                    if (result == "Y")
                        processed = confirm("此年月已經計算過，是否要繼續執行？");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });            

            return processed;
        }

        function excelCheck(msg) {
            var processed = true;
           
            if (Page_ClientValidate("GroupB")) {                
                //BlockUI();
            } else
                processed = false;

            if (processed) {
                processed = confirm("確定要進行" + msg);
                BlockUI();
            }

            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }      

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <tr>
                        <td colspan="5">
                            <fieldset style="padding:5px">
                                <legend class="Body_label">
                                    <asp:Label ID="lb_DATA_UPLOAD" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_DATA_EXPORT%>"></asp:Label>
                                </legend>
                                <table align="left">
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_ExportExcel" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_ExportExcel%>"></asp:Label>：
                                        </th>
                                        <td align="left" class="Body_label">
                                           <asp:TextBox ID="txt_Excel_YM" CssClass="MandatoryField ym date" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static"></asp:TextBox>	
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_required_Excel_YM%>"
                                               ControlToValidate="txt_Excel_YM" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>                                           
                                           <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                           ErrorMessage="<%$Resources:Resource,wfb2ib_error_Excel_YM%>" ControlToValidate="txt_Excel_YM" ForeColor="Red" ValidationGroup="GroupB"
                                           ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                        </td>                                                                                               
                                    </tr>                                   
                                </table>
                                <table align="right">
                                    <tr>                                        
                                        <td>
                                            <asp:Button ID="WFB2IB0500ExcelDown" runat="server" ValidationGroup="GroupB" Text="<%$Resources:Resource,wfb2ib_WFB2IB0500ExcelDown%>" OnClick="WFB2IB0500ExcelDown_Click" OnClientClick="return excelCheck(this.value);" />

                                            <%--
                                                <aces:Btn ID="WFB2IB0500ExcelDown" runat="server" ValidationGroup="GroupB" Text="<%$Resources:Resource,wfb2ib_WFB2IB0500ExcelDown%>" OnClick="WFB2IB0500ExcelDown_Click" OnClientClick="return excelCheck(this.value);" />			
                                                <asp:Button ID="WFB2IB0500ExcelDown" runat="server" ValidationGroup="GroupB" Text="<%$Resources:Resource,wfb2ib_WFB2IB0500ExcelDown%>" OnClick="WFB2IB0500ExcelDown_Click" OnClientClick="return excelCheck(this.value);" />--%>			
                                        </td>
                                    </tr>                                   
                                </table>

                            </fieldset>
                        </td>
                    </tr>
                    <tr style="height:10px"></tr>
            </table>

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
                        <td class="Body_label">                           
                                <table>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_SALARY_YM_A%>"></asp:Label>：	                                            
                                        </th> 
                                        <td>
                                            <asp:TextBox ID="txt_SALARY_YM" CssClass="MandatoryField date" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static"></asp:TextBox>	
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_required_SALARY_YM_A%>"
                                                ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>                                           
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ib_error_SALARY_YM_A%>" ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                        </td>                                      
                                    </tr>
                                    <tr>
                                        <th  align="left" class="Body_TableHeader">
                                             <asp:Label ID="lb_AFT_INS_TOTAL" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_AFT_INS_TOTAL%>"></asp:Label>：	
                                        </th>
                                        <td align="left">                                           
                                            <asp:TextBox ID="txt_AFT_INS_TOTAL" CssClass="MandatoryField" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_required_AFT_INS_TOTAL%>"
                                                ControlToValidate="txt_AFT_INS_TOTAL" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>                                      
                                        </td>                                                                          
                                    </tr>  
                                    <%--  
                                    <tr>
                                        <th  align="left" class="Body_TableHeader">
                                             <asp:Label ID="lb_IACYC" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_IACYC%>"></asp:Label>：	
                                        </th>
                                        <td align="left">                                           
                                            <asp:TextBox ID="txt_IACYC" CssClass="MandatoryField date" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_required_IACYC%>"
                                                ControlToValidate="txt_IACYC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>                                      
                                        </td>                                                                          
                                    </tr> 
                                        --%>                                
                                </table>
                                <table align="right">
                                    <tr>
                                        <td align="right" class="Body_label" colspan="10">   
                                              
                                                  <asp:Button ID="WFB2IB0500Exec" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0500Exec%>" ValidationGroup="GroupA" OnClick="WFB2IB0500Exec_Click" OnClientClick= "return doBlock();" />                                  
                                              <%--
                                                  <aces:Btn ID="WFB2IB0500Exec" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0500Exec%>" ValidationGroup="GroupA" OnClick="WFB2IB0500Exec_Click" OnClientClick= "return doBlock();" />
                                                  <asp:Button ID="WFB2IB0500Exec" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0500Exec%>" ValidationGroup="GroupA" OnClick="WFB2IB0500Exec_Click" OnClientClick= "return doBlock();" />--%>
                                        </td>
                                    </tr>
                                </table>                           
                        </td>
                    </tr>
                     <tr>
                        <td align="center" height="1" colspan="1">
                            <hr>
                        </td>
                    </tr>    
                    <tr style="height:10px"></tr>
                                  
                   
                    <!-- END: Create a line -->
                </tbody>
            </table>  
                     
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
               
                <colgroup>
                    <col width="10%" />
                    <col width="30%" />
                    <col width="10%" />
                    <col width="30%" />
                    <col width="20%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                         <th align="left" class="Body_TableHeader">
                             <asp:Label ID="lb_YM" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_YM_search%>"></asp:Label>：	                                            
                         </th> 
                         <td>
                             <asp:TextBox ID="txt_YM" CssClass="ym date" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static"></asp:TextBox>	                                                                                     
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                  ErrorMessage="<%$Resources:Resource,wfb2ib_error_txt_YM%>" ControlToValidate="txt_YM" ForeColor="Red" ValidationGroup="GroupC"
                                  ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                         </td>    
                         <th  align="left" class="Body_TableHeader">
                             <asp:Label ID="lb_BILL_NO" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_BILL_NO%>"></asp:Label>：	
                         </th>
                         <td align="left">                                      
                             <asp:TextBox ID="txt_BILL_NO" runat="server" MaxLength="20" Width="81px" ClientIDMode="Static"></asp:TextBox>                                                                                 
                         </td>                                   
                      </tr>                           
                      <tr>
                          <th></th>
                          <th></th>
                          <th></th>
                          <td align="right" class="Body_label" >                                   
                              <asp:Button ID="WFB2IB0500Search" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0500Search%>" ValidationGroup="GroupC" OnClick="WFB2IB0500Search_Click" OnClientClick= "return doBlock2();" />                                  
                              <%--
                                <aces:Btn ID="WFB2IB0500Search" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0500Search%>" ValidationGroup="GroupC" OnClick="WFB2IB0500Search_Click" OnClientClick= "return doBlock2();" />
                                <asp:Button ID="WFB2IB0500Search" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0500Search%>" ValidationGroup="GroupC" OnClick="WFB2IB0500Search_Click" OnClientClick= "return doBlock2();" />
                              --%>
                          </td>
                      </tr>                                          
                    <tr>
                        <td align="center" height="1" colspan="4">
                            <hr>
                        </td>
                    </tr>                       
                    <tr style="height:30px"></tr>
                       
                    <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                        <colgroup>
                            <col width="3%" />
                            <col width="32%" />
                            <col width="10%" />
                            <col width="30%" />
                            <col width="20%" />
                        </colgroup>
                        <tr>
                            <%--入帳日期 --%>                                    
                            <th align="left">
                                <asp:Label ID="lb_IaDat" runat="server" Text="入帳日期:" Visible="false"></asp:Label>
                            </th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_IaDat" runat="server" Width="81px" CssClass="date1 MandatoryField"  ClientIDMode="Static" Visible="false" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="請輸入入帳日期"
                                     ControlToValidate="txt_IaDat" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator26" runat="server" ValidateEmptyText="true"
                                     ErrorMessage="入帳日期格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                     ControlToValidate="txt_IaDat" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>                                
                       </tr>
                    </table>                              
                                

                    <!-- END: Create a line -->
                </tbody>
            </table> 

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2IB0500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_YM"
                        Name="ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_BILL_NO" DefaultValue=""
                        Name="bill_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />                    
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1" OnRowCommand="gv_result_RowCommand">
                <Columns>                   
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2ib_RowNumber%>" HeaderStyle-Width="40px"/>
                    <asp:BoundField DataField="YM" HeaderText="<%$Resources:Resource,wfb2ib_lb_ib050_YM%>" SortExpression="YM" HeaderStyle-Width="200px" />
                    <asp:BoundField DataField="TRANS_DT" HeaderText="<%$Resources:Resource,wfb2ib_lb_TRANS_DT%>" SortExpression="TRANS_DT" HeaderStyle-Width="200px" />
                    <asp:BoundField DataField="BILL_NO" HeaderText="HR單號" SortExpression="BILL_NO" HeaderStyle-Width="200px" />
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_lb_FUNC%>" HeaderStyle-Width="200px">
                        <ItemTemplate>
                            <asp:Button ID="WFB2IB0500BillOut" runat="server"
                                CommandName="SentToBill"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2ib_btn_FUNC%>"
                                OnClientClick= "return BlockUI();"/>                            

<%--                        <asp:Button ID="WFB2IB0500BillOut" runat="server"
                                CommandName="SentToBill"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2ib_btn_FUNC%>"/>
        
                            <aces:Btn ID="WFB2IB0500BillOut" runat="server"
                                CommandName="SentToBill"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2ib_btn_FUNC%>"/>                            --%>
                        </ItemTemplate>                       
                    </asp:TemplateField>  
                    <%-- 需款日期--%>
                    <asp:TemplateField HeaderText="需款日期">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:TextBox ID="txt_NcrDat" runat="server" MaxLength="10" Width="80px" ClientIDMode="AutoID" CssClass="MandatoryField date1 ymd" ></asp:TextBox>                                
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>                   
                    <asp:BoundField DataField="Lno" />  
                    <asp:BoundField DataField="TOTAL_INS" />        
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupC" ShowSummary="false" />            
        </ContentTemplate>
    </asp:UpdatePanel>

    

</asp:Content>