<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dl/WFB2DL0510_Add.aspx.cs" Inherits="WebContent_wfb2hd_WFB2DL0510_Add" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
         #txt_HR_CHG_CD {text-transform : uppercase; }
         #txt_HR_CHG_CD_Add {text-transform : uppercase;}
         #txt_HR_CHG_DESC {
                color:black;
                background-color:white;
          }
    </style>

    <script type="text/javascript">
        jQuery(document).ready(function () {
            initForm();
        }); 
        function initForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');

            $.unblockUI();
            $('#txt_HR_CHG_CD').change(function () {
                var regex = new RegExp("^[a-zA-Z0-9]+$");
                var str = $('#txt_HR_CHG_CD').val();
                if (regex.test(str)) {
                    if ($('#txt_HR_CHG_CD').val().length == 3) {
                        //ajax 
                        $.ajax({
                            url: "../commgeo/WFB2ChangeCode_Search.ashx",
                            data: {
                                HR_CHG_CD: $('#txt_HR_CHG_CD').val()
                            },
                            type: "GET",
                            cache: false,
                            dataType: 'json',
                            success: function (JData) {
                                if (JData.errMsg != "") {
                                    $('#txt_HR_CHG_DESC').val("");
                                }
                                else {
                                    $('#txt_HR_CHG_CD').val(JData.HR_CHG_CD_A);
                                    $('#txt_HR_CHG_DESC').val(JData.HR_CHG_DESC);
                                }
                            },

                            error: function (xhr, ajaxOptions, thrownError) {
                                alert(xhr.status);
                                alert(thrownError);
                            }
                        });
                    } else {
                        $('#txt_HR_CHG_DESC').val("");
                    }
                } else if ($.trim(str) != "") {
                    alert($("#hidwfb2_alphanumeric_onlyMessage").val());
                    $('#txt_HR_CHG_CD').val("");
                    return false;
                }

            });

        }
        //儲存前檢查
        function saveCheck() {           
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }

        function checkConfirm() {
            return confirm($('#hidwfb299_Cancel_ConfirmMessage').val());
        }
         
        

    </script>
    <style type="text/css">
        .auto-style1 {
            height: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
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
                                        <asp:Label ID="Label2" runat="server" Text="人事異動代碼"></asp:Label>:</th>
                                     <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_HR_CHG_CD" runat="server" ClientIDMode="Static" MaxLength="3" Width="64px" CssClass="MandatoryField"></asp:TextBox>
                                            <input id="btn_HR_CHG_CD" type="button" value="..." onclick="OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_DESC', '');" />
                                            <asp:TextBox ID="txt_HR_CHG_DESC" runat="server" BorderWidth="0" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="請輸入人事異動代碼"
                                            ControlToValidate="txt_HR_CHG_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txt_HR_CHG_CD" ErrorMessage="僅可輸入英數字3碼" ValidationExpression="^[a-zA-Z0-9]+$" ValidationGroup="GroupA" Display="None"></asp:RegularExpressionValidator>
                                     </td>

                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label3" runat="server" Text="指定職務"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_BIND_PJOB" CssClass="MandatoryField" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="請選擇指定職務"
                                            ControlToValidate="ddl_IS_BIND_PJOB" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="特休代碼"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_DL_GEN_CD" CssClass="MandatoryField" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddl_DL_GEN_CD_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="請選擇特休代碼"
                                            ControlToValidate="ddl_DL_GEN_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label4" runat="server" Text="結算方式"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_SETTLE_CD" CssClass="MandatoryField" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="請選擇結算方式"
                                            ControlToValidate="ddl_SALARY_SETTLE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PROC_CD" runat="server" Text="作業碼"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PROC_DESC" runat="server"  CssClass="txtDisabled"   Enabled="false"  ></asp:TextBox>
                                    </td>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label5" runat="server" Text="邏輯碼"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_LOGI_DESC" runat="server" Width="300px" CssClass="txtDisabled"   Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SDT_CD" runat="server" Text="起始日"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_SDT_DESC" runat="server" Width="300px" CssClass="txtDisabled"   Enabled="false"></asp:TextBox>
                                    </td>
                                      <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EDT_CD" runat="server" Text="結束日"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EDT_DESC" runat="server" Width="300px" CssClass="txtDisabled"   Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>                                                              
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label6" runat="server" Text="特休生成日"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_DL_GENDT_DESC" runat="server" Width="300px" CssClass="txtDisabled"   Enabled="false"></asp:TextBox>
                                    </td>
                                      <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label7" runat="server" Text="當年度復職"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_IS_D01_SAME" runat="server" Width="300px" CssClass="txtDisabled"   Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>          
                                <tr>
                                    <!--備註說明-->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="備註"></asp:Label>:
                                    </th>
                                    <td colspan="5">
                                        <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_REMARK" runat="server" Width="100%" Enabled="true" BorderWidth="1" Style="overflow: auto"></asp:TextBox>
                                    </td>
                                </tr>                    
                                <tr>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="5">
                                        <aces:Btn ID="WFB2DL0510Save" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2DL0510Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />
                                        <asp:Button ID="btn_cancel" runat="server" OnClick="WFB2DL0510Cancel_Click" OnClientClick="checkConfirm();"  Text="<%$Resources:Resource,btn_Cancel%>"  ClientIDMode="Static"/>
                                    </td>
                                </tr>
                                <!-- end: Create MODULE ID -->
                                <!-- START: Create a line to separate Search field with body field -->
                                <tr>
                                    <td align="center" height="1" colspan="6">
                                        <hr>
                                    </td>
                                </tr>
                                <!-- END: Create a line -->
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Body_label"></td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="WFB2DL0510Save" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ddl_DL_GEN_CD" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
