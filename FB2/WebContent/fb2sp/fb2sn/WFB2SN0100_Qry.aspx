<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sn/WFB2SN0100_Qry.aspx.cs" Inherits="WebContent_fb2sn_WFB2SN0100_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $("#txt_YEAR").mask("9999");
            gridviewScroll();
            $.unblockUI();

        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"               
            });
        }

        function CheckSearch() {

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return false;
        }
               
        function uploadCheck() {
            //檢查是否有選擇檔案            
            //if ($('#fileUpload').val() == '') {
            //    alert("請選擇要上傳的檔案");
            //    return false;
            if (!Page_ClientValidate("GroupB")) {
                return false;
            } else {
                if (confirm('您確定要以上傳的Excel檔，取代相同年月的資料嗎?')) {
                    return true;
                }
                else {
                    return false;
                }
            }

        }

        function checkDowning(msg) {
            var processed = true;

            processed = confirm("確定要進行" + msg + "?");
            //if (processed) {
            //    BlockUI();
            //}
            return processed;
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function checkvalue() {
            var processed = true;

            if (!Page_ClientValidate("GroupA")) {
                processed = false;
            }
            else {
                BlockUI();
            }
            if (!processed)
                $.unblockUI();


            return processed;

        }
        function searchCheck() {
            var processed = true;

            if (!Page_ClientValidate("GroupB")) {
                processed = false;
            }
            else {
                BlockUI();
            }
            if (!processed)
                $.unblockUI();


            return processed;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">

                <colgroup>
                    <col width="15%" />
                    <col width="35%" />
                    <col width="45%" />
                    <col width="5%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <td class="Body_label" colspan="4">
                            <fieldset style="padding: 5px">
                                <legend class="Body_label">
                                    <asp:Label ID="lb_BILL_IN" runat="server" Text="<%$Resources:Resource,wfb2sn_lb_AFA_Upload%>"></asp:Label>
                                </legend>
                                <table width="100%">
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sn_lb_YEAR%>"></asp:Label>：	
                                        </th>
                                        <td align="left">                                            
                                            <asp:TextBox ID="txt_YEAR" CssClass="MandatoryField yy date" runat="server" MaxLength="4" Width="81px" ClientIDMode="Static" OnTextChanged="txt_YEAR_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sn_required_YEAR%>"
                                                ControlToValidate="txt_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                                ErrorMessage="<%$Resources:Resource,wfb2sn_error_YEAR%>" ControlToValidate="txt_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                                ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                        </td>
                                        <td></td> 
                                        <td></td> 
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lbl_AFA_FOR" runat="server" Text="<%$Resources:Resource,wfb2sn_lbl_AFA_FOR%>"></asp:Label>:
                                        </th>                                    
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_AFA_FOR" runat="server" ClientIDMode="Static" CssClass="MandatoryField" ></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sn_required_AFA_FOR%>"
                                            ControlToValidate="ddl_AFA_FOR" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td></td> 
                                        <td></td> 
                                    </tr>                                    
                                     <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_AFA_UPLOAD_FILE" runat="server" Text="<%$Resources:Resource,wfb2de_lb_AFA_UPLOAD_FILE%>"></asp:Label>：
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:FileUpload ID="FileUpload" runat="server" Width="600px" ClientIDMode="Static" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="FileUpload" ForeColor="Red" ValidationGroup="GroupA"
                                                 ErrorMessage="<%$Resources:Resource,wfb2_format_excel%>" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(xls|XLS|xlsx|XLSX)$" Display="None"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="FileUpload" ForeColor="Red" ValidationGroup="GroupA"
                                                 ErrorMessage="<%$Resources:Resource,wfb2_required_excel%>" Display="None"></asp:RequiredFieldValidator>                                                          
                                        </td> 
                                         <td></td> 
                                         <td></td>                                     
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <th></th>
                                        <th></th>                                        
                                        <td align="right">                                            
                                            <aces:Btn ID="WFB2SN0100ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2sn_WFB2SN0100ExcelDown%>" OnClick="WFB2SN0100ExcelDown_Click" />
                                            <aces:Btn ID="WFB2SN0100Upload" runat="server" Text="<%$Resources:Resource,wfb2sn_WFB2SN0100Upload%>" OnClick="WFB2SN0100Upload_Click" OnClientClick ="return checkvalue();"/>
                                            <%-- 
                                            <aces:Btn ID="WFB2SN0100ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2sn_WFB2SN0100ExcelDown%>" OnClick="WFB2SN0100ExcelDown_Click" />
                                            <aces:Btn ID="WFB2SN0100Upload" runat="server" Text="<%$Resources:Resource,wfb2sn_WFB2SN0100Upload%>" OnClick="WFB2SN0100Upload_Click" OnClientClick ="return checkvalue();"/>
                                            
                                              <asp:Button ID="WFB2SN0100ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2sn_WFB2SN0100ExcelDown%>" OnClick="WFB2SN0100ExcelDown_Click" />
                                            <asp:Button ID="WFB2SN0100Upload" runat="server" Text="<%$Resources:Resource,wfb2sn_WFB2SN0100Upload%>" OnClick="WFB2SN0100Upload_Click" OnClientClick ="return checkvalue();"/>  
                                                --%>
                                            
                                                                          
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
                        <th align="left" class="Body_TableHeader"><asp:Label ID="lb_SEARCH_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sn_lb_SEARCH_YEAR%>"></asp:Label>：	</th>
                        <td align="left">                             
                             <asp:TextBox ID="txt_SEARCH_YEAR" CssClass="MandatoryField yy date" runat="server" MaxLength="4" Width="81px" ClientIDMode="Static" OnTextChanged="txt_SEARCH_YEAR_TextChanged" AutoPostBack="true"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sn_required_SEARCH_YEAR%>"
                                  ControlToValidate="txt_SEARCH_YEAR" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                  ErrorMessage="<%$Resources:Resource,wfb2sn_error_SEARCH_YEAR%>" ControlToValidate="txt_SEARCH_YEAR" ForeColor="Red" ValidationGroup="GroupB"
                                  ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                        </td>
                        <td></td>
                        <td></td>                        
                   </tr>                             
                   <tr>
                         <th align="left" class="Body_TableHeader">
                             <asp:Label ID="lb_SEARCH_AFA_FOR" runat="server" Text="<%$Resources:Resource,wfb2sn_lbl_SEARCH_AFA_FOR%>"></asp:Label>:
                         </th>                                    
                         <td align="left" class="Body_label">
                             <asp:DropDownList ID="ddl_SEARCH_AFA_FOR" runat="server" ClientIDMode="Static" CssClass="MandatoryField" ></asp:DropDownList>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sn_required_SEARCH_AFA_FOR%>"
                                  ControlToValidate="ddl_SEARCH_AFA_FOR" ForeColor="Red" ValidationGroup="GroupB" Display="None" InitialValue="-1">
                             </asp:RequiredFieldValidator>
                         </td>
                         <th></th>
                         <td></td>
                   </tr>
                    <tr>
                        <th></th>
                        <th></th>
                        <th></th>
                         <td align="right" >
                                   <aces:Btn ID="WFB2SN0100Search" runat="server" Text="<%$Resources:Resource,wfb2sn_WFB2SN0100Search%>" OnClick="WFB2SN0100Search_Click" OnClientClick="return searchCheck();"/>
                             <%-- 
                                 <asp:Button ID="WFB2SN0100Search" runat="server" Text="<%$Resources:Resource,wfb2sn_WFB2SN0100Search%>" OnClick="WFB2SN0100Search_Click" OnClientClick="return searchCheck();"/>

                                 --%>
                         </td>
                     </tr>  
                    <!-- END: Create a line -->
                </tbody>        
            </table>

             <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SN0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_SEARCH_YEAR"
                        Name="SEARCH_YEAR" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_SEARCH_AFA_FOR" DefaultValue=""
                        Name="SEARCH_AFA_FOR" PropertyName="SelectedValue" Type="String" />                   
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sn_lb_AFA_APPROVE_MARK%>" HeaderStyle-Width="80px">                       
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="Static" Enabled ="false"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2df_RowNumber%>" HeaderStyle-Width="60px" />
                    <asp:BoundField DataField="AFA_APPROVE_STATUS" HeaderText="<%$Resources:Resource,wfb2sn_lb_AFA_APPROVE_STATUS%>" SortExpression="AFA_APPROVE_STATUS"
                         HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2sn_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2sn_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="PJOB_CD" HeaderText="<%$Resources:Resource,wfb2sn_lb_PJOB_CD%>" SortExpression="PJOB_CD" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="LEVEL_CD" HeaderText="<%$Resources:Resource,wfb2sn_lb_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="AFA_AMT" HeaderText="<%$Resources:Resource,wfb2sn_lb_AFA_AMT%>" SortExpression="AFA_AMT" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:n0}"/>
                    <asp:BoundField DataField="AFA_REMARK" HeaderText="<%$Resources:Resource,wfb2sn_lb_AFA_REMARK%>" SortExpression="AFA_REMARK" HeaderStyle-Width="340px" ItemStyle-HorizontalAlign="Left"/>  
                    <asp:BoundField DataField="AFA_APPROVE_MARK" />            
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
             <td style="font-size: 14px;">
                <asp:Label ID="lb_TotalCount2" runat="server" Text="" Visible="false" Font-Size="10"></asp:Label>
            </td>
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

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SN0100Upload" />  
            <asp:PostBackTrigger ControlID="WFB2SN0100ExcelDown" />          
        </Triggers>
    </asp:UpdatePanel>  
</asp:Content>
