<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0400_Qry.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0400_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_EMP_NAME").attr("readonly", true);
            $(".date").mask('9999/99/99');
            $("#txt_NEW_EMP_ID").mask("99999");
            $("#txt_EMP_ID").mask("99999");
            gridviewScroll();
            $.unblockUI();

            //工號取得姓名的ajax
            $("#txt_EMP_ID").blur(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
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

            $('#txt_NEW_EMP_ID').blur(function () {
                if ($("#txt_NEW_EMP_ID").val().length == 5) {
                    //ajax 取得員工基本資料
                    $.ajax({
                        url: "WFB2HB0400_GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_NEW_EMP_ID').val()
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "")
                                alert(JData.errMsg);
                            else {
                                $('#lb_NEW_EMP_NAME').text(JData.EMP_NAME);
                                $('#lb_ORI_NEW_DEPT_NAME').text(JData.DEPT_NAME);

                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#lb_NEW_EMP_NAME').val("");
                    $('#lb_ORI_NEW_DEPT_NAME').val("");
                }
            });
        }

        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"

            });

        }

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_TRAINING_COMPANY").val("");
            $("#txt_TRAINING_GOAL").val("");
            $("#txt_START_DT_S").val("");
            $("#txt_START_DT_E").val("");
        }

        function CheckSearch() {

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return false;
        }

        //儲存前檢查
        function saveCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return;
            if (!processed)
                $.unblockUI();

            return processed;
        }

        function openUpload() {
            window.location.href("WFB2HB0400_Upload.aspx");
            return false;
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function cancelClientClick() {
            return confirm($("#hid_cancel_ConfirmMessage").val());
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1) {
                return true;
            }
            else {
                alert($('#hid_chooseOneMessage').val());
                return false;
            }
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0) {
                var rtnval = confirm($('#hid_delete_ConfirmMessage').val());
                if (rtnval) BlockUI();
                return rtnval;
            }
            else {
                alert($('#hid_notChooseMessage').val());
                return false;
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("#gv_result [type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="12%" />
                    <col width="20%" />
                    <col width="12%" />
                    <col width="15%" />
                    <col width="18%" />
                    <col width="40%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="2">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="6" Width="42px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" ></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_START_DT_S_E" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_START_DT_S_E%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="2">
                            <asp:TextBox ID="txt_START_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            ~
                    <asp:TextBox ID="txt_START_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:CustomValidator ID="cv_START_DT_S" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_CP_START_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="cv_START_DT_E" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_CP_START_DT_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_START_DT_S"
                                        ControlToValidate="txt_START_DT_E" ErrorMessage="<%$Resources:Resource,wfb2hb_CP_START_DT%>" Type="Date" Operator="GreaterThanEqual"
                                        Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_TRAINING_COMPANY" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRAINING_COMPANY%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="2">
                            <asp:TextBox ID="txt_TRAINING_COMPANY" runat="server" MaxLength="150" Width="200px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_TRAINING_GOAL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRAINING_GOAL%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_TRAINING_GOAL" runat="server" MaxLength="150" Width="200px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <td align="left" class="Body_label"></td>
                        <td align="left" class="Body_label"></td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <aces:Btn ID="WFB2HB0400Search" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Search%>" OnClick="WFB2HA0400Search_Click" OnClientClick="return CheckSearch();" />
                                <%--<asp:Button ID="WFB2HB0400Search" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Search%>" OnClick="WFB2HA0400Search_Click" OnClientClick="return CheckSearch();" />--%>
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2hb_btn_clear%>" onclick="ClearAll();" />
                                <aces:Btn ID="WFB2HB0400Upload" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Upload%>" OnClientClick="return openUpload();" />
                                <%--<asp:Button ID="WFB2HB0400Upload" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Upload%>" OnClientClick="return openUpload();" />--%>

                                <asp:HiddenField ID="hid_cancel_ConfirmMessage" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hid_delete_ConfirmMessage" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hid_notChooseMessage" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hid_chooseOneMessage" ClientIDMode="Static" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="10">
                            
                            <aces:Btn ID="WFB2HB0400Add" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Add%>" OnClick="WFB2HB0400Add_Click" />
                            <aces:Btn ID="WFB2HB0400Delete" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2HB0400Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2HB0400Edit" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HB0400Edit_Click" Visible="false" />
                            <aces:Btn ID="WFB2HB0400Save" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Save%>" Visible="false" OnClick="WFB2HB0400Save_Click" OnClientClick="return saveCheck();" />
                             
                            <%--<asp:Button ID="WFB2HB0400Add" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Add%>" OnClick="WFB2HB0400Add_Click" />
                            <asp:Button ID="WFB2HB0400Delete" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2HB0400Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2HB0400Edit" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HB0400Edit_Click" Visible="false" />
                            <asp:Button ID="WFB2HB0400Save" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Save%>" Visible="false" OnClick="WFB2HB0400Save_Click" OnClientClick="return saveCheck();" />--%>
                            <asp:Button ID="WFB2HB0400Cancel" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0400Cancel%>" Visible="false" OnClick="WFB2HB0400Cancel_Click" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val());" />
                        </td>
                    </tr>
                    <!-- END: Create a line -->
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HB0400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="emp_id" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_START_DT_S" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt_s" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_START_DT_E" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt_e" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_TRAINING_COMPANY" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="training_company" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_TRAINING_GOAL" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="training_goal" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1100px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_EMP_ID" runat="server" MaxLength="5" Width="40px" CssClass="MandatoryField AjaxEMPID" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_EMP_ID%>"
                                ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <FooterStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_EMP_NAME" runat="server" ClientIDMode="Static"></asp:Label>
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <FooterStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_ORI_DEPT_NO%>" SortExpression="ORI_DEPT_NO" HeaderStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lb_ORI_DEPT_NAME" runat="server" Text='<%#Bind("ORI_DEPT_FULL_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_ORI_DEPT_NAME" runat="server" Text='<%#Bind("ORI_DEPT_FULL_NAME")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_ORI_NEW_DEPT_NAME" runat="server" ClientIDMode="Static"></asp:Label>
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <FooterStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_START_DT" runat="server" MaxLength="10" Width="81px" BorderWidth="0" ReadOnly="true" Text='<%#Bind("START_DT")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_START_DT" runat="server" MaxLength="10" Width="81px" CssClass="MandatoryField date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_START_DT%>"
                                ControlToValidate="txt_NEW_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cv_NEW_START_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_START_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_NEW_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_END_DT%>" SortExpression="END_DT" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_DT" runat="server" Text='<%#Bind("END_DT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_END_DT" runat="server" MaxLength="10" Width="81px" CssClass="MandatoryField date" Text='<%#Bind("END_DT")%>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_END_DT%>"
                                ControlToValidate="txt_EDIT_END_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cv_NEW_END_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_END_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_EDIT_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_EDIT_START_DT"
                                ControlToValidate="txt_EDIT_END_DT" ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_START_DT_Compare%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_END_DT" runat="server" MaxLength="10" Width="81px" CssClass="MandatoryField date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_END_DT%>"
                                ControlToValidate="txt_NEW_END_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cv_NEW_END_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_END_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_NEW_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_NEW_START_DT"
                                ControlToValidate="txt_NEW_END_DT" ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_START_DT_Compare%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_TRAINING_COMPANY%>" SortExpression="TRAINING_COMPANY" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_TRAINING_COMPANY" runat="server" Text='<%#Bind("TRAINING_COMPANY")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_TRAINING_COMPANY" runat="server" MaxLength="150" CssClass="MandatoryField" Text='<%#Bind("TRAINING_COMPANY")%>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_TRAINING_COMPANY%>"
                                ControlToValidate="txt_EDIT_TRAINING_COMPANY" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_TRAINING_COMPANY" runat="server" MaxLength="150" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_TRAINING_COMPANY%>"
                                ControlToValidate="txt_NEW_TRAINING_COMPANY" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <FooterStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_TRAINING_GOAL%>" SortExpression="TRAINING_GOAL" HeaderStyle-Width="200px">
                        <ItemTemplate>
                            <asp:Label ID="lb_TRAINING_GOAL" runat="server" Text='<%#Bind("TRAINING_GOAL")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_TRAINING_GOAL" runat="server" MaxLength="150" CssClass="MandatoryField" Text='<%#Bind("TRAINING_GOAL")%>' Width="100%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_TRAINING_GOAL%>"
                                ControlToValidate="txt_EDIT_TRAINING_GOAL" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_TRAINING_GOAL" runat="server"  MaxLength="150" CssClass="MandatoryField" Width="100%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_TRAINING_GOAL%>"
                                ControlToValidate="txt_NEW_TRAINING_GOAL" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <FooterStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>

                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td></td>
                            <td width="40px;">
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2hb_RowNumber%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2hb_ORI_DEPT_NO%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2hb_START_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2hb_END_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRAINING_COMPANY%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRAINING_GOAL%>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td align="left">
                                <asp:TextBox ID="txt_NEW_EMP_ID" runat="server" MaxLength="5" Width="40px" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_EMP_ID%>"
                                    ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_NEW_START_DT" runat="server" MaxLength="10" Width="81px" CssClass="MandatoryField date"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_START_DT%>"
                                    ControlToValidate="txt_NEW_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cv_NEW_START_DT" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_START_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_END_DT" runat="server" MaxLength="10" Width="81px" CssClass="MandatoryField date"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_END_DT%>"
                                    ControlToValidate="txt_NEW_END_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cv_NEW_END_DT" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_END_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_NEW_START_DT"
                                    ControlToValidate="txt_NEW_END_DT" ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_START_DT_Compare%>" Type="Date" Operator="GreaterThanEqual"
                                    Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_NEW_TRAINING_COMPANY"  MaxLength="150" runat="server" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_TRAINING_COMPANY%>"
                                    ControlToValidate="txt_NEW_TRAINING_COMPANY" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_NEW_TRAINING_GOAL"   MaxLength="150" runat="server" CssClass="MandatoryField"  Width="100%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_TRAINING_GOAL%>"
                                    ControlToValidate="txt_NEW_TRAINING_GOAL" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>

                        </tr>
                    </table>

                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
            </asp:GridView>
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
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />

            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
