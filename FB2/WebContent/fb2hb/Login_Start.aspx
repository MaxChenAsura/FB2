<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/Login_Start.aspx.cs" Inherits="WebContent_fb2hb_Login_Start" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <title></title>
    <script type="text/javascript" src="../../Scripts/Basic.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $("#txt_JOIN_DT_2").val(getTodayDate());
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
        }

        function ReturnValue(json) {
            //if (window.opener != undefined) {
            //    //for chrome
            //    window.opener.returnValue = json;
            //}
            //else {
            //    window.returnValue = json;
            //}


            //window.close();        
           
            
            window.parent.returnHB070(json);

            window.parent.$("#div_iframeID").dialog('close');

        }

        //清空畫面
        function ClearAll() {

            $("#txt_VENDOR_MEMBER_NO").val("");
            $("#txt_VENDOR_MEMBER_NAME").val("");

        }

        function closeWindow() {
            if (confirm('您確定要離開登錄作業?')) {
                window.parent.$("#div_iframeID").dialog('close');
            }
        }

        //確定前檢查
        function doSave() {

            if (Page_ClientValidate("GroupA")) {
                processed = confirm('確定要啟動登錄作業?\r\n [!!!已報到人員將新增至員工人事主檔] \r\n [!!!不報到人員將新增至不報到人員檔]');
            }
            else
                processed = false;

            if (!processed) {
                $.unblockUI();
                return;
            }

            return processed;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <table cellspacing="1" cellpadding="1" width="300px" border="0">
            <tr>
                <%--入社日期--%>
               <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2hb070_lb_join_dt%>"></asp:Label>:</th>
                <td class="Body_TR1"">
                    <asp:TextBox ID="txt_JOIN_DT_2" runat="server" MaxLength="10" Width="100px" CssClass="MandatoryField date"></asp:TextBox>
                     <asp:HiddenField id="hid_is_check" runat="server" ClientIDMode="Static" value="" />
                     <asp:HiddenField id="hid_join_dt" runat="server" ClientIDMode="Static" value="" />

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb070_required_join_dt%>"
                    ControlToValidate="txt_JOIN_DT_2" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                      <!--驗證日期格式-->
						<asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
						ErrorMessage="<%$Resources:Resource,wfb2hb070_error_join_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
						ControlToValidate="txt_JOIN_DT_2" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>		
                </td>
            </tr>
           
            <tr>
                <td align="right" class="Body_label">
                     <asp:Button ID="Button1" runat="server" Text="確定" OnClientClick="return doSave();" OnClick="Button1_Click" ValidationGroup="GroupA" />
                     <asp:Button ID="Button2" runat="server" Text="取消" OnClientClick="closeWindow();" />                    
                </td>
            </tr>         
        </table>
        <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    </form>
</body>
</html>
