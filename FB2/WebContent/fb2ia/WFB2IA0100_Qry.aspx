<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA0100_Qry.aspx.cs" Inherits="WebContent_WFB2IA0100_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".number").mask('999');
            $(".number").css("text-align", "right");
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

        //清空畫面
        function ClearAll() {
            $("#txt_REDUCE_CD").val("");
            $("#txt_REDUCE_DESC2").val("");
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
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
                return confirm("確定要刪除?");
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function Check_DT(source, arguments) {

            if ($("#txt_UNEFFECT_DT").val() < $("#lb_EFFECT_DT").text())
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        function Check_DT_NEW(source, arguments) {

            if ($("#txt_NEW_UNEFFECT_DT").val() < $("#txt_NEW_EFFECT_DT").val())
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
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
                                        <asp:Label ID="lb_REDUCE_CD" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_REDUCE_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_REDUCE_CD" runat="server" MaxLength="3" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REDUCE_DESC2" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_REDUCE_DESC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_REDUCE_DESC2" runat="server" MaxLength="20" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2IA0100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2IA0100Search_Click" OnClientClick="BlockUI();" />

                                            <%--<asp:Button ID="WFB2IA0100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2IA0100Search_Click" OnClientClick="BlockUI();" />--%>
                                            
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td align="right" class="Body_label">
                        <hr>
                        <div id="init_grid">
                        <aces:Btn ID="WFB2IA0100Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Add%>" OnClick="WFB2IA0100Add_Click" />
                            <aces:Btn ID="WFB2IA0100Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IA0100Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2IA0100Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Edit%>" OnClick="WFB2IA0100Edit_Click" Visible="false" />
                            <aces:Btn ID="WFB2IA0100Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Save%>" Visible="false" OnClick="WFB2IA0100Save_Click" ValidationGroup="GroupA" />

                            <%--<asp:Button ID="WFB2IA0100Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Add%>" OnClick="WFB2IA0100Add_Click" />
                            <asp:Button ID="WFB2IA0100Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IA0100Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2IA0100Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Edit%>" OnClick="WFB2IA0100Edit_Click" Visible="false" />
                            <asp:Button ID="WFB2IA0100Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Save%>" Visible="false" OnClick="WFB2IA0100Save_Click" ValidationGroup="GroupA" />--%>
                            
                            <asp:Button ID="WFB2IA0100Cancel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Cancel%>" Visible="false" OnClick="WFB2IA0100Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                        </div>
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2IA0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_REDUCE_CD"
                        Name="reduce_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_REDUCE_DESC2"
                        Name="reduce_desc" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1000px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_REDUCE_CD%>" SortExpression="REDUCE_CD" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_REDUCE_CD" runat="server" Text='<%#Bind("REDUCE_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_REDUCE_CD" runat="server" Text='<%#Bind("REDUCE_CD")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_REDUCE_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="3" Width="50px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_REDUCE_CD%>"
                                ControlToValidate="txt_NEW_REDUCE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_REDUCE_CD_Chinese%>" ControlToValidate="txt_NEW_REDUCE_CD" ForeColor="Red"
                                ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EFFECT_DT%>" SortExpression="EFFECT_DT" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EFFECT_DT" runat="server" Text='<%#Bind("EFFECT_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EFFECT_DT" runat="server" Text='<%#Bind("EFFECT_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_EFFECT_DT" runat="server" ClientIDMode="Static" CssClass="MandatoryField date" Width="70px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT%>"
                                ControlToValidate="txt_NEW_EFFECT_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorEFFECT_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_NEW_EFFECT_DT" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_REDUCE_DESC%>" SortExpression="REDUCE_DESC" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_REDUCE_DESC" runat="server" Text='<%#Bind("REDUCE_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_REDUCE_DESC" runat="server" Text='<%#Bind("REDUCE_DESC")%>' ClientIDMode="Static" CssClass="MandatoryField" MaxLength="90" Width="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_REDUCE_DESC%>"
                                ControlToValidate="txt_REDUCE_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_REDUCE_DESC" runat="server" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="90" Width="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_REDUCE_DESC%>"
                                ControlToValidate="txt_NEW_REDUCE_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_LAB_RATE%>" SortExpression="LAB_RATE" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LAB_RATE" runat="server" Text='<%#Bind("LAB_RATE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_LAB_RATE" runat="server" Text='<%#Bind("LAB_RATE")%>' CssClass="MandatoryField number" Width="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_LAB_RATE%>"
                                ControlToValidate="txt_LAB_RATE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_LAB_RATE_RANGE%>"
                                ControlToValidate="txt_LAB_RATE" Type="Integer" MinimumValue="0" MaximumValue="100"
                                ValidationGroup="GroupA" Display="None"></asp:RangeValidator>
                            <asp:CustomValidator ID="CustomValidatorLAB_RATE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_LAB_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_LAB_RATE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>

                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_LAB_RATE" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Text="0" Width="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_LAB_RATE%>"
                                ControlToValidate="txt_NEW_LAB_RATE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_LAB_RATE_RANGE%>"
                                ControlToValidate="txt_NEW_LAB_RATE" Type="Integer" MinimumValue="0" MaximumValue="100"
                                ValidationGroup="GroupA" Display="None"></asp:RangeValidator>
                            <asp:CustomValidator ID="CustomValidatorLAB_RATE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_LAB_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_LAB_RATE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_HEA_RATE%>" SortExpression="HEA_RATE" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_HEA_RATE" runat="server" Text='<%#Bind("HEA_RATE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_HEA_RATE" runat="server" Text='<%#Bind("HEA_RATE")%>' CssClass="MandatoryField number" Width="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_HEA_RATE%>"
                                ControlToValidate="txt_HEA_RATE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_HEA_RATE_RANGE%>"
                                ControlToValidate="txt_HEA_RATE" Type="Integer" MinimumValue="0" MaximumValue="100"
                                ValidationGroup="GroupA" Display="None"></asp:RangeValidator>
                            <asp:CustomValidator ID="CustomValidatorHEA_RATE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_HEA_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_HEA_RATE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_HEA_RATE" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Text="0" Width="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_HEA_RATE%>"
                                ControlToValidate="txt_NEW_HEA_RATE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_HEA_RATE_RANGE%>"
                                ControlToValidate="txt_NEW_HEA_RATE" Type="Integer" MinimumValue="0" MaximumValue="100"
                                ValidationGroup="GroupA" Display="None"></asp:RangeValidator>
                            <asp:CustomValidator ID="CustomValidatorHEA_RATE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_HEA_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_HEA_RATE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_GOV_AMOUNT%>" SortExpression="GOV_AMOUNT" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_GOV_AMOUNT" runat="server" Text='<%#Bind("GOV_AMOUNT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_GOV_AMOUNT" runat="server" Text='<%#Bind("GOV_AMOUNT")%>' CssClass="MandatoryField number" Width="50px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GOV_AMOUNT%>"
                                ControlToValidate="txt_GOV_AMOUNT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorGOV_AMOUNT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GOV_AMOUNT_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_GOV_AMOUNT" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_GOV_AMOUNT" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Text="0" Width="50px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GOV_AMOUNT%>"
                                ControlToValidate="txt_NEW_GOV_AMOUNT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorGOV_AMOUNT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GOV_AMOUNT_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_GOV_AMOUNT" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_DETAILS%>" SortExpression="REMARK" HeaderStyle-Width="250px" ItemStyle-Width="250px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_REMARK" runat="server" Text='<%#Bind("REMARK")%>' MaxLength="210" Width="220px"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" MaxLength="210" Width="220px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_UNEFFECT_DT%>" SortExpression="UNEFFECT_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_UNEFFECT_DT" runat="server" Text='<%#Bind("UNEFFECT_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_UNEFFECT_DT" runat="server" Text='<%#Bind("UNEFFECT_DT", "{0:yyyy/MM/dd}")%>' CssClass="date" Width="80px" ClientIDMode="Static"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorUNEFFECT_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_UNEFFECT_DATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_UNEFFECT_DT" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_UNEFFECT_DT_RANGE%>" ClientValidationFunction="Check_DT" ForeColor="Red"
                                ControlToValidate="txt_UNEFFECT_DT" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_UNEFFECT_DT" runat="server" ClientIDMode="Static" CssClass="date" Text="9999/12/31" Width="70px"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorUNEFFECT_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_UNEFFECT_DATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_NEW_UNEFFECT_DT" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_UNEFFECT_DT_RANGE%>" ClientValidationFunction="Check_DT_NEW" ForeColor="Red"
                                ControlToValidate="txt_NEW_UNEFFECT_DT" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>

                </Columns>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_RowNumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_REDUCE_CD%>" Width="70px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EFFECT_DT%>" Width="90px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_REDUCE_DESC%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LAB_RATE%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_HEA_RATE%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_GOV_AMOUNT%>" Width="70px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_DETAILS%>" Width="250px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_UNEFFECT_DT%>" Width="100px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:CheckBox ID="cb_check" runat="server" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="lb_NewRowNumber" runat="server" Text="1" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_REDUCE_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="3" Width="50px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_REDUCE_CD%>"
                                    ControlToValidate="txt_NEW_REDUCE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_REDUCE_CD_Chinese%>" ControlToValidate="txt_NEW_REDUCE_CD" ForeColor="Red"
                                    ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                                </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_EFFECT_DT" runat="server" ClientIDMode="Static" CssClass="MandatoryField date" Width="70px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DT%>"
                                    ControlToValidate="txt_NEW_EFFECT_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorEFFECT_DT" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_EFFECT_DT" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_REDUCE_DESC" runat="server" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="90" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_REDUCE_DESC%>"
                                    ControlToValidate="txt_NEW_REDUCE_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_LAB_RATE" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Text="0" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_LAB_RATE%>"
                                    ControlToValidate="txt_NEW_LAB_RATE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_LAB_RATE_RANGE%>"
                                    ControlToValidate="txt_NEW_LAB_RATE" Type="Integer" MinimumValue="0" MaximumValue="100"
                                    ValidationGroup="GroupA" Display="None"></asp:RangeValidator>
                                <asp:CustomValidator ID="CustomValidatorLAB_RATE" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_LAB_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_LAB_RATE" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>

                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_HEA_RATE" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Text="0" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_HEA_RATE%>"
                                    ControlToValidate="txt_NEW_HEA_RATE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_HEA_RATE_RANGE%>"
                                    ControlToValidate="txt_NEW_HEA_RATE" Type="Integer" MinimumValue="0" MaximumValue="100"
                                    ValidationGroup="GroupA" Display="None"></asp:RangeValidator>
                                <asp:CustomValidator ID="CustomValidatorHEA_RATE" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_HEA_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_HEA_RATE" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>

                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_GOV_AMOUNT" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Text="0" Width="50px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GOV_AMOUNT%>"
                                    ControlToValidate="txt_NEW_GOV_AMOUNT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorGOV_AMOUNT" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GOV_AMOUNT_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_GOV_AMOUNT" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>

                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" MaxLength="210" Width="220px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_UNEFFECT_DT" runat="server" ClientIDMode="Static" CssClass="date" Text="9999/12/31" Width="80px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorUNEFFECT_DT" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_UNEFFECT_DATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_UNEFFECT_DT" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_UNEFFECT_DT_RANGE%>" ClientValidationFunction="Check_DT_NEW" ForeColor="Red"
                                    ControlToValidate="txt_NEW_UNEFFECT_DT" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
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
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:Button ID="HID_cancel" runat="server" Style="display: none" ClientIDMode="Static" OnClick="HID_cancel_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

