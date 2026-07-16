<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2dl/WFB2DL0510_Dtl.aspx.cs" Inherits="WebContent_fb2dl_WFB2DL0510_Dtl" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
     <style type="text/css">
        #txt_NEW_PJOB_CD {
            text-transform: uppercase;
        }
    </style>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            gridviewScroll();
            $.unblockUI();

            //職務代號取得職務名稱的ajax
            var currentDate = new Date();
            $("#txt_NEW_PJOB_CD").change(function () {
                if ($("#txt_NEW_PJOB_CD").val().length == 4) {
                    $.ajax({
                        url: "../commgeo/WFB2GetPjobData.ashx",
                        data: {
                            PJOB_CD: $('#txt_NEW_PJOB_CD').val(),
                            START_DT: currentDate.format("yyyy/MM/dd")
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_NEW_PJOB_DESC').val("");
                            }
                            else {
                                $('#txt_NEW_PJOB_DESC').val(JData.PJOB_DESC);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_NEW_PJOB_DESC').val("");
                }
            });
        }
         
        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        
        
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb299_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb299_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb299_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
                                            <asp:TextBox ID="txt_HR_CHG_DESC" runat="server"  CssClass="txtDisabled"  Width="300px"  Enabled="false" ></asp:TextBox>
                                     </td>

                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label3" runat="server" Text="指定職務"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_IS_BIND_PJOB" runat="server"  CssClass="txtDisabled"  Width="300px" Enabled="false" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="特休代碼"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DL_GEN_CD" runat="server"  CssClass="txtDisabled"  Width="300px" Enabled="false" ></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label4" runat="server" Text="結算方式"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_SETTLE_CD_DESC" CssClass="txtDisabled" runat="server" Enabled="false" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PROC_CD" runat="server" Text="作業碼"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PROC_DESC" runat="server"  Width="300px" CssClass="txtDisabled"   Enabled="false"  ></asp:TextBox>
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
                                        <asp:TextBox ID="txt_IS_D01_SAME_DESC" runat="server" Width="300px" CssClass="txtDisabled"   Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>          
                                <tr>
                                    <!--備註說明-->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="備註"></asp:Label>:
                                    </th>
                                    <td colspan="5">
                                        <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_REMARK" runat="server" Width="300px" Enabled="false"  CssClass="txtDisabled" Style="overflow: auto"></asp:TextBox>
                                    </td>
                                </tr>                    
                                <tr>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="5">
                                          <asp:Button ID="btn_back" runat="server" Text="返回" OnClick="btn_back_Click" />
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

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFB2DL0511Add" runat="server" Text="新增" OnClick="WFB2DL0511Add_Click" />
                            <aces:Btn ID="WFB2DL0511Del" runat="server" Text="刪除" OnClientClick="return CheckDelAction();" OnClick="WFB2DL0511Del_Click" Visible="False" />
                            <aces:Btn ID="WFB2DL0511Save" runat="server" Text="儲存" Visible="false" OnClick="WFB2DL0511Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupB" />
                            <asp:Button ID="btn_cancel" runat="server" Text="取消" Visible="false" OnClick="btn_cancel_Click" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val());" />
                         
                        </div>

                    </td>
                </tr>
                
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDtlData"
                SelectCountMethod="getDtlCount" TypeName="CFB2DL0510DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="ods1_Selecting"
                 StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_hr_chg_cd"
                        Name="hr_chg_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_dl_gen_Cd"
                        Name="dl_gen_Cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="40px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" Width="20px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb299_RowNumber%>" HeaderStyle-Width="80px"  >
                        <ItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'  Width="40px"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_PJOB_CD%>" SortExpression="PJOB_CD"    HeaderStyle-Width="200px"   ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_CD" runat="server" Text='<%#Bind("PJOB_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>                           
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_PJOB_CD" runat="server" Width="80px" CssClass="MandatoryField EnNum4" maxlength="4" ClientIDMode="Static" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_PJOB_CD%>"
                                ControlToValidate="txt_NEW_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="Regular_NEW_PJOB_CD" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_error_pjob_cd%>" ControlToValidate="txt_NEW_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_format_pjob_cd%>" ControlToValidate="txt_NEW_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_PJOB_DESC%>" SortExpression="PJOB_DESC" HeaderStyle-Width="200px"    ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_DESC" runat="server" Text='<%#Bind("PJOB_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>                           
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_PJOB_DESC" runat="server" MaxLength="30" Width="100px" ClientIDMode="Static"  Enabled="false"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
                  <%--當DB無資料時，就會使用此table --%>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td width="20px"></td>
                            <td width="80px">
                                <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb299_lbl_ORDER_SEQ%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="lblHeaderPJOB_CD" runat="server" Text="職務代碼"></asp:Label>
                            </td>
                            <td width="200px">
                                <asp:Label ID="lblHeaderPJOB_DESC" runat="server" Text="職務名稱"></asp:Label>
                            </td>                            
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <div style="text-align: center;">
                                 <asp:TextBox ID="txt_NEW_PJOB_CD" runat="server" Width="80px" CssClass="MandatoryField EnNum4" maxlength="4" ClientIDMode="Static" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_PJOB_CD%>"
                                    ControlToValidate="txt_NEW_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="Regular_NEW_PJOB_CD" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ha_error_pjob_cd%>" ControlToValidate="txt_NEW_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ha_format_pjob_cd%>" ControlToValidate="txt_NEW_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                   <asp:TextBox ID="txt_NEW_PJOB_DESC" runat="server" MaxLength="30" Width="100px" ClientIDMode="Static"  Enabled="false"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                            <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                            <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hid_hr_chg_cd" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_USER_UPD" runat="server" ClientIDMode="static" />
            <asp:HiddenField ID="hid_dl_gen_Cd" runat="server" ClientIDMode="Static" />

           <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


