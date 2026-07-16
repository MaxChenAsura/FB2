<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb299/WFB2990400_Qry.aspx.cs" Inherits="WebContent_fb299_WFB2990400_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #txt_MODE_ID_Add {
            text-transform: uppercase;
        }
    </style>
    <title></title>



    <script type="text/javascript">



        jQuery(document).ready(function () {

            iniForm();
        });


        function iniForm() {
            $("#txt_START_YM_Add").datepicker({ dateFormat: 'yymm' });
            $("#txt_END_YM_Add").datepicker({ dateFormat: 'yymm' });


            $(".number").mask('9999/99');
            gridviewScroll();
            $.unblockUI();


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
        function ClearAll() {

            $('#ddl_SYS_ID').val(-1);

        }

        function CheckMODE_ID(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if (!re.test($("#txt_MODE_ID_Add").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }


        function CheckAddAction() {
            $("#hidwfb200_mode").val("add");
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

        //儲存前檢查
        function saveCheck() {
            var processed = true;
            if ($("#hidwfb200_mode").val() == "add") {
                if (Page_ClientValidate("GroupA")) {
                    BlockUI();

                }
                else
                    processed = false;
                if (!processed) {
                    $.unblockUI();
                    return;
                }
                return processed;
            }
        }

        //檢核刪除
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb299_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
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
        function CancelConfirm() {
            return confirm($("#hidwfb299_Cancel_ConfirmMessage").val());
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
                                <col width="11%" />
                                <col width="29%" />
                                <col width="10%" />
                                <col width="50%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SYS_ID" runat="server" Text="<%$Resources:Resource,wfb299_lb_SYS_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:DropDownList ID="ddl_SYS_ID" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                             <aces:Btn ID="WFB2990400Search" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Search%>" OnClick="WFB2990400Search_Click" OnClientClick="BlockUI();" />
                                            <%--<asp:Button ID="WFB2990400Search" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Search%>" OnClick="WFB2990400Search_Click" OnClientClick="BlockUI();" />--%>
                                            <input id="WFB2990400Clear" type="button" value="<%$Resources:Resource,btn_clear%>" runat="server" onclick="ClearAll();"/>
                                        </div>
                                    </td>
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
                                                    
                             <aces:Btn ID="WFB2990400Add" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Add%>" OnClick="WFB2990400Add_Click" OnClientClick="CheckAddAction();" />
                            <aces:Btn ID="WFB2990400Delete" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2990400Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2990400Edit" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2990400Edit_Click" Visible="False" />
                            <aces:Btn ID="WFB2990400Save" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Save%>" Visible="false" OnClick="WFB2990400Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />                            
                            
                           <%-- <asp:Button ID="WFB2990400Add" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Add%>" OnClick="WFB2990400Add_Click" OnClientClick="CheckAddAction();" />
                            <asp:Button ID="WFB2990400Delete" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2990400Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2990400Edit" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2990400Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2990400Save" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Save%>" Visible="false" OnClick="WFB2990400Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />--%>
                            <asp:Button ID="WFB2990400Cancel" runat="server" Text="<%$Resources:Resource,btn_Cancel%>" Visible="false" OnClick="WFB2990400Cancel_Click" OnClientClick="return CancelConfirm();" />
                            <%--<asp:Button ID="WFB2990400Detail" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Detail%>" Visible="false" OnClientClick="return CheckDtlAction();" OnClick="WFB2990400Detail_Click" />--%>
                            <aces:Btn ID="WFB2990400Detail" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Detail%>"  Visible="false" OnClientClick="return CheckDtlAction();" OnClick="WFB2990400Detail_Click"/>
                        </div>

                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2990400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_SYS_ID"
                        Name="sys_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
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
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb299_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_SYS_ID%>" SortExpression="SYS_ID" HeaderStyle-Width="250px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lbl_SYS_ID" runat="server" Text='<%#Bind("SYS_ID")%>' MaxLength="1"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_SYS_ID_Add" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddl_SYS_ID_Add_SelectedIndexChanged" CssClass="MandatoryField" AutoPostBack="true"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_require_ddl_SYS_ID_Add%>"
                                    ControlToValidate="ddl_SYS_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_SYS_NAME%>" SortExpression="SYS_NAME" HeaderStyle-Width="250px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lbl_SYS_NAME" runat="server" Text='<%#Bind("SYS_NAME")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_SYS_NAME" runat="server"></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_0400_lbl_MODE_ID%>" SortExpression="MODE_ID" HeaderStyle-Width="250px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lbl_MODE_ID" runat="server" Text='<%#Bind("MODE_ID")%>' Width="250px"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <%--作業別代碼 --%>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_MODE_ID_Add" runat="server" MaxLength="6" ClientIDMode="Static" CssClass="MandatoryField" Width="250px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_require_txt_MODE_ID_Add%>"
                                    ControlToValidate="txt_MODE_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                                <!-- 只能輸入英數字 -->
                                <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb299_SUB_CD_isError%>" ControlToValidate="txt_MODE_ID_Add" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_0400_lbl_MODE_NAME%>" SortExpression="MODE_NAME" HeaderStyle-Width="250px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_MODE_NAME" runat="server" Text='<%#Bind("MODE_NAME")%>' Width="250px"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:TextBox ID="txt_MODE_NAME_Add" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("MODE_NAME")%>' Width="250px"></asp:TextBox>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_MODE_NAME_Add" runat="server" MaxLength="50" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>


                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td width="20px"></td>
                            <td width="40px">
                                <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb299_RowNumber%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderlbl_SYS_ID" runat="server" Text="<%$Resources:Resource,wfb299_lb_SYS_ID%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderlbl_SYS_NAME" runat="server" Text="<%$Resources:Resource,wfb299_lb_SYS_NAME%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderlbl_MODE_ID" runat="server" Text="<%$Resources:Resource,wfb299_0400_lbl_MODE_ID%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderlbl_MODE_NAME" runat="server" Text="<%$Resources:Resource,wfb299_0400_lbl_MODE_NAME%>"></asp:Label>
                            </td>

                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:DropDownList ID="ddl_SYS_ID_Add" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddl_SYS_ID_Add_SelectedIndexChanged" CssClass="MandatoryField" AutoPostBack="true"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_require_ddl_SYS_ID_Add%>"
                                        ControlToValidate="ddl_SYS_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:Label ID="lbl_SYS_NAME" runat="server"></asp:Label>
                                </div>
                            </td>
                            <td>
                                <%--作業別代碼 --%>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_MODE_ID_Add" runat="server" MaxLength="6" ClientIDMode="Static" CssClass="MandatoryField" Width="250px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_require_txt_MODE_ID_Add%>"
                                        ControlToValidate="txt_MODE_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <!-- 只能輸入英數字 -->
                                    <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                        ErrorMessage="<%$Resources:Resource,wfb299_SUB_CD_isError%>" ControlToValidate="txt_MODE_ID_Add" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_MODE_NAME_Add" runat="server" MaxLength="50" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </td>

                            </div>
                            </td>
                        </tr>
                    </table>



                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
            </asp:GridView>
            <td style="font-size: 14px;">
                <asp:Label ID="lb_TotalCount2" runat="server" Text="" Visible="false" Font-Size="10"></asp:Label>
            </td>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
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

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />


            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


