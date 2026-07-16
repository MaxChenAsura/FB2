<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC1200_Dtl2.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC1200_Dtl2" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $('#txt_GROUP_NAME').attr("readonly", true);
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
                headerrowcount: 1,
                freezesize: 4

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        function CheckORDER_SEQ(source, arguments) {
            var re = /^[\d]+$/;
            if ($("#txt_ORDER_SEQ_Add").val() != "") {                                               //排序有值做檢查
                if (!re.test($("#txt_ORDER_SEQ_Add").val())) {                                       //輸入的為數字
                    arguments.IsValid = false;
                }
                else {
                    if ($("#txt_ORDER_SEQ_Add").val() >= 1 && $("#txt_ORDER_SEQ_Add").val() <= 999)    //輸入的數字在1~999間
                        arguments.IsValid = true;
                    else
                        arguments.IsValid = false;
                }
            }
            else {
                arguments.IsValid = true;
            }
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb2sc_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb2sc_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2sc_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2sc_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("_freezeitem") != -1 && ItemCheckBoxs[i].id.indexOf("_freezeheader") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function OpenSalaryGroup_Search() {
            var selectGROUP_TYPE = "GROUP_TYPE=" + $("#hid_GROUP_TYPE").val();
            OpenSearch('SalaryGroup_Search.aspx', 'txtSUB_GROUP_ID_Add', 'txt_GROUP_NAME', selectGROUP_TYPE, 'Y');
        }
        function SalaryGroup_Search(cd, desc, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#txtSUB_GROUP_ID_Add_freezeitem").val(obj.CD);
                $("#txt_GROUP_NAME_freezeitem").val(obj.DESC);
                $("#txt_GROUP_NAME").val(obj.DESC);
                $("#txtSUB_GROUP_ID_Add").val(obj.CD);
                return obj;
            }
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
                                    <%--用途別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_KIND_CD" runat="server" Text="<%$Resources:Resource,wfb2sm_KIND_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_KIND_CD_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--群組類別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GROUP_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_GROUP_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_GROUP_TYPE_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--群組代號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GROUP_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_GROUP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_GROUP_ID_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--群組名稱--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GROUP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_GROUP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_GROUP_NAME_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label"></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFB2SC1201Add" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC1201Add_Click" />
                            <aces:Btn ID="WFB2SC1201Delete" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SC1201Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2SC1201Edit" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2SC1201Edit_Click" Visible="false" />
                            <aces:Btn ID="WFB2SC1201Save" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Save%>" Visible="false" OnClick="WFB2SC1201Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupB" />
                            <%-- <asp:Button ID="WFB2SC1201Add" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC1201Add_Click" />
                            <asp:Button ID="WFB2SC1201Delete" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SC1201Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2SC1201Edit" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2SC1201Edit_Click" Visible="false" />
                            <asp:Button ID="WFB2SC1201Save" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Save%>" Visible="false" OnClick="WFB2SC1201Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupB" />--%>
                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_cancel%>" Visible="false" OnClick="btn_cancel_Click" OnClientClick="return confirm($('#hidwfb2sc_Cancel_ConfirmMessage').val());" />
                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_back%>" OnClick="btn_back_Click" />
                        </div>

                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDtlData2"
                SelectCountMethod="getDtlCount2" TypeName="CFB2SC1200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="ods1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_QDATAKEY"
                        Name="hid_qdatakey" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1540px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center;">
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lbl_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--子項目代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_SUB_GROUP_ID%>" SortExpression="GROUP_NAME" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SUB_GROUP_ID" runat="server" Width="100%" Text='<%#Bind("GROUP_NAME")%>' Style="text-align: left; word-wrap: break-word;"></asp:Label>
                            <asp:HiddenField ID="hid_SUB_GROUP_ID" runat="server" ClientIDMode="Static" Value='<%#Bind("SUB_GROUP_ID")%>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left;">
                                <asp:TextBox ID="txtSUB_GROUP_ID_Add" runat="server" ClientIDMode="Static" MaxLength="10" Width="60px" CssClass="MandatoryField"
                                    OnTextChanged="txtSUB_GROUP_ID_Add_TextChanged" AutoPostBack="true" Style="text-align: left;"></asp:TextBox>
                                <input id="bt_SalaryGroup" type="button" value="..." onclick="OpenSalaryGroup_Search();" style="width: 30px;" />
                                <asp:TextBox ID="txt_GROUP_NAME" runat="server" ClientIDMode="Static" Width="80px" BorderWidth="0"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_SUB_GROUP_ID_isNull%>"
                                ControlToValidate="txtSUB_GROUP_ID_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--資料範圍--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_DATA_SCOPE%>" SortExpression="DATA_SCOPE" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_DATA_SCOPE" runat="server" Text='<%#Bind("DATA_SCOPE_DESC")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_DATA_SCOPE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_DATA_SCOPE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--借貸別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_CD_TYPE%>" SortExpression="CD_TYPE" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lb_CD_TYPE" runat="server" Text='<%#Bind("CD_TYPE")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_CD_TYPE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="D" Text="<%$Resources:Resource,wfb2sc_lb_CD_TYPE_D%>"></asp:ListItem>
                                <asp:ListItem Value="C" Text="<%$Resources:Resource,wfb2sc_lb_CD_TYPE_C%>"></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_CD_TYPE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="D" Text="<%$Resources:Resource,wfb2sc_lb_CD_TYPE_D%>"></asp:ListItem>
                                <asp:ListItem Value="C" Text="<%$Resources:Resource,wfb2sc_lb_CD_TYPE_C%>"></asp:ListItem>
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    
                    <%--廠商別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_VOU_VENDOR_CD%>" SortExpression="VOU_VENDOR_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_VOU_VENDOR_CD" runat="server" Text='<%#Bind("VOU_VENDOR_CD")%>' Width="100%" Style="text-align: left;"></asp:Label>
                            <asp:HiddenField ID="hid_VOU_VENDOR_CD" Value ='<%#Bind("VV_CD")%>' runat="server" ClientIDMode="static" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_VOU_VENDOR_CD_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;"/>                 
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_VOU_VENDOR_CD_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;"/>                         
                        </FooterTemplate>
                    </asp:TemplateField>
                    
                    <%--傳票支付CD--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PAY_CD%>" SortExpression="PAY_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_PAY_CD" runat="server" Text='<%#Bind("PAY_CD_DESC")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_PAY_CD_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_PAY_CD_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--傳票單據種類--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_VOUCHER_FORMAT%>" SortExpression="CD_TYPE" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_VOUCHER_FORMAT" runat="server" Text='<%#Bind("VOUCHER_FORMAT")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_VOUCHER_FORMAT_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="1" Text="<%$Resources:Resource,wfb2sc_lb_VOUCHER_FORMAT_1%>"></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_VOUCHER_FORMAT_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="1" Text="<%$Resources:Resource,wfb2sc_lb_VOUCHER_FORMAT_1%>"></asp:ListItem>
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--發票格式--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_INV_TYPE%>" SortExpression="INV_TYPE" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                        <ItemTemplate>
                            <asp:Label ID="lb_INV_TYPE" runat="server" Text='<%#Bind("INV_TYPE_DESC")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_INV_TYPE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_INV_TYPE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--傳票支付對象CD--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_VOU_PAY_TARGET%>" SortExpression="VOU_PAY_TARGET" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lb_VOU_PAY_TARGET" runat="server" Text='<%#Bind("VOU_PAY_TARGET_DESC")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_VOU_PAY_TARGET_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_VOU_PAY_TARGET_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--傳票付款方式--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_VOU_PAY_TYPE%>" SortExpression="VOU_PAY_TYPE" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_VOU_PAY_TYPE" runat="server" Text='<%#Bind("VOU_PAY_TYPE_DESC")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_VOU_PAY_TYPE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_VOU_PAY_TYPE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--付款日來源--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_VOU_PAY_DT_SRCE%>" SortExpression="VOU_PAY_DT_SRC" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_VOU_PAY_DT_SRCE" runat="server" Text='<%#Bind("VOU_PAY_DT_SRC_DESC")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_VOU_PAY_DT_SRC_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_VOU_PAY_DT_SRC_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--會計科目1--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_ACCOUNTING_NO1%>" SortExpression="ACCOUNTING_NO1" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_ACCOUNTING_NO1" runat="server"  Text='<%#Bind("ACCOUNTING_NO1")%>' Width="100%"  Style="text-align: left; word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_ACCOUNTING_NO1_Add" runat="server" Text='<%#Bind("ACCOUNTING_NO1")%>' MaxLength="10" Width="100%" Style="text-align: left;" ClientIDMode="Static"></asp:TextBox>
                            <asp:HiddenField ID="hid_ACCOUNTING_NO1" runat="server" ClientIDMode="Static" Value='<%#Bind("ACCOUNTING_NO1")%>'/>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="會計科目1只能輸入英數字，且長度為10碼"
                                ControlToValidate="txt_ACCOUNTING_NO1_Add" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="^[0-9a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_ACCOUNTING_NO1_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="10" Width="100%" Style="text-align: left;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_ACCOUNTING_NO1_isNull%>"
                                ControlToValidate="txt_ACCOUNTING_NO1_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="會計科目1只能輸入英數字，且長度為10碼"
                                ControlToValidate="txt_ACCOUNTING_NO1_Add" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="^[0-9a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--會計科目2--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_ACCOUNTING_NO2%>" SortExpression="ACCOUNTING_NO2"  HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_ACCOUNTING_NO2" runat="server" Width="100%" Text='<%#Bind("ACCOUNTING_NO2")%>' Style="text-align: left; word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_ACCOUNTING_NO2_Add" runat="server" Text='<%#Bind("ACCOUNTING_NO2")%>' MaxLength="10" Width="100%" Style="text-align: left;" ClientIDMode="Static"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator42" runat="server" ErrorMessage="會計科目2只能輸入英數字，且長度為10碼"
                                ControlToValidate="txt_ACCOUNTING_NO2_Add" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="^[0-9a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_ACCOUNTING_NO2_Add" runat="server" ClientIDMode="Static" MaxLength="10" Width="100%" Style="text-align: left;"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator52" runat="server" ErrorMessage="會計科目2只能輸入英數字，且長度為10碼"
                                ControlToValidate="txt_ACCOUNTING_NO2_Add" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="^[0-9a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--會計科目3--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_ACCOUNTING_NO3%>" SortExpression="ACCOUNTING_NO3"  HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_ACCOUNTING_NO3" runat="server" Width="100%" Text='<%#Bind("ACCOUNTING_NO3")%>' Style="text-align: left; word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_ACCOUNTING_NO3_Add" runat="server" Text='<%#Bind("ACCOUNTING_NO3")%>' MaxLength="10" Width="100%" Style="text-align: left;" ClientIDMode="Static"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator43" runat="server" ErrorMessage="會計科目3只能輸入英數字，且長度為10碼"
                                ControlToValidate="txt_ACCOUNTING_NO3_Add" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="^[0-9a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_ACCOUNTING_NO3_Add" runat="server" ClientIDMode="Static" MaxLength="10" Width="100%" Style="text-align: left;"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator53" runat="server" ErrorMessage="會計科目3只能輸入英數字，且長度為10碼"
                                ControlToValidate="txt_ACCOUNTING_NO3_Add" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="^[0-9a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--會計科目4--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_ACCOUNTING_NO4%>" SortExpression="ACCOUNTING_NO4"  HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_ACCOUNTING_NO4" runat="server" Width="100%" Text='<%#Bind("ACCOUNTING_NO4")%>' Style="text-align: left; word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_ACCOUNTING_NO4_Add" runat="server" Text='<%#Bind("ACCOUNTING_NO4")%>' MaxLength="10" Width="100%" Style="text-align: left;" ClientIDMode="Static"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator44" runat="server" ErrorMessage="會計科目4只能輸入英數字，且長度為10碼"
                                ControlToValidate="txt_ACCOUNTING_NO4_Add" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="^[0-9a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_ACCOUNTING_NO4_Add" runat="server" ClientIDMode="Static" MaxLength="10" Width="100%" Style="text-align: left;"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator54" runat="server" ErrorMessage="會計科目4只能輸入英數字，且長度為10碼"
                                ControlToValidate="txt_ACCOUNTING_NO4_Add" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="^[0-9a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--會計科目5--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_ACCOUNTING_NO5%>" SortExpression="ACCOUNTING_NO5"  HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_ACCOUNTING_NO5" runat="server" Width="100%" Text='<%#Bind("ACCOUNTING_NO5")%>' HeaderStyle-Width="100%" Style="text-align: left; word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_ACCOUNTING_NO5_Add" runat="server" Text='<%#Bind("ACCOUNTING_NO5")%>' MaxLength="10" Width="100%" Style="text-align: left;" ClientIDMode="Static"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator45" runat="server" ErrorMessage="會計科目5只能輸入英數字，且長度為10碼"
                                ControlToValidate="txt_ACCOUNTING_NO5_Add" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="^[0-9a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_ACCOUNTING_NO5_Add" runat="server" ClientIDMode="Static" MaxLength="10" Width="100%" Style="text-align: left;"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator55" runat="server" ErrorMessage="會計科目5只能輸入英數字，且長度為10碼"
                                ControlToValidate="txt_ACCOUNTING_NO5_Add" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="^[0-9a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--分攤與否--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_IS_SHARE%>" SortExpression="IS_SHARE" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                       <ItemTemplate>
                            <asp:Label ID="lb_IS_SHARE" runat="server" Text='<%#Bind("IS_SHARE")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_IS_SHARE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;">
                                <asp:ListItem Value="Y" Text="<%$Resources:Resource,wfb2sc_lb_IS_SHARE_Y%>"></asp:ListItem>
                                <asp:ListItem Value="N" Text="<%$Resources:Resource,wfb2sc_lb_IS_SHARE_N%>"></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_IS_SHARE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;">
                                <asp:ListItem Value="Y" Text="<%$Resources:Resource,wfb2sc_lb_IS_SHARE_Y%>"></asp:ListItem>
                                <asp:ListItem Value="N" Text="<%$Resources:Resource,wfb2sc_lb_IS_SHARE_N%>"></asp:ListItem>
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--預算部屬--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_BUDGET_DEPT%>" SortExpression="BUDGET_DEPT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_BUDGET_DEPT" runat="server" Width="100%" Text='<%#Bind("BUDGET_DEPT")%>' Style="text-align: left; word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_BUDGET_DEPT_Add" runat="server" Text='<%#Bind("BUDGET_DEPT")%>' MaxLength="7" Width="100%" Style="text-align: left;" ClientIDMode="Static"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidato81" runat="server" ErrorMessage="預算部屬只能輸入英數字"
                                ControlToValidate="txt_BUDGET_DEPT_Add" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="^([\d|a-zA-Z])+$" Display="None"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_BUDGET_DEPT_Add" runat="server" MaxLength="7" ClientIDMode="Static" Style="text-align: left;" Width="100%"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ErrorMessage="預算部屬只能輸入英數字"
                                ControlToValidate="txt_BUDGET_DEPT_Add" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="^([\d|a-zA-Z])+$" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--傳票摘要--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_MEMO%>" SortExpression="MEMO" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_MEMO" runat="server" Width="100%" Text='<%#Bind("MEMO")%>' Style="text-align: left; word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_MEMO_Add" runat="server" Text='<%#Bind("MEMO")%>' MaxLength="20" Width="100%" Style="text-align: left;" ClientIDMode="Static"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_MEMO_Add" runat="server" ClientIDMode="Static" MaxLength="20" Width="100%" Style="text-align: left;"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>

                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
                <EmptyDataTemplate>
                    <div style="width: 1020px; overflow: auto;">
                        <table class="grid-view" width="1350px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                            <tr class="header">
                                <td width="30px"></td>
                                <td width="40px">
                                    <asp:Label ID="lbHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb2sc_RowNumber%>"></asp:Label>
                                </td>
                                <td width="200px">
                                    <asp:Label ID="lbHeaderSUB_GROUP_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_SUB_GROUP_ID%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lbHeaderDATA_SCOPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_DATA_SCOPE%>"></asp:Label>
                                </td>
                                <td width="60px">
                                    <asp:Label ID="lbHeaderCD_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_CD_TYPE%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lbHeaderVOU_VENDOR_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_VOU_VENDOR_CD%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lbHeaderPAY_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PAY_CD%>"></asp:Label>
                                </td>
                                <td width="120px">
                                    <asp:Label ID="lbHeaderVOUCHER_FORMAT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_VOUCHER_FORMAT%>"></asp:Label>
                                </td>
                                <td width="200px">
                                    <asp:Label ID="lbHeaderINV_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_INV_TYPE%>"></asp:Label>
                                </td>
                                <td width="150px">
                                    <asp:Label ID="lbHeaderVOU_PAY_TARGET" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_VOU_PAY_TARGET%>"></asp:Label>
                                </td>
                                <td width="120px">
                                    <asp:Label ID="lbHeaderVOU_PAY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_VOU_PAY_TYPE%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lbHeaderVOU_PAY_DT_SRC" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_VOU_PAY_DT_SRCE%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lbHeaderACCOUNTING_NO1" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_ACCOUNTING_NO1%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lbHeaderACCOUNTING_NO2" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_ACCOUNTING_NO2%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lb_ACCOUNTING_NO3" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_ACCOUNTING_NO3%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lb_ACCOUNTING_NO4" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_ACCOUNTING_NO4%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lb_ACCOUNTING_NO5" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_ACCOUNTING_NO5%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lb_IS_SHARE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_IS_SHARE%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lbHeaderBUDGET_DEPT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_BUDGET_DEPT%>"></asp:Label>
                                </td>
                                <td width="100px">
                                    <asp:Label ID="lbHeaderMEMO" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_MEMO%>"></asp:Label>
                                </td>
                            </tr>
                            <tr class="normal">
                                <td></td>
                                <td></td>
                                <td>
                                    <div style="text-align: left;">
                                        <asp:TextBox ID="txtSUB_GROUP_ID_Add" runat="server" ClientIDMode="Static" MaxLength="10" Width="60px" CssClass="MandatoryField"
                                            OnTextChanged="txtSUB_GROUP_ID_Add_TextChanged" AutoPostBack="true" Style="text-align: left;"></asp:TextBox>
                                        <input id="bt_SalaryGroup" type="button" value="..." onclick="OpenSalaryGroup_Search();" style="width: 30px;" />
                                        <asp:TextBox ID="txt_GROUP_NAME" runat="server" ClientIDMode="Static" Width="70px" BorderWidth="0"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_SUB_GROUP_ID_isNull%>"
                                            ControlToValidate="txtSUB_GROUP_ID_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:DropDownList ID="ddl_DATA_SCOPE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:DropDownList ID="ddl_CD_TYPE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="D" Text="<%$Resources:Resource,wfb2sc_lb_CD_TYPE_D%>"></asp:ListItem>
                                            <asp:ListItem Value="C" Text="<%$Resources:Resource,wfb2sc_lb_CD_TYPE_C%>"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:DropDownList ID="ddl_VOU_VENDOR_CD_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;"/>  
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:DropDownList ID="ddl_PAY_CD_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:DropDownList ID="ddl_VOUCHER_FORMAT_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="1" Text="<%$Resources:Resource,wfb2sc_lb_VOUCHER_FORMAT_1%>"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center; width: 100%">
                                        <asp:DropDownList ID="ddl_INV_TYPE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:DropDownList ID="ddl_VOU_PAY_TARGET_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:DropDownList ID="ddl_VOU_PAY_TYPE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:DropDownList ID="ddl_VOU_PAY_DT_SRC_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:TextBox ID="txt_ACCOUNTING_NO1_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="10" Width="100%" Style="text-align: left;"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_ACCOUNTING_NO1_isNull%>"
                                            ControlToValidate="txt_ACCOUNTING_NO1_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="會計科目1只能輸入英數字，且長度為10碼"
                                            ControlToValidate="txt_ACCOUNTING_NO1_Add" ForeColor="Red" ValidationGroup="GroupB"
                                            ValidationExpression="^([\d|a-zA-Z]{5})+$" Display="None"></asp:RegularExpressionValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:TextBox ID="txt_ACCOUNTING_NO2_Add" runat="server" ClientIDMode="Static" MaxLength="10" Width="100%" Style="text-align: left;"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ErrorMessage="會計科目2只能輸入英數字，且長度為10碼"
                                            ControlToValidate="txt_ACCOUNTING_NO2_Add" ForeColor="Red" ValidationGroup="GroupB"
                                            ValidationExpression="^([\d|a-zA-Z]{5})+$" Display="None"></asp:RegularExpressionValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:TextBox ID="txt_ACCOUNTING_NO3_Add" runat="server" ClientIDMode="Static" MaxLength="10" Width="100%" Style="text-align: left;"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="會計科目3只能輸入英數字，且長度為10碼"
                                            ControlToValidate="txt_ACCOUNTING_NO3_Add" ForeColor="Red" ValidationGroup="GroupB"
                                            ValidationExpression="^([\d|a-zA-Z]{5})+$" Display="None"></asp:RegularExpressionValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:TextBox ID="txt_ACCOUNTING_NO4_Add" runat="server" ClientIDMode="Static" MaxLength="10" Width="100%" Style="text-align: left;"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="會計科目4只能輸入英數字，且長度為10碼"
                                            ControlToValidate="txt_ACCOUNTING_NO4_Add" ForeColor="Red" ValidationGroup="GroupB"
                                            ValidationExpression="^([\d|a-zA-Z]{5})+$" Display="None"></asp:RegularExpressionValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:TextBox ID="txt_ACCOUNTING_NO5_Add" runat="server" ClientIDMode="Static" MaxLength="10" Width="100%" Style="text-align: left;"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ErrorMessage="會計科目5只能輸入英數字，且長度為10碼"
                                            ControlToValidate="txt_ACCOUNTING_NO5_Add" ForeColor="Red" ValidationGroup="GroupB"
                                            ValidationExpression="^([\d|a-zA-Z]{5})+$" Display="None"></asp:RegularExpressionValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                       <asp:DropDownList ID="ddl_IS_SHARE_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;">
                                           <asp:ListItem Value="Y" Text="<%$Resources:Resource,wfb2sc_lb_IS_SHARE_Y%>"></asp:ListItem>
                                           <asp:ListItem Value="N" Text="<%$Resources:Resource,wfb2sc_lb_IS_SHARE_N%>"></asp:ListItem>
                                       </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:TextBox ID="txt_BUDGET_DEPT_Add" runat="server" MaxLength="7" ClientIDMode="Static" Style="text-align: left;" Width="100%"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="預算部屬只能輸入英數字"
                                            ControlToValidate="txt_BUDGET_DEPT_Add" ForeColor="Red" ValidationGroup="GroupB"
                                            ValidationExpression="^([\d|a-zA-Z])+$" Display="None"></asp:RegularExpressionValidator>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center;">
                                        <asp:TextBox ID="txt_MEMO_Add" runat="server" ClientIDMode="Static" MaxLength="20" Width="100%" Style="text-align: left;"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </EmptyDataTemplate>
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
            <asp:HiddenField ID="hid_QDATAKEY" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_KIND_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_GROUP_TYPE" runat="server" ClientIDMode="static" />
            <asp:HiddenField ID="hid_GROUP_ID" runat="server" ClientIDMode="static" />            

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Del_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Mod_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Cancel_ConfirmMessage%>" />
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


