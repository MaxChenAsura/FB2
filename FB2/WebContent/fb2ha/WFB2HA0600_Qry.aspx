<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0600_Qry.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0600_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
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
            
            iniForm();
        });
       
        
        function iniForm() {
            $("#txt_YEAR_MONTH_Add").datepicker({ dateFormat: 'yymm' });
            $(".number").mask('9999/99');
            $(".empid").mask('99999');
            gridviewScroll();
            $.unblockUI();
            $('#txt_HR_CHG_CD').change(function () {
                var regex = new RegExp("^[a-zA-Z0-9]+$");
                var str = $('#txt_HR_CHG_CD').val();
                if (regex.test(str)) {
                    if ($('#txt_HR_CHG_CD').val().length == 3) {
                        //ajax 取得薪資
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
                } else if($.trim(str)!=""){
                    alert($("#hidwfb2_alphanumeric_onlyMessage").val());
                    $('#txt_HR_CHG_CD_Add').val("");
                    return false;
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
                barcolor: "#7F7F7F",
                freezesize: 4

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }
        function ClearAll() {
            
            $('#txt_HR_CHG_DESC').val("");
            $('#txt_HR_CHG_CD').val("");
            $('#ddl_IS_VALID').val(-1);

        }

        function CheckMODE_ID(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if (!re.test($("#txt_MODE_ID_Add").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidWfb2ha_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            var processed = true;
            if (LookUpCheckboxs() >= 1) {
                processed = true;
            } else {
                alert($('#hidwfb2_Dtl_NotChoiceMessage').val());
                processed = false;
                return processed;
            }
            if (confirm($('#hidwfb2_Del_ConfirmMessage').val())) {
                processed = true;
                $.unblockUI();
            } else {
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
            }
            return processed;
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

        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hidWfb2ha_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hidWfb2ha_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hidWfb2ha_Save_ConfirmMessage').val());
        }


        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            HaveCheck = HaveCheck - 1;
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
                                <col width="20%" />
                                 <col width="35%" />


                            </colgroup>
                            <tbody>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lbl_HR_CHG_CD" runat="server" Text="">人事異動代碼</asp:Label>: 

                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_HR_CHG_CD" runat="server" ClientIDMode="Static" MaxLength="3" Width="64px"></asp:TextBox>
                                            <input id="btn_HR_CHG_CD" type="button" value="..." onclick="OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_DESC', '');" />
                                            <asp:TextBox ID="txt_HR_CHG_DESC" runat="server" BorderWidth="0" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="IS_VALID" runat="server" Text="">使用中</asp:Label>
                                            : </th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_IS_VALID" runat="server" ClientIDMode="Static">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>Y</asp:ListItem>
                                                <asp:ListItem>N</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <th></th>
                                        <th class="auto-style1">
                                        <td align="right">
                                            <div id="init">
                                                <aces:Btn ID="WFB2HA0600Search" runat="server" OnClick="WFB2HA0600Search_Click" OnClientClick="BlockUI();" Text="<%$Resources:Resource,btn_Search%>" />
                                                
                                                <%--<asp:Button ID="WFB2HA0600Search" runat="server" OnClick="WFB2HA0600Search_Click" OnClientClick="BlockUI();" Text="<%$Resources:Resource,btn_Search%>" />--%>
                                            
                                                <input id="WFB2HA0600Clear" type="button" value="<%$Resources:Resource,btn_clear%>" runat="server" onclick="ClearAll();"/>
                                                
                                            <aces:Btn ID="WFB2HA0600_PRIV3" runat="server" OnClick="WFB2HA0600_PRIV3_Click" Text="<%$Resources:Resource,btn_PRIV3%>" />
                                                <aces:Btn ID="WFB2HA0600_PRIV2" runat="server" OnClick="WFB2HA0600_PRIV2_Click" Text="<%$Resources:Resource,btn_PRIV2%>" />
                                                
<%--                                                <asp:Button ID="WFB2HA0600_PRIV3" runat="server" OnClick="WFB2HA0600_PRIV3_Click" Text="<%$Resources:Resource,btn_PRIV3%>" />
                                                <asp:Button ID="WFB2HA0600_PRIV2" runat="server" OnClick="WFB2HA0600_PRIV2_Click" Text="<%$Resources:Resource,btn_PRIV2%>" />--%>
                                            </div>
                                        </td>
                                            </th>
                                    </tr>
                           </tbody>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                        <aces:Btn ID="WFB2HA0600Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2HA0600Add_Click" />
                            <aces:Btn ID="WFB2HA0600Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2HA0600Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2HA0600Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClick="WFB2HA0600Edit_Click" Visible="false" />

<%--                            <asp:Button ID="WFB2HA0600Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2HA0600Add_Click" />
                            <asp:Button ID="WFB2HA0600Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2HA0600Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2HA0600Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClick="WFB2HA0600Edit_Click" Visible="false" />--%>
                            
                            <asp:Button ID="WFB2HA0600Cancel" runat="server" Text="<%$Resources:Resource,btn_Cancel%>" Visible="false" OnClick="WFB2HA0600Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                            
                            <aces:Btn ID="WFB2HA0600Detail" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Detail%>"  Visible="false" OnClick="WFB2HA0600Detail_Click"/>
                            
                            <%--<asp:Button ID="WFB2HA0600Detail" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Detail%>"  Visible="false" OnClick="WFB2HA0600Detail_Click"/>--%>
                        </div>

                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="Cfb2ha0600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_HR_CHG_CD"
                        Name="txt_HR_CHG_CD" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_IS_VALID"
                        Name="ddl_IS_VALID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                   
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1800px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div>
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2ib_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <div>
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                      
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_HR_CHG_CD%>" SortExpression="HR_CHG_CD">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_HR_CHG_CD" runat="server" Text='<%#Bind("HR_CHG_CD")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                       
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_HR_CHG_DESC%>" SortExpression="HR_CHG_DESC">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lbl_HR_CHG_DESC" runat="server" Text='<%#Bind("HR_CHG_DESC")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                            
                    </asp:TemplateField>


                    
                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_IS_INS_EARLIER%>" SortExpression="IS_INS_EARLIER">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_IS_INS_EARLIER" runat="server" Text='<%#Bind("IS_INS_EARLIER")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                       
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_UPD_RIGHT_CD%>" SortExpression="UPD_RIGHT_CD">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_UPD_RIGHT_CD" runat="server" Text='<%#Bind("UPD_RIGHT_CD")%>'></asp:Label>
                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_IS_FOR_TRANSFER_IN%>" SortExpression="IS_FOR_TRANSFER_IN">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_IS_FOR_TRANSFER_IN" runat="server" Text='<%#Bind("IS_FOR_TRANSFER_IN")%>'></asp:Label>
                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_IS_LEAVE%>" SortExpression="IS_LEAVE">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_IS_LEAVE" runat="server" Text='<%#Bind("IS_LEAVE")%>'></asp:Label>
                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_IS_TEMP%>" SortExpression="IS_TEMP">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_IS_TEMP" runat="server" Text='<%#Bind("IS_TEMP")%>'></asp:Label>
                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_IS_UPD_HR%>" SortExpression="IS_UPD_HR">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_IS_UPD_HR" runat="server" Text='<%#Bind("IS_UPD_HR")%>'></asp:Label>
                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_SALARY_PROC_CD%>" SortExpression="SALARY_PROC_CD">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_SALARY_PROC_CD" runat="server" Text='<%#Bind("SALARY_PROC_CD")%>'></asp:Label>
                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_INSURANCE_PROC_CD%>" SortExpression="INSURANCE_PROC_CD">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_INSURANCE_PROC_CD" runat="server" Text='<%#Bind("INSURANCE_PROC_CD")%>'></asp:Label>
                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_DUTY_PROC_CD%>" SortExpression="DUTY_PROC_CD">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_DUTY_PROC_CD" runat="server" Text='<%#Bind("DUTY_PROC_CD")%>'></asp:Label>
                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_CONTRACT_PROC_CD%>" SortExpression="CONTRACT_PROC_CD">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_CONTRACT_PROC_CD" runat="server" Text='<%#Bind("CONTRACT_PROC_CD")%>'></asp:Label>
                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_EMP_CHG_STATUS%>" SortExpression="EMP_CHG_STATUS">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_EMP_CHG_STATUS" runat="server" Text='<%#Bind("EMP_CHG_STATUS")%>'></asp:Label>
                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,Wfb2ha_IS_SHOW%>" SortExpression="IS_SHOW">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lbl_IS_SHOW" runat="server" Text='<%#Bind("IS_SHOW")%>'></asp:Label>
                            </div>
                        </ItemTemplate>                        
                    </asp:TemplateField>


                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" >
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_alphanumeric_onlyMessage" Value="<%$Resources:Resource,wfb2_alphanumeric_onlyMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2_Dtl_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Dtl_NotChoiceMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


