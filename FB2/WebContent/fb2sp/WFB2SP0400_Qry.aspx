<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sp/WFB2SP0400_Qry.aspx.cs" Inherits="WebContent_WFB2SP0400_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //$.unblockUI();
            iniForm();
        });
        var li_id;
        function iniForm() {
            $.unblockUI();
            //日期格式心須
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $('#txt_EMP_NAME').attr("readonly", true);
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                $('#txt_RETIRE_DT').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));
                                
                                if ($("#ddl_COMPUTER_TYPE").val() == "A") {
                                    //系統日
                                    $('#txt_RETIRE_DT').val($('#hid_sys_date').val());
                                    $('#hid_sys_RETIRE_DT').val($.trim(JData.LEAVE_DT))
                                } else {
                                    //退休日
                                    $('#txt_RETIRE_DT').val($.trim(JData.LEAVE_DT));
                                    $('#hid_sys_RETIRE_DT').val($.trim(JData.LEAVE_DT))
                                }
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME').val("");
                }
            });
        }

        //判斷是否為數字
        function IsIntText() {
            var charkeycode = window.event.keyCode;
            if (charkeycode > 47 && charkeycode < 58) {  //0的keycode為47 ,9的keycode為58
                return true;
            }
            return false;

        }
        function addzero(input) {
            if (input.value.length == 1) input.value = '0' + input.value;
            return input;
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

        function doClear() {
            $("#ddl_COMPUTER_TYPE").val("A");
            $("#txt_EMP_ID").val($("#hid_defalut_EMP_ID").val());
            $("#txt_EMP_NAME").val($("#hid_defalut_EMP_NAME").val());
            $("#txt_RETIRE_DT").val($("#hid_defalut_RETIRE_DT").val());            
            $("#txt_SPECIAL_PAY").val("");
            $("#txt_OTHER_PAY").val("");
            $("#txt_STOP_YY").val("");
            $("#txt_STOP_MM").val("");
            $("#txt_STOP_DD").val("");
            return false;
        }




    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--頁面table-->
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>

                                    <th align="left" class="Body_TableHeader">
                                        <%--計算類別--%>
                                        <asp:Label ID="lb_COMPUTER_TYPE" runat="server" Text="計算類別"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_COMPUTER_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField" OnTextChanged="ddl_COMPUTER_TYPE_TextChanged" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--工號--%>
                                    <th align="left" class="Body_TableHeader">                                        
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="工號"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5" CssClass="MandatoryField"> </asp:TextBox>
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="工號不可空白"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <%--退休日--%>
                                <tr>
                                    <th align="left" class="Body_TableHeader">                                        
                                        <asp:Label ID="lb_RETIRE_DT" runat="server" Text="退休日"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_RETIRE_DT" runat="server" Width="100px" ClientIDMode="Static" CssClass="date" Enabled="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="此人尚未退休"
                                            ControlToValidate="txt_RETIRE_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <%--特勤津貼 --%>
                                <tr>
                                    <th  align="left" class="Body_TableHeader">
                                         <asp:Label ID="lb_SPECIAL_PAY" runat="server" Text="<%$Resources:Resource,wfb2sp_SPECIAL_PAY%>"></asp:Label>：	
                                    </th>
                                    <td align="left">                                           
                                        <asp:TextBox ID="txt_SPECIAL_PAY" runat="server" MaxLength="7" Width="81px" Style="text-align: right;ime-mode:disabled" ClientIDMode="Static" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>                                                                      
                                    </td>
                                </tr>
                                <%--其他津貼 --%>
                                <tr>
                                    <th  align="left" class="Body_TableHeader">
                                         <asp:Label ID="lb_OTHER_PAY" runat="server" Text="<%$Resources:Resource,wfb2sp_OTHER_PAY%>"></asp:Label>：	
                                    </th>
                                    <td align="left">                                           
                                        <asp:TextBox ID="txt_OTHER_PAY" runat="server" MaxLength="7" Width="81px" Style="text-align: right;ime-mode:disabled" ClientIDMode="Static" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>                                                                        
                                    </td>
                                </tr>
                                <%--(留停)年--%>
                                <tr>                                    
                                    <th align="left" class="Body_TableHeader">                                        
                                        <asp:Label ID="lb_STOP_YY" runat="server" Text="(留停)年"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_STOP_YY" runat="server" Width="50px" MaxLength="2" Style="text-align: right;ime-mode:disabled" ClientIDMode="Static" onkeypress="return IsIntText();" onpaste="return false"> </asp:TextBox>                                        
                                    </td>
                                </tr>
                                <%--(留停)月--%>
                                <tr>                                    
                                    <th align="left" class="Body_TableHeader">                                        
                                        <asp:Label ID="lb_STOP_MM" runat="server" Text="(留停)月"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_STOP_MM" runat="server" Width="50px" MaxLength="2" Style="text-align: right;ime-mode:disabled" ClientIDMode="Static" onkeypress="return IsIntText();" onpaste="return false"> </asp:TextBox>                                        
                                    </td>
                                </tr>
                                <%--(留停)日--%>
                                <tr>                                    
                                    <th align="left" class="Body_TableHeader">                                        
                                        <asp:Label ID="lb_STOP_DD" runat="server" Text="(留停)日"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_STOP_DD" runat="server" Width="50px" MaxLength="2" Style="text-align: right;ime-mode:disabled" ClientIDMode="Static" onkeypress="return IsIntText();" onpaste="return false"> </asp:TextBox>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn  ID="WFB2SP0400Execute" runat="server" Text="計算" OnClick="WFB2SP0400Execute_Click" OnClientClick="return executeCheck();"  />
                                            <%-- 
                                            <asp:Button ID="WFB2SP0400Execute" runat="server" Text="計算" OnClick="WFB2SP0400Execute_Click" OnClientClick="return executeCheck();"  />
                                            --%>
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


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
