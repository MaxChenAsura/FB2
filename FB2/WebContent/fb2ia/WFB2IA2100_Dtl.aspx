<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA2100_Dtl.aspx.cs" Inherits="WebContent_fb2ia_WFB2IA2100_Dtl" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ym').mask('9999/99/99');
            $('.number').mask('9999');
            //$(".number").formatNumber({ format: "#,###", locale: "tw" });
            gridviewScroll();
            $.unblockUI();

        }

        function CheckEdinum(source, arguments) {
            if ($("#txt_NEW_INS_COND_AMT").val() < 0)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }

        function CheckINS_QUIT_DT(source, arguments) {
            if ($("#txt_EDIT_INS_QUIT_DT").val().replace("/", "") < $("#lb_INS_ENTRY_DT").text().replace("/", ""))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }
        function CheckINS_QUIT_DT_Add(source, arguments) {
            if ($("#txt_NEW_INS_QUIT_DT").val().replace("/", "") < $("#txt_NEW_INS_ENTRY_DT").val().replace("/", ""))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }
        function addcheck() {
            $("#HID_IDENTITY_KIND").val($("#ddl_IDENTITY_KIND").val());
            $("#HID_NEW_LICENSE_ID").val($("#txt_NEW_LICENSE_ID").val());
            $("#HID_GINS_KIND").val($("#ddl_GINS_KIND").val());
            $("#HID_NEW_INS_COND_AMT").val($("#txt_NEW_INS_COND_AMT").val());
            $("#HID_NEW_INS_ENTRY_DT").val($("#txt_NEW_INS_ENTRY_DT").val());
            $("#HID_NEW_INS_QUIT_DT").val($("#txt_NEW_INS_QUIT_DT").val());
            $("#HID_NEW_TARGET_TYPE").val($("#txt_NEW_TARGET_TYPE").val());
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
                return confirm($('#hid_wfb2ia_Del_ConfirmMessage').val());
            else {
                alert($('#hid_wfb2ia_Del_NotChoiceMessage').val());
                return false;
            }
        }
        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hid_wfb2ia_Mod_NotChoiceMessage').val());
                return false;
            }
        }
        //檢查勾了幾個checkbox
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        //function IDENTITY_KIND_Choose() {
        //    $("#HID_IDENTITY_KIND").val($("#ddl_IDENTITY_KIND").val());
        //}
        //function GINS_KIND_Choose() {
        //    $("#HID_GINS_KIND").val($("#ddl_GINS_KIND").val());
        //}
        function ddlCheck(source, arguments) {
            if ($("#ddl_IDENTITY_KIND").val() == "-1")
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }
        function ddlCheck2(source, arguments) {
            if ($("#ddl_GINS_KIND").val() == "-1")
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }
        function idCheck(value) {
            if (value != "")
                __doPostBack('question', 'true');
            else {
                $('#txt_NEW_EMP_NAME').val("");
                $('#txt_NEW_BIRTH_DT').val("");
                $('#txt_NEW_TARGET_TYPE').val("");
            }
        }
        //ID開窗
        function OpenIDSearch(obj_id, obj_name) {
            if ($('#ddl_IDENTITY_KIND').val() == "-1")
                alert("請先選擇身分別");
            else {
                OpenSearch('LICENSE_ID_Search.aspx', obj_id, obj_name, "IDENTITY_KIND=" + $("#ddl_IDENTITY_KIND").val() + "&EMP_ID=" + $("#txt_EMP_ID").val() + "&LICENSE_ID=" + $("#lb_LICENSE_ID").text(),'Y');
                //if (json != undefined) {
                //    $("#" + obj_id).val(json.CD);
                //    $("#" + obj_name).val(json.DESC);
                //    $("#txt_NEW_BIRTH_DT").val(json.Val3);
                //    $("#txt_NEW_TARGET_TYPE").val(json.Val5);
                //}
            }

        }
        function LICENSE_ID_Search(id, name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#" + id).val(obj.CD);
                $("#" + name).val(obj.DESC);
                $("#txt_NEW_BIRTH_DT").val(obj.Val3);
                $("#txt_NEW_TARGET_TYPE").val(obj.Val5);
                return obj;
            }
        }
        //檢查數字且不為負數
        function CheckAddnum(source, arguments) {
            var re = /^(0|[1-9][0-9]*)$/;
            if ($("#txt_NEW_INS_COND_AMT").val() != "") {
                if (re.test($("#txt_NEW_INS_COND_AMT").val())) {
                    arguments.IsValid = true;
                }
                else {
                    arguments.IsValid = false;
                }
            }
            else
                arguments.IsValid = true;
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
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2ia_EMP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SUB_DESC" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_SUB_DESC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SUB_DESC" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_SNAME" runat="server" Text="<%$Resources:Resource,wfb2ia_COMPANY_SNAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_COMPANY_SNAME" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DIV_DEPT_FULL_NAME" runat="server" Text="<%$Resources:Resource,wfb2ia_DIV_DEPT_FULL_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_DIV_DEPT_FULL_NAME" Width="900px" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <asp:Button ID="back_page" runat="server" Text="<%$Resources:Resource,wfb2ia_back_page%>" OnClick="back_page_Click" OnClientClick="BlockUI();" />
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
                <tr>

                    <td align="right" class="Body_label">

                        <div id="init_grid">
                            <aces:Btn ID="WFB2IA2100Add" runat="server" Text="<%$Resources:Resource,wfb2ia_add%>" OnClick="WFB2IA2100Add_Click" Visible="true" OnClientClick="BlockUI();" />
                            <aces:Btn ID="WFB2IA2100Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IA2100Delete_Click" Visible="true" />
                            <aces:Btn ID="WFB2IA2100Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_edit%>" OnClick="WFB2IA2100Edit_Click" Visible="true" OnClientClick="return CheckModeifyAction(); BlockUI();" />
                            <aces:Btn ID="WFB2IA2100OK" runat="server" Text="<%$Resources:Resource,wfb2ia_ok%>" Visible="false" OnClick="WFB2IA2100OK_Click" OnClientClick="addcheck(); CheckValid();" ValidationGroup="GroupB" />
                            
                            <%--<asp:Button ID="WFB2IA2100Add" runat="server" Text="<%$Resources:Resource,wfb2ia_add%>" OnClick="WFB2IA2100Add_Click" Visible="true" OnClientClick="BlockUI();" />
                            <asp:Button ID="WFB2IA2100Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IA2100Delete_Click" Visible="true" />
                            <asp:Button ID="WFB2IA2100Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_edit%>" OnClick="WFB2IA2100Edit_Click" Visible="true" OnClientClick="return CheckModeifyAction(); BlockUI();" />
                            <asp:Button ID="WFB2IA2100OK" runat="server" Text="<%$Resources:Resource,wfb2ia_ok%>" Visible="false" OnClick="WFB2IA2100OK_Click" OnClientClick="addcheck(); CheckValid();" ValidationGroup="GroupB" />--%>
                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2ia_cancel%>" Visible="false" OnClick="btn_cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                        </div>

                    </td>
                </tr>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetDtlData"
                SelectCountMethod="GetDtlCount" TypeName="CFB2IA2100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <%--<asp:ControlParameter ControlID="txt_EMP_NAME" DefaultValue=""
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_SUB_DESC" DefaultValue=""
                        Name="sub_desc" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_COMPANY_SNAME" DefaultValue=""
                        Name="company_sname" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_DIV_DEPT_FULL_NAME" DefaultValue=""
                        Name="div_dept_full_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />--%>
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sk_RowNum%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </FooterTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_IDENTITY_KIND%>" SortExpression="IDENTITY_KIND" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lb_IDENTITY_KIND" ClientIDMode="Static" runat="server" Text='<%#Bind("IDENTITY_KIND_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_IDENTITY_KIND" runat="server" Text='<%#Bind("IDENTITY_KIND_DESC")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_IDENTITY_KIND" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_required_IDENTITY_KIND%>" ClientValidationFunction="ddlCheck" ForeColor="Red"
                                ControlToValidate="ddl_IDENTITY_KIND" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" ClientIDMode="Static" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" ClientIDMode="Static" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_EMP_NAME" Width="100%" runat="server" ClientIDMode="Static" BorderWidth="0" Enabled="false"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_BIRTH_DT%>" SortExpression="BIRTH_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lb_BIRTH_DT" runat="server" ClientIDMode="Static" Text='<%#Bind("BIRTH_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_BIRTH_DT" runat="server" ClientIDMode="Static" Text='<%#Bind("BIRTH_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_BIRTH_DT" Width="100%" runat="server" ClientIDMode="Static" BorderWidth="0" Enabled="false"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>" SortExpression="LICENSE_ID" HeaderStyle-Width="130px" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_LICENSE_ID" runat="server" ClientIDMode="Static" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_LICENSE_ID" runat="server" MaxLength="20" ClientIDMode="Static" CssClass="MandatoryField" onblur="idCheck(this.value,'id');"></asp:TextBox>
                            <input id="bt_NEW_LICENSE_ID" type="button" value="..." onclick="OpenIDSearch('txt_NEW_LICENSE_ID', 'txt_NEW_EMP_NAME');" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_LICENSE_ID%>"
                                ControlToValidate="txt_NEW_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2dj_format_allowance_type%>" ControlToValidate="txt_NEW_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_TARGET_TYPE%>" SortExpression="TARGET_TYPE" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_TYPE" ClientIDMode="Static" runat="server" Text='<%#Bind("TARGET_TYPE_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TARGET_TYPE" runat="server" ClientIDMode="Static" Text='<%#Bind("TARGET_TYPE_DESC")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_TARGET_TYPE" Width="100%" runat="server" ClientIDMode="Static" BorderWidth="0" Enabled="false"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_GINS_KIND%>" SortExpression="GINS_KIND" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lb_GINS_KIND" ClientIDMode="Static" runat="server" Text='<%#Bind("GINS_KIND")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_GINS_KIND" runat="server" Text='<%#Bind("GINS_KIND")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_GINS_KIND" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_required_GINS_KIND%>" ClientValidationFunction="ddlCheck2" ForeColor="Red"
                                ControlToValidate="ddl_GINS_KIND" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_COND_AMT%>" SortExpression="INS_COND_AMT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_INS_COND_AMT" runat="server" Text='<%#Bind("INS_COND_AMT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Style="text-align: right" Width="100%" ID="txt_EDIT_INS_COND_AMT" runat="server" MaxLength="4" ClientIDMode="Static" CssClass="number" Text='<%#Bind("INS_COND_AMT")%>'></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                ErrorMessage="請輸入數字金額且不允負數" ClientValidationFunction="CheckEdinum" ForeColor="Red"
                                ControlToValidate="txt_EDIT_INS_COND_AMT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="checknum" runat="server" ValidateEmptyText="true"
                                ErrorMessage="自選保額格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_INS_COND_AMT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Style="text-align: right" Width="100%" ID="txt_NEW_INS_COND_AMT" runat="server" MaxLength="4" ClientIDMode="Static" CssClass="number" Text="0"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator31" runat="server" ValidateEmptyText="true"
                                ErrorMessage="請輸入數字金額且不允負數" ClientValidationFunction="CheckAddnum" ForeColor="Red"
                                ControlToValidate="txt_NEW_INS_COND_AMT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="checknum2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="自選保額格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_INS_COND_AMT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_ENTRY_DT%>" SortExpression="INS_ENTRY_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lb_INS_ENTRY_DT" ClientIDMode="Static" runat="server" Text='<%#Bind("INS_ENTRY_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_INS_ENTRY_DT" runat="server" ClientIDMode="Static" Text='<%#Bind("INS_ENTRY_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_INS_ENTRY_DT" runat="server" MaxLength="8" Width="100%" CssClass="MandatoryField ym date" ClientIDMode="Static" Text=""></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_INS_ENTRY_DT%>"
                                ControlToValidate="txt_NEW_INS_ENTRY_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="qrytest5" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_INS_ENTRY_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_INS_ENTRY_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_QUIT_DT%>" SortExpression="INS_QUIT_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lb_INS_QUIT_DT" runat="server" Text='<%#Bind("INS_QUIT_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_INS_QUIT_DT" runat="server" MaxLength="8" Width="100%" CssClass="MandatoryField ym date" ClientIDMode="Static" Text='<%#Bind("INS_QUIT_DT", "{0:yyyy/MM/dd}")%>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sk_required_INS_QUIT_DT%>"
                                ControlToValidate="txt_EDIT_INS_QUIT_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator99" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_greaterthan_INS_QUIT_DT%>" ClientValidationFunction="CheckINS_QUIT_DT" ForeColor="Red"
                                ControlToValidate="txt_EDIT_INS_QUIT_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="qrytest4" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_INS_QUIT_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_EDIT_INS_QUIT_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_INS_QUIT_DT" runat="server" MaxLength="8" Width="100%" CssClass="MandatoryField ym date" ClientIDMode="Static" Text="9999/12/31"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sk_required_INS_QUIT_DT%>"
                                ControlToValidate="txt_NEW_INS_QUIT_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="greaterthan1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_greaterthan_INS_QUIT_DT%>" ClientValidationFunction="CheckINS_QUIT_DT_Add" ForeColor="Red"
                                ControlToValidate="txt_NEW_INS_QUIT_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="qrytest3" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_INS_QUIT_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_INS_QUIT_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>

                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td width="30px">
                                <asp:CheckBox ID="cb_checkall" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                            </td>
                            <td width="40px">
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sk_RowNum%>"></asp:Label>
                            </td>
                            <td width="80px">
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ia_IDENTITY_KIND%>"></asp:Label>
                            </td>
                            <td width="150px">
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2ia_EMP_NAME%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_BIRTH_DT%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_TARGET_TYPE%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_GINS_KIND%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_INS_COND_AMT%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_INS_ENTRY_DT%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_INS_QUIT_DT%>"></asp:Label>
                            </td>

                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:CheckBox ID="cb_check" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lb_EmptyRowNumber" runat="server" Text="1"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_IDENTITY_KIND" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:CustomValidator ID="CustomValidator6" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_required_IDENTITY_KIND%>" ClientValidationFunction="ddlCheck" ForeColor="Red"
                                    ControlToValidate="ddl_IDENTITY_KIND" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox Style="text-align: left" ID="txt_NEW_LICENSE_ID" Width="150px" runat="server" MaxLength="20" ClientIDMode="Static" CssClass="MandatoryField" onblur="idCheck(this.value);"></asp:TextBox>
                                <input id="bt_NEW_LICENSE_ID" type="button" value="..." onclick="OpenIDSearch('txt_NEW_LICENSE_ID', 'txt_NEW_EMP_NAME');" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_LICENSE_ID%>"
                                    ControlToValidate="txt_NEW_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2dj_format_allowance_type%>" ControlToValidate="txt_NEW_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_EMP_NAME" Width="100px" runat="server" ClientIDMode="Static" BorderWidth="0" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_BIRTH_DT" Width="100px" runat="server" ClientIDMode="Static" BorderWidth="0" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TARGET_TYPE" Width="100px" runat="server" ClientIDMode="Static" BorderWidth="0" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_GINS_KIND" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:CustomValidator ID="CustomValidator7" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_required_GINS_KIND%>" ClientValidationFunction="ddlCheck2" ForeColor="Red"
                                    ControlToValidate="ddl_GINS_KIND" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox Style="text-align: right" Width="120px" ID="txt_NEW_INS_COND_AMT" runat="server" MaxLength="4" ClientIDMode="Static" CssClass="number" Text="0"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator8" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_error_INS_COND_AMT%>" ClientValidationFunction="CheckAddnum" ForeColor="Red"
                                    ControlToValidate="txt_NEW_INS_COND_AMT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="checknum3" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="自選保額格式錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_INS_COND_AMT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_INS_ENTRY_DT" runat="server" MaxLength="8" Width="100px" CssClass="MandatoryField ym date" ClientIDMode="Static" Text=""></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_INS_ENTRY_DT%>"
                                    ControlToValidate="txt_NEW_INS_ENTRY_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="qrytest2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_INS_ENTRY_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_INS_ENTRY_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_INS_QUIT_DT" runat="server" MaxLength="8" Width="100px" CssClass="MandatoryField ym date" ClientIDMode="Static" Text="9999/12/31"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sk_required_INS_QUIT_DT%>"
                                    ControlToValidate="txt_NEW_INS_QUIT_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="greaterthan" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_greaterthan_INS_QUIT_DT%>" ClientValidationFunction="CheckINS_QUIT_DT_Add" ForeColor="Red"
                                    ControlToValidate="txt_NEW_INS_QUIT_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_INS_QUIT_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_INS_QUIT_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </td>



                        </tr>
                    </table>

                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
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

            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_IDENTITY_KIND" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_GINS_KIND" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_LICENSE_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_INS_COND_AMT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_INS_ENTRY_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_INS_QUIT_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_TARGET_TYPE" runat="server" ClientIDMode="Static" />

            <asp:HiddenField ID="add_check" runat="server" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2ia_Del_ConfirmMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2ia_Del_ConfirmMessage%>" />
            <asp:HiddenField ID="hid_wfb2ia_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2ia_Del_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2ia_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2ia_Mod_NotChoiceMessage%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
