<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ss/WFB2SS0100_Qry.aspx.cs" Inherits="WebContent_WFB2SS0100_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //$.unblockUI();
            iniForm();
        });
        var li_id;
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $.unblockUI();
        }

        //執行檢核
        function executeCheck() {
            var processed = false;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
                processed = true;
            } else {
                return processed;
            }
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        function MyCheckValid(msg) {
            var month = "";
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                $.unblockUI();
            } else {
                processed = false;
                return;
            }
            BlockUI();
            processed = confirm("確定要進行" + msg+"?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        function doClear() {
            $("#txt_SALARY_DT").val("");
            $("#ddl_FIRED_TYPE").val("");
            $("#FileUpload1").val("");
            return false;
        }

        function CheckThanDate(source, arguments) {
            var value = $.trim(arguments.Value);
            if (value == "") {
                arguments.IsValid = true;
                return;
            }
            var sys_day = new Date();
            var salary_day = new Date(value);
            if (salary_day <= sys_day) {
                arguments.IsValid = false;
                return;
            }
        }

        function IsIntText() {
            var charkeycode = window.event.keyCode;
            if (charkeycode > 47 && charkeycode < 58) {  //0的keycode為47 ,9的keycode為58
                return true;
            }
            return false;

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
            <!--頁面table-->
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="20%" />
                                <col width="25%" />
                                <col width="10%" />
                                <col width="40%" />
                                <col width="5%" />
                            </colgroup>
                            <tbody>
                                  <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT" runat="server" Text="發薪日期"></asp:Label></th>
                                    <td align="left" class="Body_label"  >
                                        <asp:TextBox ID="txt_SALARY_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>                                          
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="發薪日期必輸入"
                                            ControlToValidate="txt_SALARY_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                         ErrorMessage="發薪日期需大於系統日" ClientValidationFunction="CheckThanDate" ForeColor="Red"
                                         ControlToValidate="txt_SALARY_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator> 
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                         ErrorMessage="發薪日期格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                         ControlToValidate="txt_SALARY_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>                          
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_FIRED_TYPE" runat="server" Text="資遺類型"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                           <asp:DropDownList ID="ddl_FIRED_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField" ></asp:DropDownList>       
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="資遺類型必挑選"
                                            ControlToValidate="ddl_FIRED_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>                                
                                <tr>
                                    <%--EXCEL上傳檔案 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_UploadExcel" runat="server" Text="<%$Resources:Resource,UploadExcel%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:FileUpload ID="FileUpload1" runat="server" class="MandatoryField" Width="500px" />             
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,FB2SS_UploadExcel_isNull%>"
                                        ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupA"
                                        ErrorMessage="<%$Resources:Resource,wfb2_format_excel%>" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(xlsx|XLSX)$" Display="None"></asp:RegularExpressionValidator>
                                    </td>                    
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label" >
                                        <div id="init">
                                            <aces:Btn ID="WFBSS0100Upload" runat="server" Text="上傳計算" OnClick="WFBSS0100Upload_Click" OnClientClick="return MyCheckValid(this.value);"/>                        
                                             <%-- 
                                                 <asp:Button ID="WFBSS0100Upload" runat="server" Text="上傳計算" OnClick="WFBSS0100Upload_Click" OnClientClick="return MyCheckValid(this.value);"/>                        
                                            --%>                                            
                                           
                                            <asp:Button ID="WFB2SS0100ExcelDown" runat="server" Text="EXCEL匯出" OnClick="btn_Excel_Down_Click" />
                                            <asp:Button ID="btn_clear" runat="server" Text="清除" OnClientClick="return doClear();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>

            <asp:HiddenField ID="hid_defalut_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_defalut_EMP_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_defalut_RETIRE_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_sys_RETIRE_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_sys_date" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />

</asp:Content>
