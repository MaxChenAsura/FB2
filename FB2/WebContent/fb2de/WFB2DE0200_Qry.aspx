<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2de/WFB2DE0200_Qry.aspx.cs" Inherits="WebContent_fb2de_WFB2DE0200_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_MANAGER_DT_NOW_S").mask("9999/99/99");
            $("#txt_MANAGER_DT_NOW_E").mask("9999/99/99");

            $.unblockUI();
           
        }       

        //清空畫面
        function ClearAll() {
            $('#txt_MANAGER_DT_NOW_S').val("");
            $('#txt_MANAGER_DT_NOW_E').val("");

        }
        

        //儲存前檢查
        function saveCheck() {
            var processed = true;
            //alert($("#hid_Valid_Flag").val());
            if (!Page_ClientValidate("GroupA")) {
                return;
            }
            
            //只能輸入間隔7天
            var a1 = new Date( $('#txt_MANAGER_DT_NOW_S').val());
            var a2 = new Date( $('#txt_MANAGER_DT_NOW_E').val());
            var gap = a2.getTime() - a1.getTime();
            if (Math.floor(gap / (1000 * 60 * 60 * 24)) > 7) {
                alert("本回結算日期起迄日不可超過七天!!");
                return false;
            }
                        

            //檢查是否做過月度結算
            $.ajax({
                url: "WFB2DE0200_CheckMonth.ashx",
                data: {
                    START_DT: $('#txt_MANAGER_DT_NOW_S').val(),
                    END_DT: $('#txt_MANAGER_DT_NOW_E').val()
                },
                type: "GET",
                dataType: 'text',
                async: false,
                success: function (result) {
                    if (result == "Y")
                        processed = confirm("此範圍條件內含已月結資料, 是否確認執行？");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });

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

        function CheckYM(source, arguments) {
            if ($("#txt_MANAGER_DT_NOW_S").val() != "" && $("#txt_MANAGER_DT_NOW_E").val() != "") {
                if ($("#txt_MANAGER_DT_NOW_E").val() < $("#txt_MANAGER_DT_NOW_S").val())
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <COL width="15%" />
					<COL width="30%" />
                    <COL width="30%" />
                    <COL width="25%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MANAGER_DT" runat="server" Text="<%$Resources:Resource,wfb2de_lb_MANAGER_DT%>"></asp:Label></th>
                        <td align="left" class="Body_label"  >
                            <asp:TextBox ID="txt_MANAGER_DT" runat="server" MaxLength="10" Width="80px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>                            
                        </td>   
                        <th></th>
                        <td></td>               
                    </tr>
                    <tr>
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MANAGER_DT_NOW" runat="server" Text="<%$Resources:Resource,wfb2de_lb_MANAGER_DT_NOW%>"></asp:Label>:
                         </th>
                         <td align="left" class="Body_label">                        
                            <asp:TextBox ID="txt_MANAGER_DT_NOW_S" runat="server" MaxLength="10" Width="81px" CssClass="MandatoryField date" ClientIDMode="Static" ></asp:TextBox>
                                    ~  
                            <asp:TextBox ID="txt_MANAGER_DT_NOW_E" runat="server" MaxLength="10" Width="81px" CssClass="MandatoryField date" ClientIDMode="Static" ></asp:TextBox>
                            <asp:CustomValidator ID="qrytest1" runat="server" ValidateEmptyText="true"
                                 ErrorMessage="<%$Resources:Resource,wfb2de_error_MANAGER_DT_NOW_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_MANAGER_DT_NOW_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                              <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                 ErrorMessage="<%$Resources:Resource,wfb2de_error_MANAGER_DT_NOW_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_MANAGER_DT_NOW_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                             
                             
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_required_MANAGER_DT_NOW_S%>"
                                            ControlToValidate="txt_MANAGER_DT_NOW_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_required_MANAGER_DT_NOW_E%>"
                                            ControlToValidate="txt_MANAGER_DT_NOW_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2de_greaterthan_CALCULATE_DT%>" ClientValidationFunction="CheckYM" ForeColor="Red"
                                            ControlToValidate="txt_MANAGER_DT_NOW_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>                             
                        </td>                       
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <aces:Btn ID="WFB2DE0200Execute" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0200Execute%>" OnClick="WFB2DE0200Execute_Click" OnClientClick="return saveCheck();" />
                                
                                <%--<asp:Button ID="WFB2DE0200Execute" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0200Execute%>" OnClick="WFB2DE0200Execute_Click" OnClientClick="return saveCheck();" />--%>
                                
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

