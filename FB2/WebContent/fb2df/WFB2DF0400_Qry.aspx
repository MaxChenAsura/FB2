<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2df/action/WFB2DF0400_Qry.aspx.cs" Inherits="WebContent_fb2df_WFB2DF0400_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {            
            iniForm();
        });
        function iniForm() {           
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_MANAGER_DT").mask("9999/99/99");
            $.unblockUI();
           
        }
        

        //清空畫面
        function ClearAll() {
            //$('#ddl_SYS_CD').val(-1);
            $("#txt_MANAGER_DT").val("");

        }

        function CheckSearch() {

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return false;
        }

        function checkDowning(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else {
                    return false;
            }
           
            processed = confirm("確定要進行" + msg+"?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        //儲存前檢查
        function saveCheck() {
            var processed = true;
            //alert($("#hid_Valid_Flag").val());

            if ($("#hid_Valid_Flag").val() == "Y") {
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
                    <col width="15%" />
					<col width="35%" />
					<col width="10%" />
					<col width="40%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->                    
                    <tr>
                        <th align="left" class="Body_TableHeader" >                            
                           <asp:Label ID="lb_MANAGER_DT" runat="server" Text="<%$Resources:Resource,wfb2df_lb_MANAGER_DT%>">:</asp:Label> 
                        </th>                        
                        <td align="left" class="Body_label">
                           <asp:TextBox ID="txt_MANAGER_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_required_MANAGER_DT%>"
                                            ControlToValidate="txt_MANAGER_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="qrytest1" runat="server" ValidateEmptyText="true"
                                 ErrorMessage="<%$Resources:Resource,wfb2df04_ERR_MANAGER_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_MANAGER_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>                 
                                                       
                        </td>                                       
                    </tr>
                               
                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                            <aces:Btn ID="WFB2DF0400ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0400ExcelDown%>" ValidationGroup="GroupA" OnClick="WFB2DF0400ExcelDown_Click" OnClientClick="return checkDowning(this.value);"/>
                                <aces:Btn ID="WFB2DF0400TxtDown" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0400TxtDown%>"  ValidationGroup="GroupA" OnClick="WFB2DF0400TxtDown_Click" />

<%--                                <asp:Button ID="WFB2DF0400ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0400ExcelDown%>" ValidationGroup="GroupA" OnClick="WFB2DF0400ExcelDown_Click" OnClientClick="return checkDowning(this.value);"/>
                                <asp:Button ID="WFB2DF0400TxtDown" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0400TxtDown%>"  ValidationGroup="GroupA" OnClick="WFB2DF0400TxtDown_Click" />--%>
                                
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2df_btn_clear%>" onclick="ClearAll();"/>                               
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
          
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DF0400ExcelDown"></asp:PostBackTrigger>
        </Triggers>
        <Triggers >
            <asp:PostBackTrigger ControlID="WFB2DF0400TxtDown" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>