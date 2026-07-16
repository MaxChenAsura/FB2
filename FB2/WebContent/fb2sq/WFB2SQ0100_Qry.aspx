<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sq/WFB2SQ0100_Qry.aspx.cs" Inherits="WebContent_WFB2SQ0100_Qry" Culture="auto" UICulture="auto" %>

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
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.date').mask('9999/99');
            $("#txt_YM").mask("9999/99");
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
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));                             
                                
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
            $("#txt_YM").val("");
            $("#txt_EMP_ID").val();
            $("#txt_EMP_NAME").val();            
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
                                        <asp:Label ID="lb_YM" runat="server" Text="<%$Resources:Resource,WFb2sq_YM%>"></asp:Label></th>
                                    <td align="left" class="Body_label"  >
                                        <asp:TextBox ID="txt_YM" runat="server" MaxLength="7" Width="80px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>                                          
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,WFb2sq_YM_isNull%>"
                                            ControlToValidate="txt_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                        ErrorMessage ="<%$Resources:Resource,wfb2sq_error_txt_YM%>" ControlToValidate="txt_YM" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>                          
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--工號--%>
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="工號"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5"> </asp:TextBox>
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>                                        
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
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn  ID="WFB2SQ0100Execute" runat="server" Text="計算" OnClick="WFB2SQ0100Execute_Click" OnClientClick="return executeCheck();"  />
                                            <%-- 
                                            <asp:Button ID="WFB2SQ0100Execute" runat="server" Text="計算" OnClick="WFB2SQ0100Execute_Click" OnClientClick="return executeCheck();"  />
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
