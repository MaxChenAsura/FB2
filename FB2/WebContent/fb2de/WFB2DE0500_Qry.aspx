<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2de/WFB2DE0500_Qry.aspx.cs" Inherits="WebContent_fb2de_WFB2DE0500_Qry" %>

<%@ Register Src="~/UserControl/UCCommCodeDropDwonList.ascx" TagPrefix="uc1" TagName="UCCommCodeDropDwonList" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $('.date').datepicker({ dateFormat: 'yy/mm' });
            $('#txt_MANAGER_YM').mask('9999/99');
            $.unblockUI();

        }

        //jQuery(document).ready(function () {
        //    $('.date').datepicker({ dateFormat: 'yy/mm' });
        //    $('#txt_MANAGER_YM').mask('9999/99');
        //});

        function checkvalue() {
            var processed = true;            

            if (!Page_ClientValidate("GroupA")) {
                processed = false;
            }
            else if (!Page_ClientValidate("GroupB")) {
                processed = false;
            } else {
                BlockUI();                
            }
            if (!processed)
                $.unblockUI();
           

            return processed; 

        }

        //清空畫面
        function ClearAll() {
            $('#txt_MANAGER_YM').val("");
            $('#FileUpload').val("");

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    
    <table class="Body_Label" width="1048px">
        <tr>
            <td class="Body_Label" style="TEXT-ALIGN: left; background-color: #808080; width: 100px; height: 30px">
                <asp:Label ID="lb_MANAGER_YM" runat="server" Text="<%$Resources:Resource,wfb2de_MANAGER_YM%>" ForeColor="White"></asp:Label>
            </td>
            <td class="Body_Label" style="TEXT-ALIGN: left; width: 70px">
                <asp:TextBox ID="txt_MANAGER_YM" runat="server" MaxLength="7" Style="text-align: left" CssClass="MandatoryField date" ClientIDMode="Static" Width="70px"></asp:TextBox>
            </td>
            <asp:RequiredFieldValidator ID="Validator_MANAGER_YM_NotNull" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_MANAGER_YM_NotNull%>" ControlToValidate="txt_MANAGER_YM"
                ForeColor="Red" Display="None" ValidationGroup="GroupA">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                ErrorMessage="<%$Resources:Resource,wfb2de_MANAGER_YM_EER%>" ControlToValidate="txt_MANAGER_YM" ForeColor="Red"
                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
            </asp:RegularExpressionValidator>
            <td>
            </td>
        </tr>
         <tr>
            <th align="left" class="Body_TableHeader">
                <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2dd_lb_PLANT_CD%>"></asp:Label>:</th>
            <td align="left" class="Body_label">
                <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
            </td>
           
            <td>
            </td>
        </tr>
        <tr>
            <th align="left" class="Body_TableHeader">
                <asp:Label ID="lb_RES_UPLOAD_FILE" runat="server" Text="<%$Resources:Resource,wfb2de_lb_RES_UPLOAD_FILE%>"></asp:Label>：
            </th>
            <td align="left" class="Body_label">
                <asp:FileUpload ID="FileUpload" runat="server" Width="600px" ClientIDMode="Static" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileUpload" ForeColor="Red" ValidationGroup="GroupB"
                     ErrorMessage="<%$Resources:Resource,wfb2_format_excel%>" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(xls|XLS|xlsx|XLSX)$" Display="None"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="FileUpload" ForeColor="Red" ValidationGroup="GroupB"
                     ErrorMessage="<%$Resources:Resource,wfb2_required_excel%>" Display="None"></asp:RequiredFieldValidator>                                                          
            </td>                                      
        </tr>
        <tr>
            <th></th>
            <th></th>
            <td class="Body_Label" style="TEXT-ALIGN: right">
                <aces:Btn ID="WFB2DE0500ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0500ExcelDown%>" OnClick="WFB2DE0500ExcelDown_Click" />
                <aces:Btn ID="WFB2DE0500Upload" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0500Upload%>" OnClientClick ="return checkvalue();" OnClick="WFB2DE0500Upload_Click"  />

                <%-- 
                

                     <asp:Button ID="WFB2DE0500ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0500ExcelDown%>" OnClick="WFB2DE0500ExcelDown_Click" />
                <asp:Button ID="WFB2DE0500Upload" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0500Upload%>" OnClientClick ="return checkvalue();" OnClick="WFB2DE0500Upload_Click" />

                --%>                
                
                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2de_btn_clear%>" onclick="ClearAll();"/>                
            </td>
        </tr>
    </table>


    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
</asp:Content>

