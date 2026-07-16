<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ib/WFB2IB0400_Qry.aspx.cs" Inherits="WebContent_fb2ib_WFB2IB0400_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            $('.ym').mask('9999/99');
            $('.date').datepicker({ dateFormat: 'yy/mm' });
            iniForm();
        });
        function iniForm() {

            $.unblockUI();

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

            //檢查是否做過月度結算
            $.ajax({
                url: "WFB2IB0400_CheckSALARY_MONTH.ashx",
                data: {
                    txt_SALARY_YM: $('#txt_SALARY_YM').val()

                },
                type: "GET",
                dataType: 'text',
                async: false,
                success: function (result) {
                    if (result == "Y")
                        processed = confirm("該期間已經有資料，是否要繼續執行？");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
            
            if (processed) {
                if (Page_ClientValidate("GroupA")) {
                    BlockUI();
                } else
                    processed = false;
            }


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
                         <th align="left" class="Body_TableHeader">
                             <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_SALARY_YM%>"></asp:Label>：	
                         </th>
                         <td align="left">
                             
                             <asp:TextBox ID="txt_SALARY_YM" CssClass="MandatoryField ym date" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static"></asp:TextBox>	
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_required_SALARY_YM%>"
                                  ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>                                           
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                  ErrorMessage="<%$Resources:Resource,wfb2ib_error_SALARY_YM%>" ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA"
                                  ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                         </td>
                         <th></th>
                         <td align="right">
                             <aces:Btn ID="WFB2IB0400Execute" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0400Execute%>" OnClick="WFB2IB0400Execute_Click" OnClientClick="return saveCheck()"/>				

                             <%--<asp:Button ID="WFB2IB0400Execute" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0400Execute%>" OnClick="WFB2IB0400Execute_Click" OnClientClick="return saveCheck()"/>--%>				
                         </td>                                       
                    </tr>                    
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lb_REMARK" runat="server"  Width="800px" ClientIDMode="Static" ForeColor="Red" Text="請在所輸入計算年月的薪資年月轉出傳票時才可按此機能，否則將造成IB050計算調整時的格式錯誤"></asp:Label> 
                        </td>
                    </tr>
            </table>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>