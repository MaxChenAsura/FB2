<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ib/WFB2IB0100_Qry.aspx.cs" Inherits="WebContent_fb2ib_WFB2IB0100_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>



    <script type="text/javascript">



        jQuery(document).ready(function () {

            iniForm();
            $(".PERSON").blur(function () {
                alert("This input field has lost its focus.");

            });

        });


        function iniForm() {
            $("#txt_YEAR_MONTH_Add").datepicker({ dateFormat: 'yy/mm' });
            $(".number").mask('9999/99');
            $(".number2").mask('99.999');
            $(".ym").mask('9999/99');
            $("#txt_INS_MAX_MONTH_Add").mask('99');
            $("#txt_INS_MIN_AMOUNT_Add").mask('99999999');
            $("#txt_INS_MAX_AMOUNT_Add").mask('99999999');
            $("#txt_INS_CON_MIN_AMOUNT_Add").mask('99999999');
            $("#txt_INS_CON_MAX_AMOUNT_Add").mask('99999999');
            $("#txt_INS_SALE_MIN_AMOUNT_Add").mask('99999999');
            $("#txt_INS_SALE_MAX_AMOUNT_Add").mask('99999999');
            $("#txt_INS_STOCK_MIN_AMOUNT_Add").mask('99999999');
            $("#txt_INS_STOCK_MAX_AMOUNT_Add").mask('99999999');
            $("#txt_INS_INTEREST_MIN_AMOUNT_Add").mask('99999999');
            $("#txt_INS_INTEREST_MAX_AMOUNT_Add").mask('99999999');

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
                barcolor: "#7F7F7F",
                freezesize: 3,
                headerrowcount: 2
            });
            //CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");

            var isAdd = $("#HID_isAdd").val();
            if (isAdd == "1") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F",
                    headerrowcount: 2
                });
            }
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

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidWFB2IB_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidWFB2IB_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {               
                if (parseInt($("#txt_INS_MAX_AMOUNT_Add").val()) <= parseInt($("#txt_INS_MIN_AMOUNT_Add").val())) {                    
                    alert($('#hidWFB2IB_RENT_ERROR').val());
                    return false;
                }                
                if (parseInt($("#INS_CON_MAX_AMOUNT").val()) <= parseInt($("#INS_CON_MIN_AMOUNT").val())) {
                    alert($('#hidWFB2IB_CON_ERROR').val());
                    return false;
                }
                if (parseInt($("#txt_INS_SALE_MAX_AMOUNT_Add").val()) <= parseInt($("#txt_INS_SALE_MIN_AMOUNT_Add").val())) {
                    alert($('#hidWFB2IB_SALE_ERROR').val());
                    return false;
                }
                if (parseInt($("#txt_INS_STOCK_MAX_AMOUNT_Add").val()) <= parseInt($("#txt_INS_STOCK_MIN_AMOUNT_Add").val())) {
                    alert($('#hidWFB2IB_STOCK_ERROR').val());
                    return false;
                }
                if (parseInt($("#txt_INS_INTEREST_MAX_AMOUNT_Add").val()) <= parseInt($("#txt_INS_INTEREST_MIN_AMOUNT_Add").val())) {
                    alert($('#hidWFB2IB_INTEREST_ERROR').val());
                    return false;
                }


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
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb299_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }
        }
        //判斷是否為數字
        function IsIntText() {
            var charkeycode = window.event.keyCode;
            if (charkeycode > 47 && charkeycode < 58) {  //0的keycode為47 ,9的keycode為58
                return true;
            }
            return false;

        }

        function BlockNumber(e) {
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            reg = /[0-9]|\./;
            return reg.test(keychar);
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--2990400_QRY開始-->
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
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <%--<asp:Button ID="WFB2IB0100Search" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Search%>" OnClick="WFB2IB0100Search_Click" OnClientClick="BlockUI();" />--%>
                                            <%--<asp:Button ID="WFB2IB0100Clear" runat="server" type="button" Text="<%$Resources:Resource,wfb299_WFB2990100Clear%>" OnClientClick="ClearAll();" />--%>
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
                            <aces:Btn ID="WFB2IB0100Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2IB0100Add_Click" />
                            <aces:Btn ID="WFB2IB0100Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IB0100Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2IB0100Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClick="WFB2IB0100Edit_Click" Visible="False" />
                            <aces:Btn ID="WFB2IB0100Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" Visible="false" OnClick="WFB2IB0100Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />

                            <%-- <asp:Button ID="WFB2IB0100Add" runat="server" Text="<%$Resources:Resource,btn_Add%>" OnClick="WFB2IB0100Add_Click" />
                            <asp:Button ID="WFB2IB0100Delete" runat="server" Text="<%$Resources:Resource,btn_Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IB0100Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2IB0100Edit" runat="server" Text="<%$Resources:Resource,btn_Edit%>" OnClick="WFB2IB0100Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2IB0100Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" Visible="false" OnClick="WFB2IB0100Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />--%>

                            <asp:Button ID="WFB2IB0100Cancel" runat="server" Text="<%$Resources:Resource,btn_Cancel%>" Visible="false" OnClick="WFB2IB0100Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                        </div>

                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="Cfb2ib0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />


                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1400px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
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
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2ib_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
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

                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2IB_YEAR_MONTH%>" SortExpression="YEAR_MONTH" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="YEAR_MONTH" runat="server" Text='<%#Bind("YEAR_MONTH")%>' CssClass="ym" ClientIDMode="Static"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="txt_YEAR_MONTH_Add" runat="server" onkeypress="return IsIntText();" onpaste="return false" Text='<%#Bind("YEAR_MONTH")%>' Width="60px" CssClass="ym" ClientIDMode="Static"></asp:Label>
                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_YEAR_MONTH_Add" runat="server" MaxLength="6" onkeypress="return IsIntText();" onpaste="return false" Width="60px" ClientIDMode="Static" CssClass="MandatoryField number"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_YEAR_MONTH_Add%>"
                                    ControlToValidate="txt_YEAR_MONTH_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="reg_DEF_YM" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ib_ERR_YEAR_MONTH_Add_DT%>" ControlToValidate="txt_YEAR_MONTH_Add" ForeColor="Red"
                                    ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                </asp:RegularExpressionValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2IB_INS_RATE_PERSON%>" SortExpression="INS_RATE_PERSON" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:Label ID="INS_RATE_PERSON" runat="server" Text='<%#Bind("INS_RATE_PERSON")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:TextBox ID="txt_INS_RATE_PERSON_Add" CssClass="MandatoryField number2" onKeypress="return BlockNumber(event);" Width="80px" MaxLength="5" ClientIDMode="Static" runat="server" Text='<%#Bind("INS_RATE_PERSON")%>' Style="text-align: right"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_RATE_PERSON_Add%>"
                                    ControlToValidate="txt_INS_RATE_PERSON_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ib_INS_RATE_PERSON_ERR%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_INS_RATE_PERSON_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </div>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_RATE_PERSON_Add" runat="server" Style="text-align: right" CssClass="MandatoryField number2" onKeypress="return BlockNumber(event);" MaxLength="5" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_RATE_PERSON_Add%>"
                                    ControlToValidate="txt_INS_RATE_PERSON_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ib_INS_RATE_PERSON_ERR%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_INS_RATE_PERSON_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>


                    <%--                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_0400_lbl_MODE_ID%>" SortExpression="MODE_ID">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_MODE_ID" runat="server" Text='<%#Bind("MODE_ID")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_MODE_ID_Add" runat="server" MaxLength="50" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="請輸入參數別"
                                    ControlToValidate="txt_MODE_ID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>--%>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2IB_INS_RATE_COMP%>" SortExpression="INS_RATE_COMP" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="INS_RATE_COMP" runat="server" Text='<%# Convert.ToString(Eval("INS_RATE_COMP"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:TextBox ID="txt_INS_RATE_COMP_Add" MaxLength="5" ClientIDMode="Static" Style="text-align: right" onKeypress="return BlockNumber(event);" CssClass="MandatoryField number2" Width="80px" runat="server" Text='<%#Bind("INS_RATE_COMP")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_RATE_COMP_Add%>"
                                    ControlToValidate="txt_INS_RATE_COMP_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ib_INS_RATE_COMP_Add_ERR%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_INS_RATE_COMP_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:TextBox ID="txt_INS_RATE_COMP_Add" MaxLength="5" ClientIDMode="Static" Style="text-align: right" onKeypress="return BlockNumber(event);" CssClass="MandatoryField number2" Width="80px" runat="server" Text='<%#Bind("INS_RATE_COMP")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_RATE_COMP_Add%>"
                                    ControlToValidate="txt_INS_RATE_COMP_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ib_INS_RATE_COMP_Add_ERR%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_INS_RATE_COMP_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2IB_INS_MAX_MONTH%>" SortExpression="INS_MAX_MONTH" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="INS_MAX_MONTH" runat="server" Text='<%#Bind("INS_MAX_MONTH")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:TextBox ID="txt_INS_MAX_MONTH_Add" MaxLength="2" onkeypress="return IsIntText();" Style="text-align: right" onpaste="return false" ClientIDMode="Static" Width="80px" CssClass="MandatoryField" runat="server" Text='<%#Bind("INS_MAX_MONTH")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_MAX_MONTH_Add%>"
                                    ControlToValidate="txt_INS_MAX_MONTH_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_MAX_MONTH_Add" runat="server" MaxLength="2" Style="text-align: right" onkeypress="return IsIntText();" onpaste="return false" Width="80px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_MAX_MONTH_Add%>"
                                    ControlToValidate="txt_INS_MAX_MONTH_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2IB_INS_CON_MIN_AMOUNT%>" SortExpression="INS_CON_MIN_AMOUNT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="INS_CON_MIN_AMOUNT" runat="server" Text='<%#Bind("INS_CON_MIN_AMOUNT","{0:N0}")%>'></asp:Label>

                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_CON_MIN_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" Style="text-align: right" onpaste="return false" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static" Text='<%#Bind("INS_CON_MIN_AMOUNT")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_CON_MIN_AMOUNT%>"
                                    ControlToValidate="txt_INS_CON_MIN_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_CON_MIN_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" Style="text-align: right" onpaste="return false" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_CON_MIN_AMOUNT_Add%>"
                                    ControlToValidate="txt_INS_CON_MIN_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2IB_INS_CON_MAX_AMOUNT%>" SortExpression="INS_CON_MAX_AMOUNT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="INS_CON_MAX_AMOUNT" runat="server" Text='<%#Bind("INS_CON_MAX_AMOUNT","{0:N0}")%>'></asp:Label>

                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_CON_MAX_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" Style="text-align: right" onpaste="return false" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static" Text='<%#Bind("INS_CON_MAX_AMOUNT")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_CON_MAX_AMOUNT%>"
                                    ControlToValidate="txt_INS_CON_MAX_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_CON_MAX_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" Style="text-align: right" onpaste="return false" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_CON_MAX_AMOUNT_Add%>"
                                    ControlToValidate="txt_INS_CON_MAX_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2IB_INS_RENT_MIN_AMOUNT%>" SortExpression="INS_RENT_MIN_AMOUNT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="INS_MIN_AMOUNT" runat="server" Text='<%#Bind("INS_RENT_MIN_AMOUNT","{0:N0}")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:TextBox ID="txt_INS_MIN_AMOUNT_Add" MaxLength="8" ClientIDMode="Static" Style="text-align: right" onkeypress="return IsIntText();" onpaste="return false" Width="80px" CssClass="MandatoryField" runat="server" Text='<%#Bind("INS_RENT_MIN_AMOUNT")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_RENT_MIN_AMOUNT_Add%>"
                                    ControlToValidate="txt_INS_MIN_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_MIN_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" Style="text-align: right" onpaste="return false" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_RENT_MIN_AMOUNT_Add%>"
                                    ControlToValidate="txt_INS_MIN_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2IB_INS_RENT_MAX_AMOUNT%>" SortExpression="INS_RENT_MAX_AMOUNT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="INS_MAX_AMOUNT" runat="server" Text='<%#Bind("INS_RENT_MAX_AMOUNT","{0:N0}")%>'></asp:Label>

                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_MAX_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" Style="text-align: right" onpaste="return false" Width="80px" MaxLength="8" CssClass="MandatoryField" ClientIDMode="Static" Text='<%#Bind("INS_RENT_MAX_AMOUNT")%>' ItemStyle-HorizontalAlign="Right" DataFormatString="{0:n0}"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_RENT_MAX_AMOUNT_Add%>"
                                    ControlToValidate="txt_INS_MAX_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_MAX_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" Style="text-align: right" onpaste="return false" Width="80px" MaxLength="8" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_RENT_MAX_AMOUNT_Add%>"
                                    ControlToValidate="txt_INS_MAX_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2IB_INS_SALE_MIN_AMOUNT%>" SortExpression="INS_SALE_MIN_AMOUNT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lb_INS_SALE_MIN_AMOUNT" runat="server" Text='<%#Bind("INS_SALE_MIN_AMOUNT","{0:N0}")%>'></asp:Label>

                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_SALE_MIN_AMOUNT_Add" onkeypress="return IsIntText();" onpaste="return false" Style="text-align: right" runat="server" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static" Text='<%#Bind("INS_SALE_MIN_AMOUNT")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_SALE_MIN_AMOUNT%>"
                                    ControlToValidate="txt_INS_SALE_MIN_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_SALE_MIN_AMOUNT_Add" onkeypress="return IsIntText();" onpaste="return false" runat="server" Style="text-align: right" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_SALE_MIN_AMOUNT%>"
                                    ControlToValidate="txt_INS_SALE_MIN_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2IB_INS_SALE_MAX_AMOUNT%>" SortExpression="INS_SALE_MAX_AMOUNT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lb_INS_SALE_MAX_AMOUNT" runat="server" Text='<%#Bind("INS_SALE_MAX_AMOUNT","{0:N0}")%>'></asp:Label>

                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_SALE_MAX_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" onpaste="return false" Style="text-align: right" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static" Text='<%#Bind("INS_SALE_MAX_AMOUNT")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_SALE_MAX_AMOUNT%>"
                                    ControlToValidate="txt_INS_SALE_MAX_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_SALE_MAX_AMOUNT_Add" runat="server" MaxLength="8" onkeypress="return IsIntText();" Style="text-align: right" onpaste="return false" Width="80px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_SALE_MAX_AMOUNT%>"
                                    ControlToValidate="txt_INS_SALE_MAX_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2IB_INS_STOCK_MIN_AMOUNT%>" SortExpression="INS_STOCK_MIN_AMOUNT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lb_INS_STOCK_MIN_AMOUNT" runat="server" Text='<%#Bind("INS_STOCK_MIN_AMOUNT","{0:N0}")%>' ItemStyle-HorizontalAlign="Right"></asp:Label>

                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_STOCK_MIN_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" onpaste="return false" Style="text-align: right" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static" Text='<%#Bind("INS_STOCK_MIN_AMOUNT")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_STOCK_MIN_AMOUNT%>"
                                    ControlToValidate="txt_INS_STOCK_MIN_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_STOCK_MIN_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" onpaste="return false" Style="text-align: right" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_STOCK_MIN_AMOUNT%>"
                                    ControlToValidate="txt_INS_STOCK_MIN_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2IB_INS_STOCK_MAX_AMOUNT%>" SortExpression="INS_STOCK_MAX_AMOUNT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lb_INS_STOCK_MAX_AMOUNT" runat="server" Text='<%#Bind("INS_STOCK_MAX_AMOUNT","{0:N0}")%>' ItemStyle-HorizontalAlign="Right"></asp:Label>

                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_STOCK_MAX_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" onpaste="return false" Style="text-align: right" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static" Text='<%#Bind("INS_STOCK_MAX_AMOUNT")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_STOCK_MAX_AMOUNT%>"
                                    ControlToValidate="txt_INS_STOCK_MAX_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_STOCK_MAX_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" onpaste="return false" Style="text-align: right" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_STOCK_MAX_AMOUNT%>"
                                    ControlToValidate="txt_INS_STOCK_MAX_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2IB_INS_INTEREST_MIN_AMOUNT%>" SortExpression="INS_INTEREST_MIN_AMOUNT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lb_INS_INTEREST_MIN_AMOUNT" runat="server" Text='<%#Bind("INS_INTEREST_MIN_AMOUNT","{0:N0}")%>' ItemStyle-HorizontalAlign="Right"></asp:Label>

                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_INTEREST_MIN_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" onpaste="return false" Style="text-align: right" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static" Text='<%#Bind("INS_INTEREST_MIN_AMOUNT")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_INTEREST_MIN_AMOUNT%>"
                                    ControlToValidate="txt_INS_INTEREST_MIN_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_INTEREST_MIN_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" onpaste="return false" Style="text-align: right" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_INTEREST_MIN_AMOUNT%>"
                                    ControlToValidate="txt_INS_INTEREST_MIN_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,WFB2IB_INS_INTEREST_MAX_AMOUNT%>" SortExpression="INS_INTEREST_MAX_AMOUNT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lb_INS_INTEREST_MAX_AMOUNT" runat="server" Text='<%#Bind("INS_INTEREST_MAX_AMOUNT","{0:N0}")%>' ItemStyle-HorizontalAlign="Right"></asp:Label>

                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_INTEREST_MAX_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" onpaste="return false" Style="text-align: right" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static" Text='<%#Bind("INS_INTEREST_MAX_AMOUNT")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_INTEREST_MAX_AMOUNT%>"
                                    ControlToValidate="txt_INS_INTEREST_MAX_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_INS_INTEREST_MAX_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" onpaste="return false" Style="text-align: right" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_INTEREST_MAX_AMOUNT%>"
                                    ControlToValidate="txt_INS_INTEREST_MAX_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>

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
                                <asp:Label ID="lblHeaderYEAR_MONTH" runat="server" Text="<%$Resources:Resource,WFB2IB_YEAR_MONTH%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderINS_RATE_PERSON" runat="server" Text="<%$Resources:Resource,WFB2IB_INS_RATE_PERSON%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderINS_RATE_COMP" runat="server" Text="<%$Resources:Resource,WFB2IB_INS_RATE_COMP%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderINS_MAX_MONTH" runat="server" Text="<%$Resources:Resource,WFB2IB_INS_MAX_MONTH%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderBUDGET_INS_MIN_AMOUNT" runat="server" Text="<%$Resources:Resource,WFB2IB_INS_MIN_AMOUNT%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderINS_MAX_AMOUNT" runat="server" Text="<%$Resources:Resource,WFB2IB_INS_MAX_AMOUNT%>"></asp:Label>
                            </td>


                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_YEAR_MONTH_Add" runat="server" MaxLength="6" onkeypress="return IsIntText();" onpaste="return false" Width="60px" ClientIDMode="Static" CssClass="MandatoryField number"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_YEAR_MONTH_Add%>"
                                        ControlToValidate="txt_YEAR_MONTH_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="reg_DEF_YM" runat="server"
                                        ErrorMessage="<%$Resources:Resource,wfb2ib_ERR_YEAR_MONTH_Add_DT%>" ControlToValidate="txt_YEAR_MONTH_Add" ForeColor="Red"
                                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_INS_RATE_PERSON_Add" runat="server" Style="text-align: right" CssClass="MandatoryField" onKeypress="return BlockNumber(event);" MaxLength="5" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_RATE_PERSON_Add%>"
                                        ControlToValidate="txt_INS_RATE_PERSON_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%;">
                                    <asp:TextBox ID="txt_INS_RATE_COMP_Add" MaxLength="5" ClientIDMode="Static" Style="text-align: right" onKeypress="return BlockNumber(event);" CssClass="MandatoryField" Width="80px" runat="server" Text='<%#Bind("INS_RATE_COMP")%>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_RATE_COMP_Add%>"
                                        ControlToValidate="txt_INS_RATE_COMP_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>

                                </div>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_INS_MAX_MONTH_Add" runat="server" MaxLength="2" Style="text-align: right" onkeypress="return IsIntText();" onpaste="return false" Width="80px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_MAX_MONTH_Add%>"
                                        ControlToValidate="txt_INS_MAX_MONTH_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>

                                </div>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_INS_MIN_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" Style="text-align: right" onpaste="return false" MaxLength="8" Width="80px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_RENT_MIN_AMOUNT_Add%>"
                                        ControlToValidate="txt_INS_MIN_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>

                                </div>
                            </td>
                            <td>
                                <div style="text-align: right; width: 100%">
                                    <asp:TextBox ID="txt_INS_MAX_AMOUNT_Add" runat="server" onkeypress="return IsIntText();" Style="text-align: right" onpaste="return false" Width="80px" MaxLength="8" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,WFB2IB_ERR_INS_RENT_MAX_AMOUNT_Add%>"
                                        ControlToValidate="txt_INS_MAX_AMOUNT_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>

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
            <asp:HiddenField ID="HID_isAdd" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidWFB2IB_RENT_ERROR" Value="<%$Resources:Resource,wfb2IB_RENT_ERROR_Message%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidWFB2IB_CON_ERROR" Value="<%$Resources:Resource,wfb2IB_CON_ERROR_Message%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidWFB2IB_SALE_ERROR" Value="<%$Resources:Resource,wfb2IB_SALE_ERROR_Message%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidWFB2IB_STOCK_ERROR" Value="<%$Resources:Resource,wfb2IB_STOCK_ERROR_Message%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidWFB2IB_INTEREST_ERROR" Value="<%$Resources:Resource,wfb2IB_INTEREST_ERROR_Message%>" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


