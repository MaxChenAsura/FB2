<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0300_Qry.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0300_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();

        });
        function iniForm() {

            $('.year').mask('9999');
            $(".number").mask('99.99');
            $(".number").css("text-align", "right");

            $("#txt_DEPT_NAME").attr("readonly", true);
            $("#txt_NEW_DEPT_NAME").attr("readonly", true);

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
                headerrowcount: 2
            });

            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        //清空畫面
        function ClearAll() {
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#ddl_TARGET_TYPE").val("-1");
            $("#txt_TARGET_YEAR_S").val("");
            $("#txt_TARGET_YEAR_E").val("");
        }

        function getDEPT_NO(type) {
            var dept_level = "20";
            if (type == "search") {
                var dept_no = $("#txt_DEPT_NO").val();
                OpenSearch('DeptGrid_Search.aspx', 'txt_DEPT_NO', 'txt_DEPT_NAME', 'DEPT_NO=' + dept_no + '&DEPT_LEVEL=' + dept_level);
            } else {
                var new_dept_no = $("#txt_NEW_DEPT_NO").val();
                OpenSearch('DeptGrid_Search.aspx', 'txt_NEW_DEPT_NO', 'txt_NEW_DEPT_NAME', 'DEPT_NO=' + new_dept_no + '&DEPT_LEVEL=' + dept_level);
            }
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

        function getDEPT_NAME(EMP_id) {
            var txt = document.getElementById(EMP_id);
            if (txt.value != "") {
                document.getElementById('hid_getDEPT_NAME').click();
                return false;
            } else {
                $("#txt_DEPT_NAME").val("");
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static" onblur="javascript:getDEPT_NAME(this.id)"></asp:TextBox>
                                        <input id="btn_DEPT_NO" type="button" value="..." onclick="getDEPT_NO('search');" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_TARGET_TYPE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_TARGET_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_TARGET_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_TARGET_YEAR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_TARGET_YEAR%>"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_TARGET_YEAR_S" runat="server" Width="64px" CssClass="year" ClientIDMode="Static"></asp:TextBox>~
                                             <asp:TextBox ID="txt_TARGET_YEAR_E" runat="server" Width="64px" CssClass="year" ClientIDMode="Static"></asp:TextBox>
                                        <!--驗證長度需為4 -->
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_format_TARGET_YEAR_S%>" ControlToValidate="txt_TARGET_YEAR_S" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                                        <!--驗證長度需為4 -->
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_format_TARGET_YEAR_E%>" ControlToValidate="txt_TARGET_YEAR_E" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>

                                        <asp:CustomValidator ID="CustomValidatorTARGET_YEAR_S" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="年度起只能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_TARGET_YEAR_S" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidatorTARGET_YEAR_E" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="年度迄只能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_TARGET_YEAR_E" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_TARGET_YEAR_S"
                                            ControlToValidate="txt_TARGET_YEAR_E" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_YEAR_RANGE%>" Type="Integer" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left">*<asp:Label ID="Label18" runat="server" Text="<%$Resources:Resource,wfb2di_lb_Only_Ministries%>"></asp:Label>
                                    </th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DI0300Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0300Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DI0300Search_Click" />
                                            <aces:Btn ID="WFB2DI0300Set" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0300Set%>" OnClientClick="$(location).attr('href','WFB2DI0300_Emp.aspx');"/>
                                            <%--<asp:Button ID="WFB2DI0300Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0300Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DI0300Search_Click" />--%>
                                            
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2di_btn_clear%>" onclick="ClearAll();" />
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
                        <aces:Btn ID="WFB2DI0300Add" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0300Add%>" OnClick="WFB2DI0300Add_Click" />
                            <aces:Btn ID="WFB2DI0300Delete" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0300Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DI0300Delete_Click" />
                            <aces:Btn ID="WFB2DI0300Edit" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0300Edit%>" Visible="false" OnClick="WFB2DI0300Edit_Click" />
                            <aces:Btn ID="WFB2DI0300Save" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0300Save%>" Visible="false" ValidationGroup="GroupB" OnClick="WFB2DI0300Save_Click" />
                            
<%--                            <asp:Button ID="WFB2DI0300Add" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0300Add%>" OnClick="WFB2DI0300Add_Click" />
                            <asp:Button ID="WFB2DI0300Delete" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0300Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DI0300Delete_Click" />
                            <asp:Button ID="WFB2DI0300Edit" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0300Edit%>" Visible="false" OnClick="WFB2DI0300Edit_Click" />
                            <asp:Button ID="WFB2DI0300Save" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0300Save%>" Visible="false" ValidationGroup="GroupB" OnClick="WFB2DI0300Save_Click" />
                            
    --%>
                            <asp:Button ID="btn_Cancel" runat="server" Text="<%$Resources:Resource,wfb2di_btn_Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_Cancel_Click" />
                        </div>

                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DI0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_TARGET_TYPE" DefaultValue=""
                        Name="target_type" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_TARGET_YEAR_S" DefaultValue=""
                        Name="target_ym_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_TARGET_YEAR_E" DefaultValue=""
                        Name="target_ym_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>" HeaderStyle-Width="40px">
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_NO")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_NO")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static" CssClass="MandatoryField" OnTextChanged="txt_NEW_DEPT_NO_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <input id="btn_DEPT_NO" type="button" value="..." onclick="getDEPT_NO('add');" />
                                <asp:TextBox ID="txt_NEW_DEPT_NAME" runat="server" Width="120px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_DEPT_NO%>"
                                ControlToValidate="txt_NEW_DEPT_NO" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <!--驗證長度需為7 -->
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2di_format_DEPT_NO%>" ControlToValidate="txt_NEW_DEPT_NO" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression=".{7,7}" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_TARGET_TYPE%>" SortExpression="TARGET_TYPE" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_TYPE" runat="server" Text='<%#Bind("TARGET_TYPE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lb_TARGET_TYPE" runat="server" Text='<%#Bind("TARGET_TYPE")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_TARGET_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_TYPE%>"
                                ControlToValidate="ddl_NEW_TARGET_TYPE" ForeColor="Red" ValidationGroup="GroupB" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_TARGET_YEAR%>" SortExpression="TARGET_YEAR" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_YEAR" runat="server" Text='<%#Bind("TARGET_YEAR")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TARGET_YEAR" runat="server" Text='<%#Bind("TARGET_YEAR")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_TARGET_YEAR" runat="server" Width="28px" ClientIDMode="Static" CssClass="MandatoryField year"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_YEAR%>"
                                ControlToValidate="txt_NEW_TARGET_YEAR" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <!--驗證長度需為4 -->
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2di_format_TARGET_YEAR%>" ControlToValidate="txt_NEW_TARGET_YEAR" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>

                            <asp:CustomValidator ID="CustomValidatorTARGET_YEAR" runat="server" ValidateEmptyText="true"
                                ErrorMessage="年度只能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_TARGET_YEAR" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>

                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_TARGET_VALUE_01%>" SortExpression="TARGET_VALUE_01" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_VALUE_01" runat="server" Text='<%#Bind("TARGET_VALUE_01")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_TARGET_VALUE_01" runat="server" Width="35px" Text='<%#Bind("TARGET_VALUE_01")%>' CssClass="number"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_01" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_01_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_TARGET_VALUE_01" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_01" runat="server" Width="35px" CssClass="number"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_01" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_01_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_TARGET_VALUE_01" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_TARGET_VALUE_02%>" SortExpression="TARGET_VALUE_02" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_VALUE_02" runat="server" Text='<%#Bind("TARGET_VALUE_02")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_TARGET_VALUE_02" runat="server" Width="35px" Text='<%#Bind("TARGET_VALUE_02")%>' CssClass="number"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_02" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_02_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_TARGET_VALUE_02" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_02" runat="server" Width="35px" CssClass="number"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_02" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_02_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_TARGET_VALUE_02" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_TARGET_VALUE_03%>" SortExpression="TARGET_VALUE_03" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_VALUE_03" runat="server" Text='<%#Bind("TARGET_VALUE_03")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_TARGET_VALUE_03" runat="server" Width="35px" Text='<%#Bind("TARGET_VALUE_03")%>' CssClass="number"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_03" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_03_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_TARGET_VALUE_03" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_03" runat="server" Width="35px" CssClass="number"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_03" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_03_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_TARGET_VALUE_03" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_TARGET_VALUE_04%>" SortExpression="TARGET_VALUE_04" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_VALUE_04" runat="server" Text='<%#Bind("TARGET_VALUE_04")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_TARGET_VALUE_04" runat="server" Width="35px" Text='<%#Bind("TARGET_VALUE_04")%>' CssClass="number"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_04" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_04_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_TARGET_VALUE_04" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_04" runat="server" Width="35px" CssClass="number"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_04" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_04_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_TARGET_VALUE_04" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_TARGET_VALUE_05%>" SortExpression="TARGET_VALUE_05" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_VALUE_05" runat="server" Text='<%#Bind("TARGET_VALUE_05")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_TARGET_VALUE_05" runat="server" Width="35px" Text='<%#Bind("TARGET_VALUE_05")%>' CssClass="number"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_05" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_05_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_TARGET_VALUE_05" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_05" runat="server" Width="35px" CssClass="number"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_05" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_05_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_TARGET_VALUE_05" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_TARGET_VALUE_06%>" SortExpression="TARGET_VALUE_06" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_VALUE_06" runat="server" Text='<%#Bind("TARGET_VALUE_06")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_TARGET_VALUE_06" runat="server" Width="35px" Text='<%#Bind("TARGET_VALUE_06")%>' CssClass="number"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_06" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_06_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_TARGET_VALUE_06" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_06" runat="server" Width="35px" CssClass="number"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_06" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_06_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_TARGET_VALUE_06" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_TARGET_VALUE_07%>" SortExpression="TARGET_VALUE_07" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_VALUE_07" runat="server" Text='<%#Bind("TARGET_VALUE_07")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_TARGET_VALUE_07" runat="server" Width="35px" Text='<%#Bind("TARGET_VALUE_07")%>' CssClass="number"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_07" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_07_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_TARGET_VALUE_07" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_07" runat="server" Width="35px" CssClass="number"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_07" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_07_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_TARGET_VALUE_07" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_TARGET_VALUE_08%>" SortExpression="TARGET_VALUE_08" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_VALUE_08" runat="server" Text='<%#Bind("TARGET_VALUE_08")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_TARGET_VALUE_08" runat="server" Width="35px" Text='<%#Bind("TARGET_VALUE_08")%>' CssClass="number"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_08" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_08_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_TARGET_VALUE_08" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_08" runat="server" Width="35px" CssClass="number"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_08" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_08_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_TARGET_VALUE_08" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_TARGET_VALUE_09%>" SortExpression="TARGET_VALUE_09" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_VALUE_09" runat="server" Text='<%#Bind("TARGET_VALUE_09")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_TARGET_VALUE_09" runat="server" Width="35px" Text='<%#Bind("TARGET_VALUE_09")%>' CssClass="number"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_09" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_09_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_TARGET_VALUE_09" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_09" runat="server" Width="35px" CssClass="number"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_09" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_09_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_TARGET_VALUE_09" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_TARGET_VALUE_10%>" SortExpression="TARGET_VALUE_10" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_VALUE_10" runat="server" Text='<%#Bind("TARGET_VALUE_10")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_TARGET_VALUE_10" runat="server" Width="35px" Text='<%#Bind("TARGET_VALUE_10")%>' CssClass="number"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_10" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_10_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_TARGET_VALUE_10" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_10" runat="server" Width="35px" CssClass="number"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_10" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_10_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_TARGET_VALUE_10" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_TARGET_VALUE_11%>" SortExpression="TARGET_VALUE_11" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_VALUE_11" runat="server" Text='<%#Bind("TARGET_VALUE_11")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_TARGET_VALUE_11" runat="server" Width="35px" Text='<%#Bind("TARGET_VALUE_11")%>' CssClass="number"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_11" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_11_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_TARGET_VALUE_11" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_11" runat="server" Width="35px" CssClass="number"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_11" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_11_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_TARGET_VALUE_11" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_TARGET_VALUE_12%>" SortExpression="TARGET_VALUE_12" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_VALUE_12" runat="server" Text='<%#Bind("TARGET_VALUE_12")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_TARGET_VALUE_12" runat="server" Width="35px" Text='<%#Bind("TARGET_VALUE_12")%>' CssClass="number"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_12" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_12_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_TARGET_VALUE_12" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right; width: 100%">
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_12" runat="server" Width="35px" CssClass="number"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_12" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_12_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_TARGET_VALUE_12" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                   <%-- <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_TARGET_ALLOW%>" HeaderStyle-Width="80px">
                        <ItemTemplate>
                             <aces:Btn ID="WFB2DI0300Set" runat="server"
                                CommandName="ToSetup"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2di_WFB2DI0300Set%>" />
                           <%--   <asp:Button ID="WFB2DI0300Set" runat="server"
                                CommandName="ToSetup"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2di_WFB2DI0300Set%>" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <aces:Btn ID="WFB2DI0300Set" runat="server"
                                    CommandName="ToSetup"
                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    Text="<%$Resources:Resource,wfb2di_WFB2DI0300Set%>" Enabled="false" />

                              <asp:Button ID="WFB2DI0300Set" runat="server"
                                    CommandName="ToSetup"
                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    Text="<%$Resources:Resource,wfb2di_WFB2DI0300Set%>" Enabled="false" />
                            </div>
                        </EditItemTemplate>

                    </asp:TemplateField>--%>
                </Columns>
                <EmptyDataTemplate>

                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2di_RowNumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>" Width="150px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2di_lb_TARGET_TYPE%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2di_lb_TARGET_YEAR%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2di_TARGET_VALUE_01%>" Width="35px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2di_TARGET_VALUE_02%>" Width="35px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2di_TARGET_VALUE_03%>" Width="35px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2di_TARGET_VALUE_04%>" Width="35px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2di_TARGET_VALUE_05%>" Width="35px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2di_TARGET_VALUE_06%>" Width="35px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2di_TARGET_VALUE_07%>" Width="35px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2di_TARGET_VALUE_08%>" Width="35px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2di_TARGET_VALUE_09%>" Width="35px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2di_TARGET_VALUE_10%>" Width="35px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="<%$Resources:Resource,wfb2di_TARGET_VALUE_11%>" Width="35px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2di_TARGET_VALUE_12%>" Width="35px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="<%$Resources:Resource,wfb2di_TARGET_ALLOW%>"></asp:Label>
                            </td>

                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:CheckBox ID="cb_check" runat="server" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="lb_EmptyRowNumber" runat="server" Text="1" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_NEW_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static" CssClass="MandatoryField" OnTextChanged="txt_NEW_DEPT_NO_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <input id="btn_DEPT_NO" type="button" value="..." onclick="getDEPT_NO('add');" />
                                    <asp:TextBox ID="txt_NEW_DEPT_NAME" runat="server" Width="120px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_DEPT_NO%>"
                                    ControlToValidate="txt_NEW_DEPT_NO" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <!--驗證長度需為7 -->
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_format_DEPT_NO%>" ControlToValidate="txt_NEW_DEPT_NO" ForeColor="Red" ValidationGroup="GroupB"
                                    ValidationExpression=".{7,7}" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:DropDownList ID="ddl_NEW_TARGET_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_TYPE%>"
                                    ControlToValidate="ddl_NEW_TARGET_TYPE" ForeColor="Red" ValidationGroup="GroupB" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TARGET_YEAR" runat="server" Width="28px" ClientIDMode="Static" CssClass="MandatoryField year"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_YEAR%>"
                                    ControlToValidate="txt_NEW_TARGET_YEAR" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <!--驗證長度需為4 -->
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_format_TARGET_YEAR%>" ControlToValidate="txt_NEW_TARGET_YEAR" ForeColor="Red" ValidationGroup="GroupB"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="CustomValidatorTARGET_YEAR" runat="server" ValidateEmptyText="true"
                                ErrorMessage="年度只能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_TARGET_YEAR" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_01" runat="server" CssClass="number" Width="35px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_01" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_01_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TARGET_VALUE_01" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_02" runat="server" CssClass="number" Width="35px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_02" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_02_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TARGET_VALUE_02" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_03" runat="server" CssClass="number" Width="35px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_03" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_03_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TARGET_VALUE_03" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_04" runat="server" CssClass="number" Width="35px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_04" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_04_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TARGET_VALUE_04" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_05" runat="server" CssClass="number" Width="35px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_05" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_05_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TARGET_VALUE_05" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_06" runat="server" CssClass="number" Width="35px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_06" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_06_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TARGET_VALUE_06" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_07" runat="server" CssClass="number" Width="35px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_07" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_07_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TARGET_VALUE_07" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_08" runat="server" CssClass="number" Width="35px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_08" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_08_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TARGET_VALUE_08" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_09" runat="server" CssClass="number" Width="35px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_09" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_09_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TARGET_VALUE_09" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_10" runat="server" CssClass="number" Width="35px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_10" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_10_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TARGET_VALUE_10" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_11" runat="server" CssClass="number" Width="35px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_11" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_11_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TARGET_VALUE_11" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_TARGET_VALUE_12" runat="server" CssClass="number" Width="35px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorTARGET_VALUE_12" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_VALUE_12_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_TARGET_VALUE_12" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td></td>

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
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <asp:Button ID="hid_getDEPT_NAME" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getDEPT_NAME_Click" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

