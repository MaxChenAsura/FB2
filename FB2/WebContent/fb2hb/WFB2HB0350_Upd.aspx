<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0350_Upd.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0350_Upd" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $("#txt_EMP_NAME").attr("readonly", true);
          
            $.unblockUI();
        }

        function checkConfirm() {
            var result = confirm("是否確定取消?");
            return result;
        }

        //清空畫面
        function ClearAll() {
           
        }

        function CheckSearch() {

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return false;
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
            <table cellspacing="1" cellpadding="1" width="1020" border="0"
                class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="30%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />

                </colgroup>
                <tbody>
                      <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_HR_CHG_NO%>" ></asp:Label>:</th>
                        <td align="left" class="Body_label">
                             <asp:Label ID="lb_HR_CHG_NO" runat="server" ></asp:Label></th>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="42px" BorderWidth="0" ClientIDMode="Static" readonly="true"></asp:TextBox>
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="50px" BorderWidth="0" ClientIDMode="Static" readonly="true"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PLAN_END_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_PLAN_END_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="預計結束日期錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_PLAN_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="預計結束日期必輸入"
                             ControlToValidate="txt_PLAN_END_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None" ></asp:RequiredFieldValidator>
                        </td>
                        <th></th>
                    </tr>  
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_END_DT" runat="server" Text="<%$Resources:Resource,wfb2dl_END_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_END_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:CustomValidator ID="cv_END_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="實際結束日期錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>
                        <th></th>
                    </tr>  

                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                 <asp:Button ID="btn_Save" runat="server" Text="儲存" OnClick="btn_Save_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();"  />
                                 <asp:Button ID="btn_back" runat="server" Text="取消" OnClick="btn_back_Click" OnClientClick="return checkConfirm();"/>
                            </div>
                        </td>

                    </tr>
                 
                </tbody>
            </table>           
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
