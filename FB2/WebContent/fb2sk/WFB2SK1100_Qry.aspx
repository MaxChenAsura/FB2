<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sk/WFB2SK1100_Qry.aspx.cs" Inherits="WebContent_fb2sk_WFB2SK1100_Qry"  Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".year").datepicker({ dateFormat: 'yy' });
            $(".year").mask("9999");           
            $.unblockUI();
        }

        function checkConfirm() {

            history.back(-2);
        }
        function MyCheckValid(msg) {
            //setTimeout("MydelayedShow()", 1);
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                $.unblockUI();
            } else {
                processed = false;
                return processed;
            }

            BlockUI();
            processed = confirm("確定要進行" + msg);
            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }

        function MydelayedShow() {
            if (Page_IsValid != null && Page_IsValid == true) {
                $.unblockUI();
            }
        }

        //function toRedirect() {
        //    window.location.href = "WFB2HE0100_Qry.aspx";
        //}


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
        <colgroup>
            <col width="15%" />
            <col width="55%" />
            <col width="30%" />
        </colgroup>
        <tbody>            
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sk_lb_year%>"></asp:Label>:              
                </th>
                <td>
                    <asp:TextBox ID="txt_YEAR" runat="server" MaxLength="4" Width="81px" ClientIDMode="Static" CssClass="year MandatoryField">></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sk_required_YEAR%>"
                         ControlToValidate="txt_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>  
                </td>
            </tr>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_UploadExcel" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_UploadExcel%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label">
                    <asp:FileUpload ID="FileUpload1" runat="server" class="MandatoryField" Width="600px" />                
                </td>

                <td align="right" class="Body_label">
                    <%--                                     
                        <aces:Btn ID="WFB2SK1100Upload" runat="server" Text="上傳" OnClick="WFB2SK1100Upload_Click" OnClientClick="return MyCheckValid(this.value);"/>
                        <asp:Button ID="WFB2SK1100Upload" runat="server" Text="上傳" OnClick="WFB2SK1100Upload_Click" OnClientClick="return MyCheckValid(this.value);"/>                                   
                    --%>
                     <aces:Btn ID="WFB2SK1100Upload" runat="server" Text="上傳" OnClick="WFB2SK1100Upload_Click" OnClientClick="return MyCheckValid(this.value);"/>                  
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_lb_UploadExcel_isNull%>"
                        ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupA"
                        ErrorMessage="<%$Resources:Resource,wfb2_format_excel%>" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(xls|XLS|xlsx|XLSX)$" Display="None"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <br>
                </td>
            </tr>
            <tr>
                <td>
                    <br>
                </td>
            </tr>
            <tr>
                <td>
                    <br>
                </td>
            </tr>
            <tr>
                <td align="left" class="Body_label" colspan="5">

                    <fieldset style="padding:5px">
                        <legend class="Body_label">
                            <asp:Label ID="lb_TemplateDownload" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TemplateDownload%>"></asp:Label>

                        </legend>
                        <table align="right">
                            
                            <tr>
                                <td>
                                    <p style="color: red;">
                                        <asp:Label ID="lb_type" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_type%>"></asp:Label>:

                                    </p>
                                </td>
                                <td>
                                   <asp:Button ID="WFB2HE0100ExcelDown" runat="server" Text="EXCEL匯出" OnClick="btn_Excel_Down_Click" />

                                </td>
                            </tr>
                        </table>

                    </fieldset>
                </td>
            </tr>
            <tr>
                <td align="left" class="Body_label"></td>
                <td align="left" class="Body_label"></td>
            </tr>
        </tbody>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>