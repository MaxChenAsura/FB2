<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0300_Qry.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0300_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            gridviewScroll();
            $.unblockUI();
            $('#txt_REMARK_Add').change(function () {
                if ($('#txt_REMARK_Add').val().length > 150) {
                    $('#txt_REMARK_Add').val($('#txt_REMARK_Add').val().substring(0, 150));

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
        function ClearAll() {
            $('#txt_ACC_DEPT_NO').val("");
            $('#txt_ACC_DEPT_NAME').val("");
            $('#txt_COST_DEPT_NO').val("");
            $('#txt_BUDGET_DEPT_NO').val("");
            $('#ddl_CAR_TYPE').val(-1);
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
                alert($('#hidwfb299_Mod_NotChoiceMessage').val());
                return false;
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
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--2HA0300_QRY開始-->
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="15%" />
                                <col width="15%" />
                                <col width="15%" />
                                <col width="15%" />
                                <col width="25%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="ACC_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2ha03_lb_acc_dept_no%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_ACC_DEPT_NO" runat="server" MaxLength="7" Style="TEXT-TRANSFORM: uppercase" Columns="10" ClientIDMode="Static" onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="ACC_DEPT_NAME" runat="server" Text="<%$Resources:Resource,wfb2ha03_lb_acc_dept_name%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_ACC_DEPT_NAME" runat="server" MaxLength="30" Columns="10" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="CAR_TYPE" runat="server" Text="<%$Resources:Resource,wfb2ha03_lb_car_type%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_CAR_TYPE" runat="server" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="COST_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2ha03_lb_cost_dept_no%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_COST_DEPT_NO" runat="server" Style="TEXT-TRANSFORM: uppercase" MaxLength="7" Columns="7" ClientIDMode="Static" onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="BUDGET_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2ha03_lb_budget_dept_no%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_BUDGET_DEPT_NO" runat="server" MaxLength="7" Style="TEXT-TRANSFORM: uppercase" Columns="7" ClientIDMode="Static" onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="IS_VALID" runat="server" Text="<%$Resources:Resource,wfb2ha03_lb_is_valid%>"></asp:Label>:
                                    </th>
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
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="10">
                                        <div id="init">
                                            <aces:Btn ID="WFB2HA0300Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2HA0300Search_Click" OnClientClick="BlockUI();" />
                                            <%--<asp:Button ID="WFB2HA0300Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2HA0300Search_Click" OnClientClick="BlockUI();" />--%>
                                            <asp:Button ID="WFB2HA0300Clear" runat="server" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="ClearAll();" />
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
                                                    
                            <aces:Btn ID="WFB2HA0300Add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="WFB2HA0300Add_Click" />
                            <aces:Btn ID="WFB2HA0300Delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2HA0300Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2HA0300Edit" runat="server" Text="<%$Resources:Resource,btn_edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HA0300Edit_Click" Visible="False" />
                            <aces:Btn ID="WFB2HA0300Save" runat="server" Text="<%$Resources:Resource,btn_ok%>" Visible="false" OnClick="WFB2HA0300Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                            
                            <%--<asp:Button ID="WFB2HA0300Add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="WFB2HA0300Add_Click" />
                            <asp:Button ID="WFB2HA0300Delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2HA0300Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2HA0300Edit" runat="server" Text="<%$Resources:Resource,btn_edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HA0300Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2HA0300Save" runat="server" Text="<%$Resources:Resource,btn_ok%>" Visible="false" OnClick="WFB2HA0300Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                            <asp:Button ID="WFB2HA0300Cancel" runat="server" Text="<%$Resources:Resource,btn_cancel%>" Visible="false" OnClick="WFB2HA0300Cancel_Click" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val())" />
                        </div>

                    </td>
                </tr>
            </table>
            <!--2HA0300_QRY結束-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HA0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_ACC_DEPT_NO"
                        Name="sys_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_CAR_TYPE"
                        Name="ddl_CAR_TYPE" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_ACC_DEPT_NAME"
                        Name="txt_ACC_DEPT_NAME" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_COST_DEPT_NO"
                        Name="txt_COST_DEPT_NO" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_BUDGET_DEPT_NO"
                        Name="txt_BUDGET_DEPT_NO" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_IS_VALID"
                        Name="ddl_IS_VALID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />

                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" HeaderStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="50px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbl_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="50px"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_ACC_DEPT_DESC%>" SortExpression="ACC_DEPT_NO" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_ACC_DEPT_NO" runat="server" Text='<%#Bind("ACC_DEPT_NO")%>' Width="140px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="txt_ACC_DEPT_NO_Add" runat="server" Text='<%#Bind("ACC_DEPT_NO")%>' Width="140px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_ACC_DEPT_NO_Add" runat="server" MaxLength="7" Width="140px" ClientIDMode="Static" Style="TEXT-TRANSFORM: uppercase" CssClass="MandatoryField" onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator52" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_acc_dept_no%>"
                                ControlToValidate="txt_ACC_DEPT_NO_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_ACC_DEPT_NAME%>" SortExpression="ACC_DEPT_NAME" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_ACC_DEPT_NAME" runat="server" Text='<%#Bind("ACC_DEPT_NAME")%>' Width="140px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_ACC_DEPT_NAME_Add" MaxLength="30" ClientIDMode="Static" runat="server" Width="140px" CssClass="MandatoryField" Text='<%#Bind("ACC_DEPT_NAME")%>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_acc_dept_name%>"
                                ControlToValidate="txt_ACC_DEPT_NAME_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_ACC_DEPT_NAME_Add" runat="server" MaxLength="30" Width="140px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_acc_dept_name%>"
                                ControlToValidate="txt_ACC_DEPT_NAME_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_CAR_TYPE%>" SortExpression="CAR_TYPE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_CAR_TYPE" runat="server" Text='<%# Convert.ToString(Eval("CAR_TYPE")) +"-"+ Convert.ToString(Eval("SUB_DESC"))%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_CAR_TYPE_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField" Width="100px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_car_type%>"
                                ControlToValidate="ddl_CAR_TYPE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_CAR_TYPE_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField" Width="100px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_car_type%>"
                                ControlToValidate="ddl_CAR_TYPE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_COST_DEPT_NO%>" SortExpression="COST_DEPT_NO" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_COST_DEPT_NO" runat="server" Text='<%#Bind("COST_DEPT_NO")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_COST_DEPT_NO_Add" MaxLength="7" ClientIDMode="Static" runat="server" Width="120px" Style="TEXT-TRANSFORM: uppercase" Text='<%#Bind("COST_DEPT_NO")%>' CssClass="MandatoryField" onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_cost_dept_no%>"
                                ControlToValidate="txt_COST_DEPT_NO_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_COST_DEPT_NO_Add" runat="server" MaxLength="7" ClientIDMode="Static" Width="120px" CssClass="MandatoryField" Style="TEXT-TRANSFORM: uppercase" onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_cost_dept_no%>"
                                ControlToValidate="txt_COST_DEPT_NO_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_BUDGET_DEPT_NO%>" SortExpression="BUDGET_DEPT_NO" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_BUDGET_DEPT_NO" runat="server" Text='<%#Bind("BUDGET_DEPT_NO")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_BUDGET_DEPT_NO_Add" MaxLength="7" ClientIDMode="Static" runat="server" Width="120px" Style="TEXT-TRANSFORM: uppercase" CssClass="MandatoryField" Text='<%#Bind("BUDGET_DEPT_NO")%>' onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_budget_dept_no%>"
                                ControlToValidate="txt_BUDGET_DEPT_NO_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_BUDGET_DEPT_NO_Add" runat="server" MaxLength="7" ClientIDMode="Static" Width="120px" Style="TEXT-TRANSFORM: uppercase" CssClass="MandatoryField" onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_budget_dept_no%>"
                                ControlToValidate="txt_BUDGET_DEPT_NO_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_IS_VALID%>" SortExpression="IS_VALID" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Label ID="lbl_IS_VALID" runat="server" Text='<%#Bind("IS_VALID")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_IS_VALID_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField" Width="80px">
                                <asp:ListItem Text=" " Value=" "></asp:ListItem>
                                <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="N" Value="N"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_is_valid%>"
                                ControlToValidate="ddl_IS_VALID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_IS_VALID_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField" Width="80px">
                                <asp:ListItem Text=" " Value=" "></asp:ListItem>
                                <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="N" Value="N"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_is_valid%>"
                                ControlToValidate="ddl_IS_VALID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="400" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_REMARK_Add" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("REMARK")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_REMARK_Add" runat="server" MaxLength="150" ClientIDMode="Static"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td></td>
                            <td>
                                <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb2_RowNumber%>" Width="50px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHeaderACC_DEPT_DESC" runat="server" Text="<%$Resources:Resource,wfb2ha_ACC_DEPT_DESC%>" Width="140px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHeaderACC_DEPT_NAME" runat="server" Text="<%$Resources:Resource,wfb2ha_ACC_DEPT_NAME%>" Width="140px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHeaderCAR_TYPE" runat="server" Text="<%$Resources:Resource,wfb2ha_CAR_TYPE%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHeaderCOST_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2ha_COST_DEPT_NO%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHeaderBUDGET_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2ha_BUDGET_DEPT_NO%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHeaderIS_VALID" runat="server" Text="<%$Resources:Resource,wfb2ha_IS_VALID%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHeaderREMARK" runat="server" Text="<%$Resources:Resource,wfb2ha_REMARK%>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_ACC_DEPT_NO_Add" runat="server" MaxLength="7" Style="TEXT-TRANSFORM: uppercase" Width="140px" ClientIDMode="Static" CssClass="MandatoryField" onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_acc_dept_name%>"
                                    ControlToValidate="txt_ACC_DEPT_NO_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_ACC_DEPT_NAME_Add" runat="server" MaxLength="30" Width="140px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_acc_dept_name%>"
                                    ControlToValidate="txt_ACC_DEPT_NAME_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_CAR_TYPE_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField" Width="100px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_car_type%>"
                                    ControlToValidate="ddl_CAR_TYPE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_COST_DEPT_NO_Add" runat="server" MaxLength="7" Style="TEXT-TRANSFORM: uppercase" ClientIDMode="Static" Width="120px" CssClass="MandatoryField" onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_cost_dept_no%>"
                                    ControlToValidate="txt_COST_DEPT_NO_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_BUDGET_DEPT_NO_Add" runat="server" MaxLength="7" Style="TEXT-TRANSFORM: uppercase" ClientIDMode="Static" Width="120px" CssClass="MandatoryField" onkeyup="value=value.replace(/[\W]/g,'')"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_budget_dept_no%>"
                                    ControlToValidate="txt_BUDGET_DEPT_NO_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_IS_VALID_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField" Width="80px">
                                    <asp:ListItem Text=" " Value=" "></asp:ListItem>
                                    <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_is_valid%>"
                                    ControlToValidate="ddl_IS_VALID_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_REMARK_Add" MaxLength="150" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
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


