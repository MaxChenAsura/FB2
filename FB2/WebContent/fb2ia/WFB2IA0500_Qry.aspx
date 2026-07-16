<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA0500_Qry.aspx.cs" Inherits="WebContent_WFB2IA0500_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".number").mask('999');
            $(".number2").mask('99');
            $(".numberr").css("text-align", "right");
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

        function CheckTARGET_TYPE(TARGET_TYPE_id) {
            var txt = document.getElementById(TARGET_TYPE_id);
            if (txt.value == "-1") {
                //alert("團保對象別不可空白");
                document.getElementById(TARGET_TYPE_id).value = "1";
            }

            $("#txt_NEW_PERSON_QTY_S").removeClass("ro").removeAttr("disabled");
            $("#txt_NEW_PERSON_QTY_E").removeClass("ro").removeAttr("disabled");
            $("#cb_NEW_HOUSE_YN").removeClass("ro").removeAttr("disabled");

            if (txt.value != 3) {
                $("#txt_NEW_PERSON_QTY_S").val("1");
                $("#txt_NEW_PERSON_QTY_S").attr("disabled", true);
                $("#cb_NEW_HOUSE_YN").attr("checked", false);
                $("#cb_NEW_HOUSE_YN").attr("disabled", true);
            }

            if (txt.value == 1 || txt.value == 2) {
                $("#txt_NEW_PERSON_QTY_E").val("1");
                $("#txt_NEW_PERSON_QTY_E").attr("disabled", true);
            }
            else if (txt.value == 4) {
                $("#txt_NEW_PERSON_QTY_E").val("2");
                $("#txt_NEW_PERSON_QTY_E").attr("disabled", true);
            }
        }

        function IsDelete() {
            var answer = confirm("確定要刪除?");
            if (answer)
                return true;
            else {
                document.getElementById('HID_cancel').click();
                return false;
            }
        }

        //清空畫面
        function ClearAll() {
            $("#ddl_TARGET_TYPE").val("-1");
            $("#ddl_GINS_KIND").val("-1");
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
                                        <asp:Label ID="lb_TARGET_TYPE" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_TARGET_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_TARGET_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GINS_KIND" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_GINS_KIND%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_GINS_KIND" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2IA0500Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Search%>" OnClick="WFB2IA0500Search_Click" OnClientClick="BlockUI();" />

                                            <%--<asp:Button ID="WFB2IA0500Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Search%>" OnClick="WFB2IA0500Search_Click" OnClientClick="BlockUI();" />--%>

                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2ia_btn_clear%>" onclick="ClearAll();" />
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
                            <aces:Btn ID="WFB2IA0500Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Add%>" OnClick="WFB2IA0500Add_Click" />
                            <aces:Btn ID="WFB2IA0500Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IA0500Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2IA0500Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Edit%>" OnClick="WFB2IA0500Edit_Click" Visible="false" />
                            <aces:Btn ID="WFB2IA0500Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Save%>" Visible="false" OnClick="WFB2IA0500Save_Click" ValidationGroup="GroupA" />

                            <%--<asp:Button ID="WFB2IA0500Add" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Add%>" OnClick="WFB2IA0500Add_Click" />
                            <asp:Button ID="WFB2IA0500Delete" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IA0500Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2IA0500Edit" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Edit%>" OnClick="WFB2IA0500Edit_Click" Visible="false" />
                            <asp:Button ID="WFB2IA0500Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Save%>" Visible="false" OnClick="WFB2IA0500Save_Click" ValidationGroup="GroupA" />
                            --%>
                            <asp:Button ID="WFB2IA0500Cancel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0500Cancel%>" Visible="false" OnClick="WFB2IA0500Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                        </div>
                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2IA0500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_TARGET_TYPE" DefaultValue=""
                        Name="target_type" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_GINS_KIND" DefaultValue=""
                        Name="gins_kind" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1020px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_TARGET_TYPE%>" SortExpression="TARGET_TYPE" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_TYPE" runat="server" Text='<%#Bind("TARGET_TYPE")%>' ToolTip='<%#Bind("SUB_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TARGET_TYPE" runat="server" Text='<%#Bind("TARGET_TYPE")%>' ToolTip='<%#Bind("SUB_CD")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_TARGET_TYPE" runat="server" ClientIDMode="Static" Width="50px"
                                    onblur="javascript:CheckTARGET_TYPE(this.id)" CssClass="MandatoryField">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_TARGET_TYPE%>"
                                ControlToValidate="ddl_NEW_TARGET_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_GINS_KIND%>" SortExpression="GINS_KIND" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_GINS_KIND" runat="server" Text='<%#Bind("GINS_KIND")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_GINS_KIND" runat="server" Text='<%#Bind("GINS_KIND")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_GINS_KIND" runat="server" ClientIDMode="Static" Width="40px" CssClass="MandatoryField">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GINS_KIND%>"
                                ControlToValidate="ddl_NEW_GINS_KIND" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_GINS_ITEM%>" SortExpression="GINS_ITEM" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_GINS_ITEM" runat="server" Text='<%#Bind("GINS_ITEM")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_GINS_ITEM" runat="server" Text='<%#Bind("GINS_ITEM")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_GINS_ITEM" runat="server" ClientIDMode="Static" Width="40px" MaxLength="1" CssClass="MandatoryField"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GINS_ITEM%>"
                                ControlToValidate="txt_NEW_GINS_ITEM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GINS_ITEM_Chinese%>" ControlToValidate="txt_NEW_GINS_ITEM" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression="[\d|a-zA-Z]" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_GINS_ITEM_NAME%>" SortExpression="GINS_ITEM_NAME" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_GINS_ITEM_NAME" runat="server" Text='<%#Bind("GINS_ITEM_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_GINS_ITEM_NAME" runat="server" Text='<%#Bind("GINS_ITEM_NAME")%>' ClientIDMode="Static" Width="80px"
                                MaxLength="60" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GINS_ITEM_NAME%>"
                                ControlToValidate="txt_GINS_ITEM_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_GINS_ITEM_NAME" runat="server" ClientIDMode="Static" Width="80px" MaxLength="60" CssClass="MandatoryField"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GINS_ITEM_NAME%>"
                                ControlToValidate="txt_NEW_GINS_ITEM_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_AMT%>" SortExpression="AMT" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_AMT" runat="server" Text='<%#Bind("AMT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_AMT" runat="server" Text='<%#Bind("AMT")%>' ClientIDMode="Static" CssClass="number numberr" Width="40px"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorAMT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_AMT_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_AMT" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_AMT" runat="server" ClientIDMode="Static" CssClass="number numberr" Width="40px" Text="0"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorAMT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_AMT_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_AMT" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_PERSON_QTY_S%>" SortExpression="PERSON_QTY_S" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_PERSON_QTY_S" runat="server" Text='<%#Bind("PERSON_QTY_S")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_PERSON_QTY_S" runat="server" Text='<%#Bind("PERSON_QTY_S")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_PERSON_QTY_S" runat="server" ClientIDMode="Static" CssClass="MandatoryField number2 numberr" Width="40px" Text="1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_PERSON_QTY_S%>"
                                ControlToValidate="txt_NEW_PERSON_QTY_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorPERSON_QTY_S" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_PERSON_QTY_S_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_PERSON_QTY_S" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_PERSON_QTY_E%>" SortExpression="PERSON_QTY_E" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_PERSON_QTY_E" runat="server" Text='<%#Bind("PERSON_QTY_E")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_PERSON_QTY_E" runat="server" Text='<%#Bind("PERSON_QTY_E")%>' ClientIDMode="Static" CssClass="number2 numberr" Width="40px"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorPERSON_QTY_E" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_PERSON_QTY_E_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_PERSON_QTY_E" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_PERSON_QTY_E" runat="server" ClientIDMode="Static" CssClass="number2 numberr" Width="40px" Text="1"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorPERSON_QTY_E" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_PERSON_QTY_E_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_PERSON_QTY_E" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_HOUSE_YN%>" SortExpression="HOUSE_YN" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_HOUSE_YN" runat="server" ClientIDMode="Static" Checked="false" Enabled="false" />
                            <asp:HiddenField ID="HID_HOUSE_YN" runat="server" Value='<%#Bind("HOUSE_YN")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_HOUSE_YN" runat="server" ClientIDMode="Static" Checked="false" Width="40px" />
                            </div>
                            <asp:HiddenField ID="HID_HOUSE_YN" runat="server" Value='<%#Bind("HOUSE_YN")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_NEW_HOUSE_YN" runat="server" ClientIDMode="Static" Width="40px" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EMP_RATE%>" SortExpression="EMP_RATE" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_RATE" runat="server" Text='<%#Bind("EMP_RATE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EMP_RATE" runat="server" Text='<%#Bind("EMP_RATE")%>' ClientIDMode="Static" CssClass="number" Width="75px"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorEMP_RATE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EMP_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EMP_RATE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_EMP_RATE" runat="server" ClientIDMode="Static" CssClass="number" Width="75px" Text="0"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorEMP_RATE" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EMP_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_EMP_RATE" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CMP_RATE%>" SortExpression="CMP_RATE" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CMP_RATE" runat="server" Text='<%#Bind("CMP_RATE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_CMP_RATE" runat="server" Text='<%#Bind("CMP_RATE")%>' ClientIDMode="Static" CssClass="number" Width="75px"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorCMP_RATE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CMP_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_CMP_RATE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_CMP_RATE" runat="server" ClientIDMode="Static" CssClass="number" Width="75px" Text="0"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorCMP_RATE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CMP_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_CMP_RATE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_UNION_RATE%>" SortExpression="UNION_RATE" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_UNION_RATE" runat="server" Text='<%#Bind("UNION_RATE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_UNION_RATE" runat="server" Text='<%#Bind("UNION_RATE")%>' ClientIDMode="Static" CssClass="number" Width="80px"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorUNION_RATE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_UNION_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_UNION_RATE" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_UNION_RATE" runat="server" ClientIDMode="Static" CssClass="number" Width="100px" Text="0"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorUNION_RATE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_UNION_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_UNION_RATE" ValidationGroup="GroupA" Display="None">
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
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_TARGET_TYPE%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_GINS_KIND%>" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_GINS_ITEM%>" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_GINS_ITEM_NAME%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_AMT%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_PERSON_QTY_S%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_PERSON_QTY_E%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_HOUSE_YN%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_RATE%>" Width="75px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CMP_RATE%>" Width="75px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_UNION_RATE%>" Width="80px"></asp:Label>
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
                                <asp:DropDownList ID="ddl_NEW_TARGET_TYPE" runat="server" ClientIDMode="Static" Width="80px"
                                    onblur="javascript:CheckTARGET_TYPE(this.id)" CssClass="MandatoryField">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_TARGET_TYPE%>"
                                    ControlToValidate="ddl_NEW_TARGET_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_GINS_KIND" runat="server" ClientIDMode="Static" Width="60px"
                                    CssClass="MandatoryField">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GINS_KIND%>"
                                    ControlToValidate="ddl_NEW_GINS_KIND" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_GINS_ITEM" runat="server" Width="60px" ClientIDMode="Static" MaxLength="1" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GINS_ITEM%>"
                                    ControlToValidate="txt_NEW_GINS_ITEM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GINS_ITEM_Chinese%>" ControlToValidate="txt_NEW_GINS_ITEM" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="[\d|a-zA-Z]" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_GINS_ITEM_NAME" runat="server" Width="80px" ClientIDMode="Static" MaxLength="60" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_GINS_ITEM_NAME%>"
                                    ControlToValidate="txt_NEW_GINS_ITEM_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_AMT" runat="server" Width="40px" ClientIDMode="Static" CssClass="number numberr" Text="0"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorAMT" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_AMT_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_AMT" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_PERSON_QTY_S" runat="server" Width="40px" ClientIDMode="Static" CssClass="MandatoryField number2 numberr" Text="1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_PERSON_QTY_S%>"
                                    ControlToValidate="txt_NEW_PERSON_QTY_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorPERSON_QTY_S" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_PERSON_QTY_S_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_PERSON_QTY_S" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_PERSON_QTY_E" runat="server" Width="40px" ClientIDMode="Static" CssClass="number2 numberr" Text="1"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorPERSON_QTY_E" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_PERSON_QTY_E_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_PERSON_QTY_E" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:CheckBox ID="cb_NEW_HOUSE_YN" runat="server" ClientIDMode="Static" Width="40px" />
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_EMP_RATE" runat="server" Width="75px" ClientIDMode="Static" CssClass="number" Text="0"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorEMP_RATE" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EMP_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_EMP_RATE" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_CMP_RATE" runat="server" Width="75px" ClientIDMode="Static" CssClass="number" Text="0"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorCMP_RATE" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CMP_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_CMP_RATE" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_UNION_RATE" runat="server" Width="80px" ClientIDMode="Static" CssClass="number" Text="0"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorUNION_RATE" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_UNION_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_UNION_RATE" ValidationGroup="GroupA" Display="None">
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

