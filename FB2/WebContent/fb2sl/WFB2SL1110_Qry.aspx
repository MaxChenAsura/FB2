<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sl/action/WFB2SL1110_Qry.aspx.cs" Inherits="WebContent_fb2sl_WFB2SL1110_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $("#txt_DATA_YEAR").mask("9999");            
            $.unblockUI();

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

            if (!Page_ClientValidate("GroupA") || !Page_ClientValidate("GroupC")) {
                processed = false;
            }
            else {
                BlockUI();
            }
            if (!processed)
                $.unblockUI();


            return processed;

        }

        function checkvalue2() {
            var processed = true;

            if (!Page_ClientValidate("GroupB") || !Page_ClientValidate("GroupC")) {
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
                                    <asp:Label ID="lb_BILL_IN" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_Labor_Upload%>"></asp:Label>
                                </legend>
                                <table width="100%">
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_DATA_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_DATA_YEAR%>"></asp:Label>：	
                                        </th>
                                        <td align="left">                                            
                                            <asp:TextBox ID="txt_DATA_YEAR" CssClass="MandatoryField yy date" runat="server" MaxLength="4" Width="81px" ClientIDMode="Static" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_required_DATA_YEAR%>"
                                                ControlToValidate="txt_DATA_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                                ErrorMessage="<%$Resources:Resource,wfb2sl_error_DATA_YEAR%>" ControlToValidate="txt_DATA_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                                ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                        </td>
                                        <td></td> 
                                        <td></td> 
                                    </tr>                                                                     
                                     <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_LABOR_UPLOAD_FILE" runat="server" Text="<%$Resources:Resource,wfb2de_lb_AFA_UPLOAD_FILE%>"></asp:Label>：
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:FileUpload ID="FileUpload" runat="server" Width="600px" ClientIDMode="Static" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="FileUpload" ForeColor="Red" ValidationGroup="GroupA"
                                                 ErrorMessage="<%$Resources:Resource,wfb2_format_excel%>" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(txt|TXT)$" Display="None"></asp:RegularExpressionValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="FileUpload" ForeColor="Red" ValidationGroup="GroupB"
                                                 ErrorMessage="<%$Resources:Resource,wfb2_format_excel%>" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(xls|XLS|xlsx|XLSX)$" Display="None"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="FileUpload" ForeColor="Red" ValidationGroup="GroupC"
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
                                            <aces:Btn ID="WFB2SL1111Upload" runat="server" Text="勞保匯入" OnClick="WFB2SL1111Upload_Click" OnClientClick ="return checkvalue();"/>
                                            <aces:Btn ID="WFB2SL1112Upload" runat="server" Text="健保匯入" OnClick="WFB2SL1112Upload_Click" OnClientClick ="return checkvalue2();"/>
                                            <asp:Button ID="WFB2SL1110ExcelDown" runat="server" Text="健保範例匯出" OnClick="btn_Excel_Down_Click" />
                                   <%-- 
                                         <asp:Button ID="WFB2SL1110ExcelDown" runat="server" Text="健保範例匯出" OnClick="btn_Excel_Down_Click" />
                                    --%>
                                            <%-- 
                                            <aces:Btn ID="WFB2SL1111Upload" runat="server" Text="勞保匯入" OnClick="WFB2SL1111Upload_Click" OnClientClick ="return checkvalue();"/>
                                            <aces:Btn ID="WFB2SL1112Upload" runat="server" Text="健保匯入" OnClick="WFB2SL1112Upload_Click" OnClientClick ="return checkvalue2();"/>
                                            
                                            <asp:Button ID="WFB2SL1111Upload" runat="server" Text="勞保匯入" OnClick="WFB2SL1111Upload_Click" OnClientClick ="return checkvalue();"/>
                                            <asp:Button ID="WFB2SL1112Upload" runat="server" Text="健保匯入" OnClick="WFB2SL1112Upload_Click" OnClientClick ="return checkvalue2();"/>
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
                     
                    <!-- END: Create a line -->
                </tbody>        
            </table>
               

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupC" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SL1111Upload" />  
            <asp:PostBackTrigger ControlID="WFB2SL1112Upload" />   
            <asp:PostBackTrigger ControlID="WFB2SL1110ExcelDown" />            
        </Triggers>
    </asp:UpdatePanel>  
</asp:Content>
