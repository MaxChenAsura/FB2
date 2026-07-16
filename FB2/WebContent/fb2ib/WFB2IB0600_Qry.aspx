<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ib/WFB2IB0600_Qry.aspx.cs" Inherits="WebContent_fb2ib_WFB2IB0600_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            $('.ym').mask('9999/99');
            iniForm();
        });
        function iniForm() {
            $("#txt_YEAR").mask('999');
            gridviewScroll();
            $.unblockUI();           
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
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

        function checkDowning(msg) {
            var processed = true;

            processed = confirm("確定要進行" + msg + "?");
            //if (processed) {
            //    BlockUI();
            //}
            return processed;
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

        function uploadCheck(msg) {
            var processed = true;
            //檢查是否有選擇檔案            
            if (!Page_ClientValidate("GroupB")) {
                $.unblockUI();
                return false;
            } else {
                processed = true;
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
                        <td align="left" class="Body_label" colspan="5">
                            <fieldset style="padding:5px">
                                <legend class="Body_label">
                                    <asp:Label ID="lb_BOSS_UPLOAD" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_BOSS_UPLOAD%>"></asp:Label>
                               </legend>
                                <table align="left">
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_BOSS_UPLOAD_FILE" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_BOSS_UPLOAD_FILE%>"></asp:Label>：
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:FileUpload ID="FileUpload1" runat="server" class="MandatoryField" Width="600px" ClientIDMode="Static" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupB"
                                                ErrorMessage="<%$Resources:Resource,wfb2_format_excel%>" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(xls|XLS|xlsx|XLSX)$" Display="None"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupB"
                                                ErrorMessage="<%$Resources:Resource,wfb2_required_excel%>" Display="None"></asp:RequiredFieldValidator>                                                          
                                        </td>                                      
                                    </tr>                                   
                                </table>
                                <table align="right">
                                    <tr>                                        
                                        <td>          
                                        <aces:Btn ID="WFB2IB0600Upload" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0600Upload%>" OnClick="WFB2IB0600Upload_Click" OnClientClick="return uploadCheck(this.value);" />			
                                            <aces:Btn ID="WFB2IB0601Download" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0601Download%>" OnClick="WFB2IB0600Download_Click" OnClientClick="return checkDowning(this.value);"/>
                                                                             
                                            <%--<asp:Button ID="WFB2IB0600Upload" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0600Upload%>" OnClick="WFB2IB0600Upload_Click" OnClientClick="return uploadCheck(this.value);" />			
                                            <asp:Button ID="WFB2IB0601Download" runat="server" Text="<%$Resources:Resource,wfb2ib_WFB2IB0601Download%>" OnClick="WFB2IB0600Download_Click" OnClientClick="return checkDowning(this.value);"/>--%>
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
                        <td colspan="5">                  
                            <table align="left">
                                <tr>
                                    <th align="left" class="Body_TableHeader" width="120px">
                                        <asp:Label ID="lb_DownLoad_Year" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_DownLoad_Year%>"></asp:Label>：
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_YEAR" CssClass="MandatoryField" runat="server" MaxLength="3" Width="81px" ClientIDMode="Static"></asp:TextBox>
                                        (<asp:Label ID="lb_Year" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_Year%>" />) 
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_required_txt_YEAR%>"
                                                ControlToValidate="txt_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>                                                                     
                                    </td>                                                                                               
                                 </tr>
                                <tr>
                                    <th colspan="3">
                                        <asp:Label ID="lb_Describtion" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_Describtion%>"></asp:Label>
                                    </th>
                                </tr>                                   
                            </table>
                            <table align="right">
                                 <tr>                                        
                                     <td>
                                     <aces:Btn ID="WFB2IB0600Download" runat="server" ValidationGroup="GroupA" Text="<%$Resources:Resource,wfb2ib_WFB2IB0600Download%>" OnClick="WFB2IB0600Download_Click1" />			
                                         <aces:Btn ID="WFB2IB0600Search" runat="server" ValidationGroup="GroupA" Text="<%$Resources:Resource,wfb2ib_WFB2IB0600Search%>" OnClick="WFB2IB0600Search_Click" />

                                        <%-- <asp:Button ID="WFB2IB0600Download" runat="server" ValidationGroup="GroupA" Text="<%$Resources:Resource,wfb2ib_WFB2IB0600Download%>" OnClick="WFB2IB0600Download_Click1" />			
                                         <asp:Button ID="WFB2IB0600Search" runat="server" ValidationGroup="GroupA" Text="<%$Resources:Resource,wfb2ib_WFB2IB0600Search%>" OnClick="WFB2IB0600Search_Click" />--%>
                                     </td>
                                 </tr>                                   
                            </table>
                        </td>
                    </tr>                                  
                   
                    <!-- END: Create a line -->
                </tbody>
            </table>          
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2IB0600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_YEAR"
                        Name="PAYMENT_DATE" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />                   
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px" 
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
                <Columns>                    
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2ib_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="40px"/>
                    <asp:BoundField DataField="PAYMENT_DATE" HeaderText="<%$Resources:Resource,wfb2ib_lb_PAYMENT_DATE%>" SortExpression="PAYMENT_DATE" HeaderStyle-Width="333px" />
                    <asp:BoundField DataField="INS_KIND" HeaderText="<%$Resources:Resource,wfb2ib_lb_INS_KIND%>" HeaderStyle-Width="333px" />
                    <asp:BoundField DataField="INS_COST" HeaderText="<%$Resources:Resource,wfb2ib_lb_INS_COST%>" HeaderStyle-Width="333px" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:n0}" />
                   
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />


            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2IB0600Upload"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2IB0600Download"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2IB0601Download"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>   
    
</asp:Content>