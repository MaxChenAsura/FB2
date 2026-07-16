<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2dg/WFB2DG0100_Qry.aspx.cs" Inherits="WebContent_fb2dg_WFB2DG0100_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        h1 {
            text-transform: uppercase;
        }

        #txt_CAR_PARK_NO_Add {
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
            $('#txt_CAR_PARK_NO').val("");
            $('#ddl_PLANT_CD').val(-1);
            //$('#txt_START_YM').val("");
            //$('#txt_LICENSE_ID').val("");
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
                alert($('#hidwfb2_Dtl_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb2dg_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            var processed = true;
            if (LookUpCheckboxs() > 0) {
                processed = true;
            } else {
                alert($('#hidwfb2_Dtl_NotChoiceMessage').val());
                processed = false;
                return processed;
            }

            if (confirm($('#hidwfb2dg_Del_ConfirmMessage').val())) {
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

            if (!processed) {
                $.unblockUI();
                return;
            }
            return processed;
        }

        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hidWFB2IB_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hidWFB2IB_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hidWFB2IB_Save_ConfirmMessage').val());
        }

        function LookUpCheckboxs() {
            var choietr = null;
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    if (choietr == null)
                        choietr = i;
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
                                <col width="15%" />
                                <col width="15%" />
                                <col width="10%" />
                                <col width="15%" />
                                <col width="20%" />
                                <col width="25%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_CAR_PARK_NO" runat="server" Text="<%$Resources:Resource,wfb2dg_CAR_PARK_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CAR_PARK_NO" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2dg_CAR_PARK_NO_Add_Error%>" ControlToValidate="txt_CAR_PARK_NO" ForeColor="Red"
                                            ValidationExpression="^[A-Za-z0-9]+$" Display="None" ValidationGroup="GroupB">
                                        </asp:RegularExpressionValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2dg_PLANT_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_PLANT_CD_Required%>"
                                            ControlToValidate="ddl_PLANT_CD" ForeColor="Red" ValidationGroup="GroupB" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>


                                    </td>

                                </tr>

                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DG010Search" runat="server" Text="<%$Resources:Resource,btn_Search%>" OnClick="WFB2DG010Search_Click" ValidationGroup="GroupB" />

                                            <%--<asp:Button ID="WFB2DG010Search" runat="server" Text="<%$Resources:Resource,btn_Search%>" OnClick="WFB2DG010Search_Click" ValidationGroup="GroupB" />--%>
                                            
                                            <asp:Button ID="WFB2DG010Clear" runat="server" type="button" Text="<%$Resources:Resource,btn_Clear%>" OnClientClick="ClearAll();" OnClick="WFB2DG010Clear_Click" />
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
                        <aces:Btn ID="WFB2DG010Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2DG010Add_Click" />
                            <aces:Btn ID="WFB2DG010Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DG010Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2DG010Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2DG010Edit_Click" Visible="False" />
                            <aces:Btn ID="WFB2DG010Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2DG010Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" Visible="False" />

<%--                            <asp:Button ID="WFB2DG010Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2DG010Add_Click" />
                            <asp:Button ID="WFB2DG010Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DG010Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2DG010Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2DG010Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2DG010Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2DG010Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" Visible="False" />--%>
                            
                            <asp:Button ID="WFB2DG010Cancel" runat="server" Text="<%$Resources:Resource,btn_Cancel%>" Visible="false" OnClick="WFB2DG010Cancel_Click" OnClientClick="return confirm('是否確定取消?');" CausesValidation="false" />
                            
                        </div>

                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="Cfb2DG010DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_CAR_PARK_NO"
                        Name="txt_CAR_PARK_NO" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_PLANT_CD"
                        Name="ddl_PLANT_CD" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />

                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1015px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2ib_RowNumber%>" HeaderStyle-Width="40px">
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

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_CAR_PARK_NO%>" SortExpression="CAR_PARK_NO">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lbl_CAR_PARK_NO" runat="server" Text='<%#Bind("CAR_PARK_NO")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox ID="txt_CAR_PARK_NO_Add" runat="server" Text='<%#Bind("CAR_PARK_NO")%>' MaxLength="10" Enabled="false" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_ERR_CAR_PARK_NO%>"
                                    ControlToValidate="txt_CAR_PARK_NO_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2dg_CAR_PARK_NO_Add_Error%>" ControlToValidate="txt_CAR_PARK_NO_Add" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_CAR_PARK_NO_Add" runat="server" MaxLength="10" Width="120px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_ERR_CAR_PARK_NO%>"
                                    ControlToValidate="txt_CAR_PARK_NO_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2dg_CAR_PARK_NO_Add_Error%>" ControlToValidate="txt_CAR_PARK_NO_Add" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_PLANT_CD%>" SortExpression="PLANT_CD">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_PLANT_CD" runat="server" Text='<%#Bind("PLANT_NAME")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:DropDownList ID="ddl_PLANT_CD_Add" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_ERR_PLANT_CD%>"
                                    ControlToValidate="ddl_PLANT_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                </asp:RequiredFieldValidator>

                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_PLANT_CD_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_ERR_PLANT_CD%>"
                                    ControlToValidate="ddl_PLANT_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_PARKING_NAME%>" SortExpression="PARKING_NAME">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lbl_PARKING_NAME" runat="server" Text='<%#Bind("PARKING_NAME")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox ID="txt_PARKING_NAME_Add" MaxLength="30" ClientIDMode="Static" runat="server" Text='<%#Bind("PARKING_NAME")%>' Width="240px" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_ERR_PARKING_NAME%>"
                                    ControlToValidate="txt_PARKING_NAME_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                </asp:RequiredFieldValidator>

                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_PARKING_NAME_Add" runat="server" MaxLength="30" Width="240px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_ERR_PARKING_NAME%>"
                                    ControlToValidate="txt_PARKING_NAME_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_PARKING_TYPE%>" SortExpression="SUBPARKING_TYPE">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_PARKING_TYPE" runat="server" Text='<%# Convert.ToString(Eval("SUBPARKING_TYPE"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:DropDownList ID="ddl_PARKING_TYPE_Add" ClientIDMode="Static" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_ERR_PARKING_TYPE%>"
                                    ControlToValidate="ddl_PARKING_TYPE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:DropDownList ID="ddl_PARKING_TYPE_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_ERR_PARKING_TYPE%>"
                                    ControlToValidate="ddl_PARKING_TYPE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_PARKING_SPOT%>" SortExpression="PARKING_SPOT">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lbl_PARKING_SPOT" runat="server" Text='<%# Convert.ToString(Eval("PARKING_SPOT"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:TextBox ID="txt_PARKING_SPOT_Add" MaxLength="3" ClientIDMode="Static" runat="server" Text='<%#Bind("PARKING_SPOT")%>' Width="55px" CssClass="MandatoryField" Style="ime-mode: disabled" onkeyup="this.value=this.value.replace(/[^0-9]/g,'')"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_ERR_PARKING_SPOT%>"
                                    ControlToValidate="txt_PARKING_SPOT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                </asp:RequiredFieldValidator>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:TextBox ID="txt_PARKING_SPOT_Add" MaxLength="3" ClientIDMode="Static" runat="server" Text='<%#Bind("PARKING_SPOT")%>' Width="55px" CssClass="MandatoryField" Style="ime-mode: disabled" onkeyup="this.value=this.value.replace(/[^0-9]/g,'')"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_ERR_PARKING_SPOT%>"
                                    ControlToValidate="txt_PARKING_SPOT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_USING_PARKING_SPOT%>" SortExpression="USING_PARKING_SPOT">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lbl_USING_PARKING_SPOT" runat="server" Text='<%# Convert.ToString(Eval("USING_PARKING_SPOT"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%;">
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%;">
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_REMAINDER_PARKING_SPOT%>" SortExpression="REMAINDER_PARKING_SPOT">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lbl_REMAINDER_PARKING_SPOT" runat="server" Text='<%# Convert.ToString(Eval("REMAINDER_PARKING"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 60px;">
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 60px;">
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_OVERLAP%>" SortExpression="OVERLAP">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lbl_OVERLAP" runat="server" Text='<%# Convert.ToString(Eval("OVERLAP"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:TextBox ID="txt_OVERLAP_Add" MaxLength="3" ClientIDMode="Static" runat="server" Text='<%#Bind("OVERLAP")%>' Width="55px" CssClass="MandatoryField" Style="ime-mode: disabled" onkeyup="this.value=this.value.replace(/[^0-9]/g,'')"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_ERR_OVERLAP%>"
                                    ControlToValidate="txt_OVERLAP_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                </asp:RequiredFieldValidator>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:TextBox ID="txt_OVERLAP_Add" MaxLength="3" ClientIDMode="Static" runat="server" Text='<%#Bind("OVERLAP")%>' Width="55px" CssClass="MandatoryField" Style="ime-mode: disabled" onkeyup="this.value=this.value.replace(/[^0-9]/g,'')"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_ERR_OVERLAP%>"
                                    ControlToValidate="txt_OVERLAP_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dg_NEEDSELECT%>" SortExpression="NEEDSELECT">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_NEEDSELECT" runat="server" Text='<%#Bind("NEEDSELECT")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:DropDownList ID="ddl_NEEDSELECT_Add" runat="server" CssClass="MandatoryField">
                                    <asp:ListItem Value="Y" Text="Y" ></asp:ListItem>
                                    <asp:ListItem Value="N" Text="N" ></asp:ListItem>                                 
                                </asp:DropDownList>    
                                <asp:HiddenField  id="hid_NEEDSELECT" runat="server" ClientIDMode="Static" value='<%#Bind("NEEDSELECT")%>' />                            
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_NEEDSELECT_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                    <asp:ListItem Value="Y" Text="Y" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="N" ></asp:ListItem>
                                </asp:DropDownList>                                
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                </Columns>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td width="20px"></td>
                            <td width="40px">
                                <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb299_RowNumber%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderCAR_PARK_NO" runat="server" Text="<%$Resources:Resource,wfb2dg_CAR_PARK_NO%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderPLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2dg_PLANT_CD%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderPARKING_NAME" runat="server" Text="<%$Resources:Resource,wfb2dg_PARKING_NAME%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderPARKING_TYPE" runat="server" Text="<%$Resources:Resource,wfb2dg_PARKING_TYPE%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderPARKING_SPOT" runat="server" Text="<%$Resources:Resource,wfb2dg_PARKING_SPOT%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderUSING_PARKING_SPOT" runat="server" Text="<%$Resources:Resource,wfb2dg_USING_PARKING_SPOT%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderREMAINDER_PARKING_SPOT" runat="server" Text="<%$Resources:Resource,wfb2dg_REMAINDER_PARKING_SPOT%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderOVERLAP" runat="server" Text="<%$Resources:Resource,wfb2dg_OVERLAP%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderPARKING_LC_TYPE" runat="server" Text="<%$Resources:Resource,wfb2dg_PARKING_LC_TYPE%>"></asp:Label>

                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderNEEDSELECT" runat="server" Text="<%$Resources:Resource,wfb2dg_NEEDSELECT%>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <div style="text-align: center; width: 100%;">
                                    <asp:TextBox ID="txt_CAR_PARK_NO_Add" MaxLength="6" ClientIDMode="Static" runat="server"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                        ErrorMessage="<%$Resources:Resource,wfb2dg_alphanumeric_Error%>" ControlToValidate="txt_CAR_PARK_NO_Add" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:DropDownList ID="ddl_PLANT_CD_Add" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_PARKING_NAME_Add" runat="server" MaxLength="30" Width="120px" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:DropDownList ID="ddl_PARKING_TYPE_Add" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_PARKING_SPOT_Add" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("PARKING_SPOT")%>'></asp:TextBox>
                                </div>
                            </td>
                            <td></td>
                            <td></td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_OVERLAP_Add" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("OVERLAP")%>'></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_PARKING_LC_TYPE_Add" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("PARKING_LC_TYPE")%>'></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:DropDownList ID="ddl_NEEDSELECT_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                        <asp:ListItem Value="Y" Text="Y" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="N" Text="N" ></asp:ListItem>
                                    </asp:DropDownList> 
                                </div>
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
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dg_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2dg_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2_alphanumeric_onlyMessage" Value="<%$Resources:Resource,wfb2_alphanumeric_onlyMessage%>" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


