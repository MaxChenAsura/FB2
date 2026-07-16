<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2de/WFB2DE0400_Qry.aspx.cs" Inherits="WebContent_fb2de_WFB2DE0400_Qry" %>
<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date1").datepicker({ dateFormat: 'yy/mm' });
            $('.ymd').mask('9999/99/99');
            $('.ym').mask('9999/99');
            $.unblockUI();

        }

        //清空畫面
        function ClearAll() {
            $('#txt_MANAGER_YM_S').val("");
            $('#txt_MANAGER_YM_E').val("");
            $('#txt_MANAGER_YM').val("");
            $("#ddl_RESTAURANT_CD").val("-1");
            $("#ddl_DOCUMENT_CD").val("-1");
        }

        function CheckSearch() {


            return false;
        }

        //儲存前檢查
        function saveCheck(msg) {
            var processed = true;
            if ($("#ddl_DOCUMENT_CD").val() == '1') {
                if (Page_ClientValidate("GroupA")) {
                    $.unblockUI();
                } else
                    processed = false;

            } else if ($("#ddl_DOCUMENT_CD").val() == '2') {
                if (Page_ClientValidate("GroupB")) {
                    //BlockUI();
                } else
                    processed = false;
            }

            if (processed) {
                processed = confirm("確定要進行" + msg);
                BlockUI();
            }

            if (!processed) {
                $.unblockUI();
            }
            
            return processed;
        }

        function CheckYM(source, arguments) {
            if ($("#txt_MANAGER_YM_S").val() != "" && $("#txt_MANAGER_YM_E").val() != "") {
                if ($("#txt_MANAGER_YM_E").val() < $("#txt_MANAGER_YM_S").val())
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }

        }

        function ShowRecord(obj) {
            
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <COL width="10%" />
					<COL width="30%" />
					<COL width="15%" />
					<COL width="45%" />		
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_RESTAURANT_CD" runat="server" Text="<%$Resources:Resource,wfb2de_lb_RESTAURANT_CD%>"></asp:Label></th>
                        <td align="left" class="Body_label"  >
                            <asp:DropDownList ID="ddl_RESTAURANT_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>                            
                        </td>   
                        <th></th>
                        <td></td>               
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DOCUMENT_CD" runat="server" Text="<%$Resources:Resource,wfb2de_lb_DOCUMENT_CD%>"></asp:Label></th>
                        <td align="left" class="Body_label"  >
                            <asp:DropDownList ID="ddl_DOCUMENT_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>                            
                        </td>   
                        <th></th>
                        <td></td>               
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MANAGER_DT" runat="server" Text="<%$Resources:Resource,wfb2de_MANAGER_DT%>"></asp:Label></th>
                        <td class="Body_label" colspan="3"> 
                            <asp:TextBox ID="txt_MANAGER_YM_S" runat="server" MaxLength="10" Width="81px" CssClass="MandatoryField date ymd" ClientIDMode="Static" ></asp:TextBox>
                                    ~  
                            <asp:TextBox ID="txt_MANAGER_YM_E" runat="server" MaxLength="10" Width="81px" CssClass="MandatoryField date ymd" ClientIDMode="Static" ></asp:TextBox>(選擇日度報表時為必填)
                            <asp:CustomValidator ID="qrytest1" runat="server" ValidateEmptyText="true"
                                 ErrorMessage="<%$Resources:Resource,wfb2de_error_MANAGER_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_MANAGER_YM_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                              <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                 ErrorMessage="<%$Resources:Resource,wfb2de_error_MANAGER_DT_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_MANAGER_YM_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_required_MANAGER_YM_S%>"
                                            ControlToValidate="txt_MANAGER_YM_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                           
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_required_MANAGER_YM_E%>"
                                            ControlToValidate="txt_MANAGER_YM_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                           
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2de_greaterthan_MANAGER_DT%>" ClientValidationFunction="CheckYM" ForeColor="Red"
                                            ControlToValidate="txt_MANAGER_YM_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <%-- 
                             <uc1:UCDateTimeRange runat="server" ID="UCDateTimeRange" StartDateMaxLength="10" ClientIDMode="Static" ControlClientIDMode="Static" EndDateMaxLength="10"
                                            EndDateValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])[/ /.](0[1-9]|[12][0-9]|3[01])"
                                            StartDateValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])[/ /.](0[1-9]|[12][0-9]|3[01])"
                                            CompareValidatorOperator="GreaterThanEqual" CompareValidatorErrMesg="<%$Resources:Resource,wfb2de_greaterthan_BONUS_SDT%>" ValidatorStartDateErrMesg="<%$Resources:Resource,wfb2de_error_BONUS_SDT_start%>"
                                            ValidatorEndDateErrMesg="<%$Resources:Resource,wfb2de_error_BONUS_SDT_end%>" ValidationGroup="GroupA" />  
							--%>									
						</td> 
                        <th></th>
                        <td></td>               
                    </tr>
                    <tr>
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MANAGER_YM" runat="server" Text="<%$Resources:Resource,wfb2de_lb_MANAGER_YM%>"></asp:Label></th>
                        <td align="left" colspan="3">                           	
                           <asp:TextBox ID="txt_MANAGER_YM" CssClass="MandatoryField date1 ym" runat="server" MaxLength="6" Width="81px" ClientIDMode="Static"></asp:TextBox>(選擇月度報表時為必填)	
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_required_MANAGER_YM%>"
                                ControlToValidate="txt_MANAGER_YM" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                          
                           <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                ErrorMessage ="<%$Resources:Resource,wfb2de_error_MANAGER_YM%>" ControlToValidate="txt_MANAGER_YM" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                       </td>                                       
                    </tr>                      
                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <aces:Btn ID="WFB2DE0400ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0400ExcelDown%>" OnClientClick="return saveCheck(this.value);" OnClick="WFB2DE0400ExcelDown_Click" />

                                <%--<asp:Button ID="WFB2DE0400ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0400ExcelDown%>" OnClientClick="return saveCheck(this.value);" OnClick="WFB2DE0400ExcelDown_Click" />--%>
                                
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2de_btn_clear%>" onclick="ClearAll();"/>                               
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
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DE0400ExcelDown" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

